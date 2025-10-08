import { Input } from "@components/utils/Input";
import { useRegisterMutation } from "@features/auth/api/authApi";
import { loginAction } from "@features/auth/store/authSlice";
import { zodResolver } from "@hookform/resolvers/zod";
import { phoneFormater } from "@scripts/main.script";
import { useForm } from "react-hook-form";
import { useTranslation } from "react-i18next";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router";
import { z } from "zod";

const schema = z.object({
    username: z.string()
        .min(1, "Username é obrigatório")
        .max(32, "Username deve ter no máximo 32 caracteres"),
    firstName: z.string()
        .min(1, "Nome é obrigatório")
        .max(16, "Nome deve ter no máximo 16 caracteres"),
    lastName: z.string()
        .min(1, "Nome é obrigatório")
        .max(16, "Nome deve ter no máximo 16 caracteres"),
    phone: z.string()
        .min(1, "Telefone é obrigatório")
        .max(16, "Telefone deve ter no máximo 16 caracteres"),
    email: z.string()
        .min(1, "E-mail é obrigatório")
        .email("E-mail inválido"),
    confirmEmail: z.string()
        .min(1, "Confirmação de e-mail é obrigatório")
        .email("E-mail inválido"),
    password: z.string()
        .min(8, "Senha é obrigatório")
        .max(32, "Senha deve ter no máximo 32 caracteres")
        .refine((val) => /(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,32}/.test(val), {
            message: "Senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial",
        }),
    confirmPassword: z.string()
        .min(8, "Confirmação de senha é obrigatório")
        .max(32, "Confirmação de senha deve ter no máximo 32 caracteres")
}).refine((data) => data.email === data.confirmEmail, {
    message: "Os e-mails não coincidem",
    path: ["confirmEmail"],
}).refine((data) => data.password === data.confirmPassword, {
    message: "As senhas não coincidem",
    path: ["confirmPassword"],
});

type FormData = z.infer<typeof schema>;

export const Register = () => {
    const [registerUser] = useRegisterMutation();
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
            username: '',
            firstName: '',
            lastName: '',
            phone: '',
            email: '',
            password: '',
            confirmEmail: '',
            confirmPassword: ''
        }
    });

    const handleRegister = async (data: FormData) => {      
        try {
            const response = await registerUser({
                username : data.username.trim(),
                firstName : data.firstName.trim(),
                lastName : data.lastName.trim(),
                phone : data.phone.trim(),
                email: data.email.trim(),
                password: data.password
            }).unwrap();
            dispatch(loginAction({
                user: response.data.user,
            }));
            navigate('/home');
        } catch (error: any) {
            if (error?.data?.status === 401) {
                setError('root', {
                    type: 'server',
                    message: tAuth('errors.invalid')
                });
            } else if (error?.data?.status === 500) {
                setError('root', {
                    type: 'server',
                    message: tCommon('errors.internalServerError')
                });

            }
            console.error(error)
        }
    };
    
    return (
        <div className="flex flex-col items-center w-full pt-24">
            <div className="flex flex-col items-center border-[1px] border-gray-400 rounded-sm p-6 ">
                <div className="pb-3 flex flex-col items-center">
                    <h1 className="text-3xl text-white playwrite-au-sa-logo">TrainIN</h1>
                    <h3>{tAuth('register.texts.registerText')}</h3>
                </div>  
                <form 
                    className="flex flex-col gap-2 w-full" 
                    onSubmit={handleSubmit(handleRegister)}
                >
                    <Input 
                        {...register("username")}
                        type="text"
                        placeholder="Seu username"
                        error={errors.username}
                        required
                    />
                    <Input 
                        {...register("firstName")}
                        type="text"
                        placeholder="Seu primeiro nome"
                        error={errors.firstName}
                        required
                    />
                    <Input 
                        {...register("lastName")}
                        type="text"
                        placeholder="Seu sobrenome"
                        error={errors.lastName}
                        required
                    />
                    <Input 
                          {...register("phone", {
                            onChange: (e) => {
                            e.target.value = phoneFormater(e.target.value);
                            }
                        })}
                        type="text"
                        placeholder="Seu telefone"
                        error={errors.phone}
                        required
                    />
                    <Input
                        {...register("email")}
                        type="email"
                        placeholder="Seu email"
                        error={errors.email}
                        required
                    />
                    <Input
                        {...register("confirmEmail")}
                        type="email"
                        placeholder="Confirme seu email"
                        error={errors.confirmEmail}
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
                    <Input 
                        {...register("confirmPassword")}
                        type="password"
                        placeholder="Confirme sua senha"
                        error={errors.confirmPassword}
                        isPassword={true}
                        required
                    />
                    <button 
                        className="bg-primary py-1 px-4 w-full rounded-md text-white" 
                        type="submit"
                        disabled={isSubmitting}
                    >
                        {isSubmitting ? 'Entrando...' : 'Entrar'}
                    </button>
                </form>
            </div>
        </div>
    )
}