import { CardTitle, CardDescription, CardHeader, CardContent, Card } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Button } from "@/components/ui/button"

export default function Component() {
  return (
    <div className="min-h-screen flex flex-col items-center justify-center bg-[#f5f5f6]">
      <Card className="w-[400px] bg-white p-8">
        <CardHeader className="mb-4">
          <CardTitle className="text-2xl font-bold text-center text-[#e42312]">SwissPension 7</CardTitle>
          <CardDescription className="text-sm text-gray-600 text-center">Login to your account</CardDescription>
        </CardHeader>
        <CardContent>
          <form className="flex flex-col space-y-4">
            <div className="flex flex-col space-y-1">
              <label className="text-sm font-medium text-gray-700" htmlFor="username">
                Username
              </label>
              <Input id="username" placeholder="Enter your username" />
            </div>
            <div className="flex flex-col space-y-1">
              <label className="text-sm font-medium text-gray-700" htmlFor="password">
                Password
              </label>
              <Input id="password" placeholder="Enter your password" type="password" />
            </div>
            <Button className="bg-[#e42312] text-white hover:bg-[#c81e10]">Log In</Button>
          </form>
        </CardContent>
      </Card>
    </div>
  )
}