import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { RenderHookResult, render, renderHook } from "@testing-library/react";
import { ReactElement, ReactNode } from "react";

export const createTestQueryClient = () =>
  new QueryClient({
    defaultOptions: {
      queries: {
        retry: false,
        cacheTime: 0,
        gcTime: 0,
      },
      mutations: {
        retry: false,
      },
    },
  });

const buildWrapper =
  (client: QueryClient) =>
  ({ children }: { children: ReactNode }) =>
    <QueryClientProvider client={client}>{children}</QueryClientProvider>;

export const renderWithQueryClient = (ui: ReactElement) => {
  const queryClient = createTestQueryClient();
  return render(ui, {
    wrapper: buildWrapper(queryClient),
  });
};

export const renderHookWithQueryClient = <T,>(
  callback: () => T,
): RenderHookResult<T, unknown> => {
  const queryClient = createTestQueryClient();
  return renderHook(callback, {
    wrapper: buildWrapper(queryClient),
  });
};

