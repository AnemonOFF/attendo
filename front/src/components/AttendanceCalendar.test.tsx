/* eslint-disable sonarjs/no-duplicate-string */
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { vi } from "vitest";

import AttendanceCalendar from "./AttendanceCalendar";

const hooksMocks = vi.hoisted(() => ({
  useGroups: vi.fn(),
  useClasses: vi.fn(),
}));

vi.mock("../hooks/useGroups", () => ({
  useGroups: hooksMocks.useGroups,
}));

vi.mock("../hooks/useClasses", () => ({
  useClasses: hooksMocks.useClasses,
}));

const mockNavigate = vi.fn();

vi.mock("react-router-dom", async () => {
  const actual =
    await vi.importActual<typeof import("react-router-dom")>(
      "react-router-dom",
    );
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  };
});

const computeWeekStart = () => {
  const today = new Date();
  const dayOfWeek = today.getDay();
  const diff = dayOfWeek === 0 ? -6 : 1 - dayOfWeek;
  const monday = new Date(today);
  monday.setDate(today.getDate() + diff);
  monday.setHours(12, 0, 0, 0);
  return monday;
};

const weekStart = computeWeekStart();
const dateForOffset = (offset: number) => {
  const date = new Date(weekStart);
  date.setDate(weekStart.getDate() + offset);
  return date.toISOString();
};

const yogaGroup = { id: 1, title: "Yoga", students: [] };
const pilatesGroup = { id: 2, title: "Pilates", students: [] };

const mockGroups = [yogaGroup, pilatesGroup];

const mockClasses = [
  {
    id: 1,
    name: "Morning Yoga",
    start: dateForOffset(0),
    end: dateForOffset(0),
    frequency: "Weekly",
    startTime: "08:00",
    endTime: "09:00",
    group: yogaGroup,
  },
  {
    id: 2,
    name: "Mat Pilates",
    start: dateForOffset(2),
    end: dateForOffset(2),
    frequency: "Weekly",
    startTime: "10:00",
    endTime: "11:00",
    group: pilatesGroup,
  },
];

describe("AttendanceCalendar", () => {
  beforeEach(() => {
    mockNavigate.mockReset();
    hooksMocks.useGroups.mockReset();
    hooksMocks.useClasses.mockReset();
    hooksMocks.useGroups.mockReturnValue({ data: mockGroups });
    hooksMocks.useClasses.mockReturnValue({ data: mockClasses });
  });

  it("updates week range when navigating between weeks", async () => {
    const user = userEvent.setup();
    render(<AttendanceCalendar />);

    const [weekLabel] = screen.getAllByText(
      (content, element) =>
        element?.tagName === "SPAN" && content.includes(" - "),
    );
    const initialRange = weekLabel.textContent;

    await user.click(screen.getByLabelText(/next week/i));
    expect(weekLabel.textContent).not.toBe(initialRange);

    await user.click(screen.getByLabelText(/previous week/i));
    expect(weekLabel.textContent).toBe(initialRange);
  });

  it("filters classes when a specific group is selected", async () => {
    const user = userEvent.setup();
    render(<AttendanceCalendar />);

    await user.click(screen.getByRole("button", { name: /all groups/i }));
    await user.click(screen.getByText("Pilates"));

    expect(screen.getByText("Mat Pilates")).toBeInTheDocument();
    expect(screen.queryByText("Morning Yoga")).not.toBeInTheDocument();
  });

  it("navigates to other screens from quick actions", async () => {
    const user = userEvent.setup();
    render(<AttendanceCalendar />);

    await user.click(
      screen.getByRole("button", {
        name: /add class/i,
      }),
    );
    expect(mockNavigate).toHaveBeenCalledWith("/addClass");

    mockNavigate.mockClear();
    await user.click(screen.getByLabelText(/add group/i));
    expect(mockNavigate).toHaveBeenCalledWith("/addGroup");

    mockNavigate.mockClear();
    await user.click(screen.getByText("Morning Yoga"));
    expect(mockNavigate).toHaveBeenCalledWith("/classInfo", {
      state: expect.objectContaining({
        id: 1,
        name: "Morning Yoga",
      }),
    });
  });
});
