import React, { useEffect, useState } from "react";
import { GreeterClient } from "../generated/greeter_grpc_web_pb";
import { HelloRequest } from "../generated/greeter_pb";

export default function HelloWorld() {
    const [message, setMessage] = useState("");

    useEffect(() => {
        const client = new GreeterClient("https://localhost:5001");
        const request = new HelloRequest();
        request.setName("Hugo");

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
            <h1>Hello World and {message}</h1>
        </div>
    );
}