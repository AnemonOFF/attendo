import { useQuery } from "@tanstack/react-query";
import * as classApi from "../api/classController";

export const useClasses = (from: string, to: string) =>
  useQuery({
    queryKey: ["classes", from, to],
    queryFn: () => classApi.getClasses(from, to).then(res => res.data.items),
  });
