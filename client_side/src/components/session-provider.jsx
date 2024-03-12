import React, { createContext, useContext, useState, useEffect } from "react";
import { AuthClient } from "../generated/auth_grpc_web_pb";
import { LoginRequest } from "../generated/auth_pb";
import { HubConnectionBuilder } from "@microsoft/signalr";

import { GreeterClient } from "../generated/greeter_grpc_web_pb";

function createAuthenticatedClient(ClientConstructor, baseAddress, token) {
    const authInterceptor = (options, nextCall) => {
        options.metadata = options.metadata || {};
        options.metadata["Authorization"] = `Bearer ${token}`;
        return nextCall(options);
    };

    const client = new ClientConstructor(baseAddress, null, {
        unaryInterceptors: [authInterceptor],
        streamInterceptors: [authInterceptor],
    });

    return client;
}


const SessionContext = createContext();

export const useSession = () => useContext(SessionContext);

export const SessionProvider = ({ children, baseAddress }) => {
    const [username, setUsername] = useState(null);
    const [password, setPassword] = useState(null);
    const [grpcClients, setGrpcClients] = useState({});
    const [isInProcess, setIsInProcess] = useState(false);
    const [isSessionEstablished, setIsSessionEstablished] = useState(false);

    async function login() {
        if (username && password) {
            setIsInProcess(true);

            const authClient = new AuthClient(baseAddress);
            const request = new LoginRequest();
            request.setUsername(username);
            request.setPassword(password);

            try {
                const response = await new Promise((resolve, reject) => {
                    authClient.login(request, {}, (err, response) => {
                        if (err) reject(err);
                        else resolve(response);
                    });
                });

                const token = response.getToken();

                // FIXME: Create a connection-token to establish a SignalR connection, don"t use the token directly
                const messageHubUrl = `${baseAddress}hubs/session-hub?access_token=${token}`;
                const hubConnection = new HubConnectionBuilder()
                    .withUrl(messageHubUrl)
                    .withAutomaticReconnect()
                    .build();

                // TODO: Set up event handlers for the hubConnection

                await hubConnection.start();

                const clients = {
                    authClient: createAuthenticatedClient(AuthClient, baseAddress, token),
                    greeterClient: createAuthenticatedClient(GreeterClient, baseAddress, token),
                };

                setGrpcClients(clients);
                setIsSessionEstablished(true);
            } catch (error) {
                console.error("Login failed:", error);

                setUsername(null);
                setPassword(null);
            } finally {
                setIsInProcess(false);
            }
        } else {
            setGrpcClients({});
            setIsSessionEstablished(false);
        }
    }


    useEffect(() => {
        login();
    }, [username, password]);

    useEffect(() => {
        // TODO: Check if the token is stored in local storage and use it to re-establish the session
    }, []);

    return (
        <SessionContext.Provider value={{ username, setUsername, password, setPassword, grpcClients, isSessionEstablished, isInProcess }}>
            {children}
        </SessionContext.Provider>
    );
};
