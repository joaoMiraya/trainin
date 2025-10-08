export const API_BASE_URL = import.meta.env.VITE_API_URL;
export const PAYMENT_API_BASE_URL = import.meta.env.VITE_PAYMENT_API_URL;

export const ENDPOINTS = {
  AUTH: {
    GOOGLE_AUTH: "/oauth/google",
    LOGIN: "/auth/login",
    REGISTER: "/auth/register",
    REFRESH_TOKEN: "/auth/refresh",
    LOGOUT: "/auth/logout",
  },
  USERS: {
    GET_ALL: "/users",
    GET_BY_ID: (id: number) => `/users/${id}`,
    UPDATE: (id: number) => `/users/${id}`,
    DELETE: (id: number) => `/users/${id}`,
  },
  POSTS: {
    GET_ALL: "/posts",
    GET_BY_ID: (id: number) => `/posts/${id}`,
    CREATE: "/posts",
    UPDATE: (id: number) => `/posts/${id}`,
    DELETE: (id: number) => `/posts/${id}`,
  },
  PAYMENT: {
    PAYMENT_INTENT: "/transactions",
  },
};
