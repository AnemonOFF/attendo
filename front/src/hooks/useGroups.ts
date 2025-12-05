import { useQuery } from "@tanstack/react-query";

import * as groupApi from "../api/groupController";

export const useGroups = (offset?: number, limit?: number) =>
  useQuery({
    queryKey: ["groups", offset, limit],
    queryFn: () =>
      groupApi.getGroups(offset, limit).then((res) => res.data.items),
  });
