import { MagnifyingGlass } from "@phosphor-icons/react"
import { useState } from "react";

export const SearchInput = () => {
    const [search, setSearch] = useState<string>("");
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSearch(event.target.value);
    };

    return (
        <>
            <div className="flex items-center bg-medium-dark p-1 rounded-sm">
                <input type="text" className="text-white"
                  onChange={handleChange}
                  value={search}
                />
                <button className="px-1">
                    <MagnifyingGlass size={24} color="white"/>
                </button>
            </div>
        </>
    )
}