import { useSession } from "./session_provider";
import LoginForm from "./login_form";
import Dashboard from "./dashboard";

export default function App() {
    const { isSessionEstablished } = useSession();

    return isSessionEstablished ? <Dashboard /> : <LoginForm />;
}