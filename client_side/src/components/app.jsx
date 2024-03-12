import { useSession } from "./session-provider";
import LoginForm from "./login-form";
import Dashboard from "./dashboard";

export default function App() {
    const { isSessionEstablished } = useSession();

    return isSessionEstablished ? <Dashboard /> : <LoginForm />;
}