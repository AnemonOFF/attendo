import { useMutation, useQuery } from '@tanstack/react-query';
import * as auth from '../api/authController';

export const useLogin = () =>
  useMutation({
    mutationFn: ({ login, password }: { login: string; password: string }) =>
      auth.login(login, password),
  });

export const useRegister = () =>
  useMutation({
    mutationFn: ({ login, password }: { login: string; password: string }) =>
      auth.register(login, password),
  });

export const useMe = () => 
  useQuery({
    queryKey: ['me'],
    queryFn: auth.getMe,
  });