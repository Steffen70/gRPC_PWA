import { CardTitle, CardDescription, CardHeader, CardContent, CardFooter, Card } from "@/components/ui/card"
import { Label } from "@/components/ui/label"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"

export default function LoginForm() {
    return (
        <div className="flex items-center justify-center h-screen bg-gray-100">
            <Card className="w-full max-w-md mx-auto mt-10">
                <CardHeader className="text-center">
                    <CardTitle className="text-2xl font-bold">
                        <span className="text-primary">Swiss</span>
                        <span className="text-secondary-foreground">Pension 7</span>
                    </CardTitle>
                    <CardDescription>Please enter your username and password to login.</CardDescription>
                </CardHeader>
                <CardContent className="space-y-4">
                    <div className="space-y-2">
                        <Label htmlFor="username">Username</Label>
                        <Input id="username" placeholder="Username" required type="text" />
                    </div>
                    <div className="space-y-2">
                        <Label htmlFor="password">Password</Label>
                        <Input id="password" placeholder="Password" required type="password" />
                    </div>
                </CardContent>
                <CardFooter>
                    <Button className="w-full">Login</Button>
                </CardFooter>
            </Card>
        </div>
    )
}