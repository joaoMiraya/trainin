import ReactDOM from "react-dom/client";
import { BrowserRouter, Route, Routes } from 'react-router';
import App from '../App.tsx';
import { Home } from "@pages/Home/Home.tsx";
import { Provider } from "react-redux";
import { Login } from "@pages/Login/Login.tsx";
import { Register } from "@pages/Register/Register.tsx";
import { NotFound } from "@pages/partials/NotFound.tsx";
import { PersistGate } from "redux-persist/integration/react";
import { persistor, store } from "@store/store.ts";
import "@i18n/i18n.ts";
import { Suspense } from "react";
import { ProtectedRoute } from "./protectedRoute.tsx";

const root = document.getElementById("root");

if (root) {
  ReactDOM.createRoot(root).render(
    <BrowserRouter>
      <Provider store={store}>
        <PersistGate loading={null} persistor={persistor}>
          <Suspense fallback={<div>Carregando...</div>}>
            <Routes>
              <Route path="*" element={<NotFound/>} />
              <Route path="/" element={<App />}>
                <Route path="/login" element={<Login/>} />
                <Route path="/register" element={<Register/>} />
                <Route
                  path="/home"
                  element={
                    <ProtectedRoute>
                      <Home />
                    </ProtectedRoute>
                  }
                />
              </Route>
            </Routes>
          </Suspense>
        </PersistGate>
      </Provider>
    </BrowserRouter>
  );
} else {
  console.error("Root element not found.");
}
