import { axiosBaseQuery } from '@customApi/axiosBaseQuery';
import { createApi } from '@reduxjs/toolkit/query/react';

export const apiSlice = createApi({
  reducerPath: 'api',
  baseQuery: axiosBaseQuery(),
  endpoints: () => ({}),
});
