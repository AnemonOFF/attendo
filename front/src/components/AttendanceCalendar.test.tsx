/* eslint-disable sonarjs/no-duplicate-string */
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { vi } from "vitest";

import AttendanceCalendar from "./AttendanceCalendar";

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

describe("AttendanceCalendar", () => {
  beforeEach(() => {
    mockNavigate.mockReset();
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
        id: "1",
        name: "Morning Yoga",
      }),
    });
  });
});
