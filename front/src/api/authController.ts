import api from "./axios";

export const login = async (login: string, password: string) => {
  const response = await api.post("/login", { login, password });
  return response.data.token; // Return only the token
};

export const register = async (
  name: string,
  login: string,
  password: string,
) => {
  const response = await api.post("/register", { name, login, password });
  return response.data.token;
};

export const getMe = () => api.get("/me");
