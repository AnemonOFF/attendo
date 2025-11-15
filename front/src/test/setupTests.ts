import "@testing-library/jest-dom/vitest";
import { cleanup } from "@testing-library/react";
import { afterEach, beforeAll, vi } from "vitest";

afterEach(() => {
  cleanup();
  vi.clearAllMocks();
});

beforeAll(() => {
  if (!window.URL.createObjectURL) {
    window.URL.createObjectURL = vi.fn(() => "blob:mock");
  }

  if (!window.URL.revokeObjectURL) {
    window.URL.revokeObjectURL = vi.fn();
  }
});
