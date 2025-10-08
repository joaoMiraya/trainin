import { Eye, EyeSlash } from "@phosphor-icons/react";
import { forwardRef, useState } from "react";
import { FieldError } from "react-hook-form";

interface InputProps extends React.InputHTMLAttributes<HTMLInputElement> {
  error?: FieldError;
  isPassword?: boolean;
}

export const Input = forwardRef<HTMLInputElement, InputProps>(
  ({ error, isPassword, ...props }, ref) => {
    const [showPassword, setShowPassword] = useState(false);

    const inputType = isPassword
      ? showPassword
        ? "text"
        : "password"
      : props.type;

    return (
      <div className="w-full relative min-w-[16rem]">
        <input
          ref={ref}
          {...props}
          className={`dark:bg-medium-dark dark:text-white bg-light-gray p-1 rounded-sm w-full ${
            error ? "border border-red-500" : ""
          } ${props.className || ""}`}
           type={inputType}
        />
        {isPassword && (
          <button
            type="button"
            onClick={() => setShowPassword((prev) => !prev)}
            className="absolute right-2 top-1/2 transform -translate-y-1/2"
          >
            {showPassword ? <EyeSlash color="gray" size={16} /> : <Eye color="gray" size={16} />}
          </button>
        )}
        {error && (
          <p className="text-red-500 text-sm mt-1">{error.message}</p>
        )}
      </div>
    );
  }
);

Input.displayName = "Input";