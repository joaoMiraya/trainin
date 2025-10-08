import '@stylesheets/reset.css';
import '@stylesheets/index.css';
import { Outlet } from "react-router"
import { Footer } from "./components/partials/Footer"
import { Container } from "@components/patterns/Container";

function App() {  

  return (
    <>
      <Container>
        <Outlet />
      </Container>
      <Footer />
    </>
  )
}

export default App
