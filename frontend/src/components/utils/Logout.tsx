import { useLogoutMutation } from "@features/auth/api/authApi";
import { logout } from "@features/auth/store/authSlice";
import { SignOut } from "@phosphor-icons/react"
import { persistor } from "@store/store";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router";


export const Logout = () => {
    const dispatch = useDispatch();
    const [logoutApi] = useLogoutMutation();
    const navigate = useNavigate();

    const handleLogout = async () => {
        try {
            await logoutApi().unwrap();
        } catch (error) {
            console.error('Erro ao deslogar no servidor', error);
        } finally {
            dispatch(logout());
            await persistor.purge();
            navigate('/login');
        }
    };
    
    return (
        <>
        <button onClick={() => handleLogout()}>
            <SignOut size={32} />
        </button>
        </>
    )
}