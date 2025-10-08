import { Link } from "react-router"
import { LogoType } from "./Logotype"
import { Logout } from "@components/utils/Logout"

export const Header = () => {


    return (

       <header className="p-4 border-b-[1px] border-gray-300">
        <div className="flex items-center justify-center ">
          <Link to={"/"}>
             <LogoType />
          </Link>
        </div>
        <Logout />
       </header>
    )
}
