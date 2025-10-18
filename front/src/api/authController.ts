import api from './axios';

export const login = (login: string, password: string) => api.post('/login', { login, password });

export const register = (name: string, login: string, password: string) =>
	api.post('/register', { name, login, password });

export const getMe = () => api.get('/me');
