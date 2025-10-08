import api from './axios';

// GET /groups
export const getGroups = (offset?: number, limit?: number) =>
  api.get('/groups', { params: { offset, limit } });

// POST /groups
export const createGroup = (data: { title: string }) =>
  api.post('/groups', data);

// GET /groups/{id}
export const getGroupById = (id: number) =>
  api.get(`/groups/${id}`);

// PUT /groups/{id}
export const updateGroup = (id: number, data: { title?: string; students?: number[] }) =>
  api.put(`/groups/${id}`, data);

// DELETE /groups/{id}
export const deleteGroup = (id: number) =>
  api.delete(`/groups/${id}`);