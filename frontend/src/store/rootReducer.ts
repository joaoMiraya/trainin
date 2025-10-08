import { combineReducers } from "@reduxjs/toolkit";
import { apiSlice } from "./api/apiSlice";
import authReducer from "@features/auth/store/authReducer";


const rootReducer = combineReducers({
  auth: authReducer,
  [apiSlice.reducerPath]: apiSlice.reducer,
});

export default rootReducer;

export type RootState = ReturnType<typeof rootReducer>;
