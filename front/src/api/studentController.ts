import api from "./axios";

export const createStudent = (data: { fullName: string }) =>
  api.post("/students", data);
