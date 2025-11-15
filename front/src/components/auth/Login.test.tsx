import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import { vi } from "vitest";
import { MemoryRouter } from "react-router-dom";

import Login from "./Login";

vi.mock("../../assets/logo.png", () => ({ default: "logo.png" }));

const mockNavigate = vi.fn();
const mockUseLogin = vi.fn();

vi.mock("react-router-dom", async () => {
  const actual = await vi.importActual<typeof import("react-router-dom")>(
    "react-router-dom",
  );
  return {
    ...actual,
    useNavigate: () => mockNavigate,
  };
});

vi.mock("../../hooks/useAuth", () => ({
  useLogin: () => mockUseLogin(),
}));

describe("Login component", () => {
  beforeEach(() => {
    mockNavigate.mockReset();
    mockUseLogin.mockReset();
    mockUseLogin.mockReturnValue({
      mutateAsync: vi.fn(),
      isPending: false,
    });
  });

  const renderLogin = () =>
    render(
      <MemoryRouter>
        <Login />
      </MemoryRouter>,
    );

  it("shows validation errors when submitting empty form", async () => {
    const user = userEvent.setup();
    renderLogin();

    await user.click(
      screen.getByRole("button", {
        name: /sign in/i,
      }),
    );

    expect(screen.getByText("Email is required")).toBeInTheDocument();
    expect(screen.getByText("Password is required")).toBeInTheDocument();
  });

  it("navigates to calendar when form is valid", async () => {
    const user = userEvent.setup();
    renderLogin();

    await user.type(screen.getByLabelText(/email address/i), "user@test.com");
    await user.type(screen.getByLabelText(/password/i), "123456");

    await user.click(
      screen.getByRole("button", {
        name: /sign in/i,
      }),
    );

    expect(mockNavigate).toHaveBeenCalledWith("/calendar");
  });

  it("disables submit button when login mutation is pending", () => {
    mockUseLogin.mockReturnValue({
      mutateAsync: vi.fn(),
      isPending: true,
    });

    renderLogin();

    const button = screen.getByRole("button", { name: /signing in/i });
    expect(button).toBeDisabled();
  });
});

