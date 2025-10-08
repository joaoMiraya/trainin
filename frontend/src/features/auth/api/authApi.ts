import { ENDPOINTS } from '@constants/APIConstants';
import { apiSlice } from '@store/api/apiSlice';
import { AuthResponse, GoogleAuthRequest, LoginRequest, RegisterRequest } from '../types/auth.types';

export const authApi = apiSlice.injectEndpoints({
    endpoints: (builder) => ({
        register: builder.mutation<AuthResponse, RegisterRequest>({
            query: (credentials) => ({
                url: ENDPOINTS.AUTH.REGISTER,
                method: 'POST',
                body: credentials,
            })
        }),
        login: builder.mutation<AuthResponse, LoginRequest>({
            query: (credentials) => ({
                url: ENDPOINTS.AUTH.LOGIN,
                method: 'POST',
                body: credentials,
            })
        }),
        googleAuth: builder.mutation<AuthResponse, GoogleAuthRequest>({
            query: (credentials) => ({
                url: ENDPOINTS.AUTH.GOOGLE_AUTH,
                method: 'POST',
                body: credentials,
            })
        }),
        logout: builder.mutation<void, void>({
            query: (credentials) => ({
                url: ENDPOINTS.AUTH.LOGOUT,
                method: 'POST',
                body: credentials,
            })
        }),
    }),
    overrideExisting: false,
});

export const {
    useRegisterMutation,
    useLoginMutation,
    useGoogleAuthMutation,
    useLogoutMutation
} = authApi;
