import { createSlice, PayloadAction } from '@reduxjs/toolkit';
import { AuthState, User } from '../types/auth.types';


const user = localStorage.getItem('user');

const initialState: AuthState = {
  user: user ? JSON.parse(user) : null,
  isAuthenticated: !!user,
};

const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    loginAction(state, action: PayloadAction<{ user: User }>) {
        state.user = action.payload.user;
        state.isAuthenticated = true;
    },
    logout(state) {
      state.isAuthenticated = false;
      state.user = null;
    },
  },
});

export const { loginAction, logout } = authSlice.actions;
export default authSlice.reducer;
