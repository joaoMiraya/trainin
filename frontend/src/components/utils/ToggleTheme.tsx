import { Moon, SunDim } from "@phosphor-icons/react";
import { useEffect, useState } from "react";

export const ToggleTheme = () => {
    const [theme, setTheme] = useState<'light' | 'dark'>('light');

    useEffect(() => {
        const savedTheme = localStorage.getItem('theme') as 'light' | 'dark';
        if (savedTheme) {
            setTheme(savedTheme);
            document.documentElement.classList.toggle('dark', savedTheme === 'dark');
        }
    }, []);

    const toggleTheme = () => {
        const newTheme = theme === 'light' ? 'dark' : 'light';
        setTheme(newTheme);
        document.documentElement.classList.toggle('dark', newTheme === 'dark');
        localStorage.setItem('theme', newTheme);
    };

    return (
        <button onClick={toggleTheme} className="p-2">
            {theme === 'light' ? <SunDim size={32} /> : <Moon size={32} />}
        </button>
    );
}