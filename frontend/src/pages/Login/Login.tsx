import { Input } from "@components/utils/Input";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from 'zod';
import { useLoginMutation } from "@features/auth/api/authApi";
/* import { GoogleAuth } from "./GoogleAuth"; */
import { Link, useNavigate } from "react-router";
import { useTranslation } from "react-i18next";
import { useDispatch } from "react-redux";
import { loginAction } from "@features/auth/store/authSlice";
import { LogoType } from "@components/partials/Logotype";

const schema = z.object({
    email: z.string()
        .min(1, "E-mail é obrigatório"),
    password: z.string()
        .min(1, "Senha é obrigatório")
        .max(32, "Senha deve ter no máximo 32 caracteres")
});

type FormData = z.infer<typeof schema>;

export const Login = () => {
    const [login] = useLoginMutation();
    const { t: tAuth } = useTranslation('auth');
    const { t: tCommon } = useTranslation('common');
    const dispatch = useDispatch();
    const navigate = useNavigate();

    const {
        register,
        handleSubmit,
        formState: { errors, isSubmitting },
        setError,
    } = useForm<FormData>({
        resolver: zodResolver(schema),
        defaultValues: {
            email: '',
            password: ''
        }
    });

    const handleLogin = async (data: FormData) => {        
        try {
            const response = await login({
                email: data.email.trim(),
                password: data.password
            }).unwrap();
            dispatch(loginAction({
                user: response.data.user,
            }));
            navigate('/home');
        } catch (error: any) {
            if (error?.status === 401) {
                setError('root', {
                    type: 'server',
                    message: tAuth('error.invalid')
                });
            } 
            if (error?.status === 500) {
                setError('root', {
                    type: 'server',
                    message: tCommon('error.internalServerError')
                });
            }
            console.error(error);
        }
    };
    
    return (
        <div className='flex h-screen w-full justify-center lg:justify-around items-center pt-12 gap-6'>
            <div className='flex flex-col justify-around px-6 border-[1px] border-gray-500 rounded-sm h-[20rem]'>
                <div className="pb-3 self-center">
                    <LogoType />
                </div>            
                <div className="flex flex-col items-center w-full max-w-[18rem]">
                    {errors.root && (
                        <div className="text-red-500 mb-2">
                            {errors.root.message}
                        </div>
                    )}
                    
                    <form 
                        className="flex flex-col gap-2 w-full" 
                        onSubmit={handleSubmit(handleLogin)}
                    >
                        <Input
                            {...register("email")}
                            type="text"
                            placeholder="Seu email o username"
                            error={errors.email}
                            required
                        />
                        <Input 
                            {...register("password")}
                            type="password"
                            placeholder="Sua senha"
                            error={errors.password}
                            isPassword={true}
                            required
                        />
                        {errors.root?.message}
                        <button 
                            className="bg-primary py-1 px-4 w-full rounded-md text-white" 
                            type="submit"
                            disabled={isSubmitting}
                        >
                            {isSubmitting ? 'Entrando...' : 'Entrar'}
                        </button>
                    </form>
                    
                    <div className="flex items-center w-full gap-2 py-2">
                        <span className="w-full h-[1px] bg-dark"></span>
                        <p>Ou</p>
                        <span className="w-full h-[1px] bg-dark"></span>
                    </div>
                    <div className="flex items-center w-full gap-2">
                        <p>Não tem conta ainda? <Link to={"/register"}><span className="text-primary underline">Crie a sua conta</span></Link></p>
                    </div>
                    {/* <GoogleAuth/> */}
                </div>
            </div>
        </div>

    )
}