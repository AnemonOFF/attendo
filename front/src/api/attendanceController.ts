import api from "./axios";

// PUT /classes/{id}/attendance
export const updateAttendance = (id: number, attendance: number[]) =>
  api.put(`/classes/${id}/attendance`, { attendance });
