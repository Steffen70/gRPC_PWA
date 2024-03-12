import { useSession } from "./session_provider";
import LoginForm from "./login_form";
import Dashboard from "./dashboard";
import { Loader2 } from "lucide-react";

export default function App() {
    const { isSessionEstablished, isTokenRenewalInProgress } = useSession();

    if (isTokenRenewalInProgress) {
        return (
            <div className="flex justify-center items-center h-screen text-primary">
                <Loader2 className="h-12 w-12 animate-spin"/>
            </div>
        );
    }

    return isSessionEstablished ? <Dashboard /> : <LoginForm />;
}