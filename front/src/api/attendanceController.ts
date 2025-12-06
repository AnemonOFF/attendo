import api from "./axios";

// PUT /classes/{id}/attendance
export const updateAttendance = (
  id: number,
  attendance: { date: string; students: number[] }[],
) => api.put(`/classes/${id}/attendance`, { attendance });

export const getAttendance = (id: number) =>
  api.get<{
    attendance: { date: string; students: number[] }[];
  }>(`/classes/${id}/attendance`);
