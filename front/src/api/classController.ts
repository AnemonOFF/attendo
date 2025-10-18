import api from './axios';

// GET /classes
export const getClasses = (from: string, to: string) =>
  api.get('/classes', { params: { from, to } });

// POST /classes
export const createClass = (data: { start: string; end: string; groupId: number }) =>
  api.post('/classes', data);

// GET /classes/{id}
export const getClassById = (id: number) => api.get(`/classes/${id}`);

// PUT /classes/{id}
export const updateClass = (id: number, data: { start?: string; end?: string; groupId?: number }) =>
  api.put(`/classes/${id}`, data);

// DELETE /classes/{id}
export const deleteClass = (id: number) => api.delete(`/classes/${id}`);
