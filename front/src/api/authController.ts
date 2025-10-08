import api from './axios';

export const login = (login: string, password: string) =>
  api.post('/login', { login, password });

export const register = (login: string, password: string) =>
  api.post('/register', { login, password });

export const getMe = () => api.get('/me');