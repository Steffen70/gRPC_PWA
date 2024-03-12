import { useEffect, useState } from "react";
import { HelloRequest } from "../generated/greeter_pb";

export default function HelloWorld() {
    const [message, setMessage] = useState("");
    const { grpcClients } = useSession();

    useEffect(() => {
        const client = grpcClients["greeterClient"];
            
        const request = new HelloRequest();
        request.setName("World");

        client.helloWorld(request, {}, (err, response) => {
            if (err) {
                console.error(err);
            } else {
                console.log(response.getMessage());

                setMessage(response.getMessage());
            }
        });
    }, []);

    return (
        <div>
            <h1>{message}</h1>
        </div>
    );
}