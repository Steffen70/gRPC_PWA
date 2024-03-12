import { useEffect, useState } from "react";
import { useSession } from "./session_provider";
import { Empty } from '../protobuf-javascript/google/protobuf/empty_pb.js';

export default function HelloWorld() {
    const [message, setMessage] = useState("");
    const { grpcClients } = useSession();

    useEffect(() => {
        const client = grpcClients["greeterClient"];

        client.helloWorld(new Empty(), {}, (err, response) => {
            if (err) {
                console.error(err);
            } else {
                console.log(response.getMessage());
                setMessage(response.getMessage());
            }
        });
    }, []);

    return (
        <h1>{message}</h1>
    );
}