import { configureStore } from '@reduxjs/toolkit';
import rootReducer from './rootReducer';
import { apiSlice } from './api/apiSlice';
import {
  persistStore,
  FLUSH,
  REHYDRATE,
  PAUSE,
  PERSIST,
  PURGE,
  REGISTER,
} from 'redux-persist';

export const store = configureStore({
  reducer: rootReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
      },
    }).concat(apiSlice.middleware),
  devTools: import.meta.env.NODE_ENV !== 'production',
});

export const persistor = persistStore(store);

// Tipos globais do store
export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;