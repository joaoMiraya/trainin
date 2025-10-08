import { ReactNode } from "react";

interface ContainerProps {
  children: ReactNode;
}

export const Container = ({ children }: ContainerProps) => {
  return (
    <div className="dark:bg-medium-dark bg-neutro-200 dark:text-white  min-h-dvh">
      {children}
    </div>
  )
};
