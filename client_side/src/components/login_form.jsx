import { useState, useRef, useEffect } from "react";
import { CardTitle, CardDescription, CardHeader, CardContent, CardFooter, Card } from "@/components/ui/card";
import { Label } from "@/components/ui/label";
import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button";
import { useSession } from "./session_provider";
import { Loader2 } from "lucide-react";

export default function LoginForm() {
    const { setUsername, setPassword, isSessionEstablished, isLoginInProgress } = useSession();
    const [inputUsername, setInputUsername] = useState("");
    const [inputPassword, setInputPassword] = useState("");
    const [attemptedLogin, setAttemptedLogin] = useState(false);

    const timeoutRef = useRef();
    useEffect(() => {
        if (isLoginInProgress) {
            setAttemptedLogin(true);
        }

        if (!isLoginInProgress && attemptedLogin && !isSessionEstablished) {

            setInputPassword("");

            document.getElementById("password").focus();

            if (timeoutRef.current)
                clearTimeout(timeoutRef.current);

            timeoutRef.current = setTimeout(() => {
                setAttemptedLogin(false);
            }, 3000);

        }
    }, [isLoginInProgress, attemptedLogin]);

    const onLogin = () => {
        setUsername(inputUsername);
        setPassword(inputPassword);
    };

    const cardDescription = !isLoginInProgress && attemptedLogin && !isSessionEstablished
        ? <CardDescription className="text-primary">Login failed. Please check your username and password.</CardDescription>
        : <CardDescription>Please enter your username and password to login.</CardDescription>;

    const handleKeyDown = (event) => {
        if (event.key === "Enter") {
            onLogin();
        }
    };

    return (
        <div className="flex items-center justify-center h-screen bg-muted">
            <Card className="w-full max-w-md mx-auto mt-10 rounded-lg" style={{ borderColor: "hsl(var(--border))" }}>
                <CardHeader className="text-center">
                    <CardTitle className="text-2xl font-bold">
                        <span className="text-primary">Swiss</span>
                        <span className="text-secondary-foreground">Pension 7</span>
                    </CardTitle>
                    {cardDescription}
                </CardHeader>
                <CardContent className="space-y-4">
                    <div className="space-y-2">
                        <Label htmlFor="username">Username</Label>
                        <Input id="username" value={inputUsername} onChange={(e) => setInputUsername(e.target.value)} placeholder="Username" required type="text" disabled={isLoginInProgress} />
                    </div>
                    <div className="space-y-2">
                        <Label htmlFor="password">Password</Label>
                        <Input id="password" value={inputPassword} onChange={(e) => setInputPassword(e.target.value)} placeholder="Password" required type="password" onKeyDown={handleKeyDown} disabled={isLoginInProgress} />
                    </div>
                </CardContent>
                <CardFooter>
                    <Button className="w-full" onClick={onLogin} disabled={isLoginInProgress}>
                        {isLoginInProgress ? <Loader2 className="mr-2 h-4 w-4 animate-spin" /> : ""}
                        Login
                    </Button>
                </CardFooter>
            </Card>
        </div>
    );
}