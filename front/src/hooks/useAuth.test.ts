import { act, waitFor } from "@testing-library/react";
import { vi } from "vitest";

import { useLogin, useRegister, useMe } from "./useAuth";
import * as authApi from "../api/authController";
import { renderHookWithQueryClient } from "../test/testUtils";

vi.mock("../api/authController", () => ({
  login: vi.fn(),
  register: vi.fn(),
  getMe: vi.fn(),
}));

const mockedAuth = authApi as {
  login: ReturnType<typeof vi.fn>;
  register: ReturnType<typeof vi.fn>;
  getMe: ReturnType<typeof vi.fn>;
};

describe("useAuth hooks", () => {
  beforeEach(() => {
    vi.clearAllMocks();
  });

  it("calls auth.login with provided credentials", async () => {
    mockedAuth.login.mockResolvedValue({ data: { token: "123" } });
    const { result } = renderHookWithQueryClient(() => useLogin());

    await act(async () => {
      await result.current.mutateAsync({ login: "john", password: "secret" });
    });

    expect(mockedAuth.login).toHaveBeenCalledWith("john", "secret");
  });

  it("calls auth.register with name and credentials", async () => {
    mockedAuth.register.mockResolvedValue({ data: { id: "user-1" } });
    const { result } = renderHookWithQueryClient(() => useRegister());

    await act(async () => {
      await result.current.mutateAsync({
        name: "Jane",
        login: "jane@example.com",
        password: "password123",
      });
    });

    expect(mockedAuth.register).toHaveBeenCalledWith(
      "Jane",
      "jane@example.com",
      "password123",
    );
  });

  it("fetches current user with useMe", async () => {
    const mockUser = { id: "me", name: "Attendo User" };
    mockedAuth.getMe.mockResolvedValue(mockUser);

    const { result } = renderHookWithQueryClient(() => useMe());

    await waitFor(() => expect(result.current.data).toEqual(mockUser));
    expect(mockedAuth.getMe).toHaveBeenCalledTimes(1);
    expect(result.current.isSuccess).toBe(true);
  });
});
