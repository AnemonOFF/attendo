/* eslint-disable sonarjs/no-duplicate-string */
import { render, screen, waitFor } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { vi } from "vitest";

import ClassAttendanceScreen from "./ClassAttendanceScreen";

const mockGroups = [
  { id: 1, name: "Beginner Flow", color: "#E5E7EB" },
  { id: 2, name: "Power Lunch", color: "#FDE68A" },
  { id: 3, name: "Advanced Yoga", color: "#A7F3D0" },
];

const mockStudents = [
  { id: 1, name: "Alice Johnson", groupId: 1 },
  { id: 2, name: "Bob Smith", groupId: 1 },
  { id: 3, name: "Carla Gomez", groupId: 1 },
  { id: 4, name: "David Lee", groupId: 1 },
  { id: 5, name: "Eva Brown", groupId: 1 },
  { id: 6, name: "Ian Malcolm", groupId: 3 },
];

const apiMocks = vi.hoisted(() => ({
  getGroups: vi.fn(),
  getStudentsByGroup: vi.fn(),
  updateAttendance: vi.fn(),
}));

vi.mock("../api/groupController", () => ({
  getGroups: apiMocks.getGroups,
}));

vi.mock("../api/studentController", () => ({
  getStudentsByGroup: apiMocks.getStudentsByGroup,
}));

vi.mock("../api/attendanceController", () => ({
  updateAttendance: apiMocks.updateAttendance,
}));

const mockNavigate = vi.fn();
const mockLocationState = { id: 101, name: "Morning Yoga" };

vi.mock("react-router-dom", async () => {
  const actual =
    await vi.importActual<typeof import("react-router-dom")>(
      "react-router-dom",
    );
  return {
    ...actual,
    useNavigate: () => mockNavigate,
    useLocation: () => ({
      pathname: "/classInfo",
      search: "",
      hash: "",
      state: mockLocationState,
      key: "test",
    }),
  };
});

describe("ClassAttendanceScreen", () => {
  beforeEach(() => {
    mockNavigate.mockReset();
    apiMocks.getGroups.mockReset();
    apiMocks.getStudentsByGroup.mockReset();
    apiMocks.updateAttendance.mockReset();

    apiMocks.getGroups.mockResolvedValue({
      data: { items: mockGroups },
    });

    apiMocks.getStudentsByGroup.mockImplementation(
      (groupId: number | undefined) => {
        const students = mockStudents.filter(
          (student) => student.groupId === groupId,
        );
        return Promise.resolve({ data: students });
      },
    );

    apiMocks.updateAttendance.mockResolvedValue({});
  });

  it("renders class name from navigation state and filters students", async () => {
    const user = userEvent.setup();
    render(<ClassAttendanceScreen />);

    expect(await screen.findByText("Morning Yoga")).toBeInTheDocument();

    const groupSelect = await screen.findByRole("combobox");
    await user.selectOptions(groupSelect, "3");

    expect(await screen.findByText("Ian Malcolm")).toBeInTheDocument();
    await waitFor(() =>
      expect(screen.queryByText("Alice Johnson")).not.toBeInTheDocument(),
    );
  });

  it("allows toggling attendance for individual students", async () => {
    const user = userEvent.setup();
    render(<ClassAttendanceScreen />);

    const firstCheckbox = (await screen.findAllByRole("checkbox"))[0];
    expect(firstCheckbox).not.toBeChecked();

    await user.click(firstCheckbox);
    expect(firstCheckbox).toBeChecked();

    await user.click(firstCheckbox);
    expect(firstCheckbox).not.toBeChecked();
  });

  it("toggles attendance for an entire day when header is clicked", async () => {
    const user = userEvent.setup();
    render(<ClassAttendanceScreen />);

    const dayHeaders = await screen.findAllByRole("columnheader");
    const firstDayHeader = dayHeaders[1]; // index 0 is "Student Name"

    await user.click(firstDayHeader);

    const mondayCheckboxes = screen
      .getAllByRole("checkbox")
      .filter((_, index) => index % 7 === 0);

    expect(mondayCheckboxes).toHaveLength(5);
    mondayCheckboxes.forEach((checkbox) => {
      expect(checkbox).toBeChecked();
    });
  });

  it("exports CSV with the class name in the filename", async () => {
    const user = userEvent.setup();
    const anchor = document.createElement("a");
    const clickSpy = vi.fn();
    anchor.click = clickSpy as unknown as HTMLAnchorElement["click"];

    const originalCreateElement = document.createElement.bind(document);
    const createElementSpy = vi
      .spyOn(document, "createElement")
      .mockImplementation((tagName: string) => {
        if (tagName === "a") {
          return anchor;
        }
        return originalCreateElement(tagName);
      });
    const createObjectURLSpy = vi
      .spyOn(URL, "createObjectURL")
      .mockReturnValue("blob:mock");
    const appendSpy = vi.spyOn(document.body, "appendChild");
    const removeSpy = vi.spyOn(document.body, "removeChild");

    render(<ClassAttendanceScreen />);

    const exportButton = await screen.findByRole("button", {
      name: /export csv/i,
    });

    await user.click(exportButton);

    expect(createObjectURLSpy).toHaveBeenCalled();
    expect(appendSpy).toHaveBeenCalledWith(anchor);
    expect(clickSpy).toHaveBeenCalled();
    expect(removeSpy).toHaveBeenCalledWith(anchor);
    expect(anchor.getAttribute("download")).toContain("Morning Yoga");

    createElementSpy.mockRestore();
    createObjectURLSpy.mockRestore();
    appendSpy.mockRestore();
    removeSpy.mockRestore();
  });

  it("navigates back when the back button is clicked", async () => {
    const user = userEvent.setup();
    render(<ClassAttendanceScreen />);

    await user.click(await screen.findByLabelText(/go back/i));
    expect(mockNavigate).toHaveBeenCalledWith(-1);
  });

  it("updates the visible week when week navigation buttons are used", async () => {
    const user = userEvent.setup();
    render(<ClassAttendanceScreen />);

    await screen.findAllByRole("columnheader");
    const getFirstHeader = () =>
      screen.getAllByRole("columnheader")[1].textContent;

    const initialHeader = getFirstHeader();
    await user.click(screen.getByLabelText(/next week view/i));
    const updatedHeader = getFirstHeader();
    expect(updatedHeader).not.toBe(initialHeader);

    await user.click(screen.getByLabelText(/previous week view/i));
    expect(getFirstHeader()).toBe(initialHeader);
  });
});
