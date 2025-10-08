import { GoogleLogin, GoogleOAuthProvider } from '@react-oauth/google';

type CredentialResponse = {
    credential?: string;
    clientId?: string;
    select_by?: string;
};
export const GoogleAuth = () => {
    // REVISAR ---------------------------------
    const handleLoginSuccess = (credentialResponse: CredentialResponse) => {
        const credential = credentialResponse.credential;
        fetch('https://localhost:5001/api/auth/google', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ token: credential })
        }).then(response => response.json())
            .then(data => {
            // salvar JWT do backend, por exemplo
                localStorage.setItem('token', data.token);
            });
    };

    return (
        <>
        <GoogleOAuthProvider clientId={import.meta.env.VITE_OAUTH_CLIENT_ID}>
            <GoogleLogin
                onSuccess={handleLoginSuccess}
                onError={() => {
                    console.log('Login Failed');
                }}
                theme="filled_black"
            />
        </GoogleOAuthProvider>
        </>
    )
}