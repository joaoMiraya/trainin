import { useAppSelector } from "@store/hooks";
import { Navigate } from "react-router";

type Props = {
  children: JSX.Element;
};

export const ProtectedRoute = ({ children }: Props) => {
    const auth = useAppSelector(state => state.auth)

  if (!auth.isAuthenticated) {
    return <Navigate to="/login" replace />;
  }

  return children;
};