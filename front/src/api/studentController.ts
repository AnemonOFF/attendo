import api from "./axios";

// GET /groups/{groupId}/students
export const getStudentsByGroup = (groupId: number) =>
  api.get(`/groups/${groupId}/students`);
