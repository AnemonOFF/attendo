import api from "./axios";

export const login = async (login: string, password: string) => {
  const response = await api.post("/login", { login, password });
  return response.data.accessToken; // Return only the token
};

export const register = async (
  login: string,
  email: string,
  password: string,
) => {
  await api.post("/register", { email, login, password });
};

export const getMe = () => api.get("/me");
