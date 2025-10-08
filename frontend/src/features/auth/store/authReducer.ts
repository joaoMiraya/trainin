import { persistReducer } from 'redux-persist';
import authSlice from './authSlice';
import storageSession from 'redux-persist/lib/storage/session';

const authPersistConfig = {
  key: 'auth',
  storage: storageSession,
  whitelist: ['user', 'isAuthenticated'],
};

const authReducer = persistReducer(authPersistConfig, authSlice);

export default authReducer;