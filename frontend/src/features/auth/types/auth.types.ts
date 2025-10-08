export interface User {
    username: string;
    firstName: string;
    lastName: string;
    email: string;
};
  
export interface AuthState {
    user: User | null;
    isAuthenticated: boolean;
};
  
export interface RegisterRequest {
    username: string;
    firstName: string;
    lastName: string;
    phone: string;
    email: string;
    password: string;
};

export interface LoginRequest {
    email: string;
    password: string;
};

export interface GoogleAuthRequest {
    credential: string;
};

export interface AuthResponse {
    data: {
        user: User;
    }
    message: string;
    isSuccess: number;
};
