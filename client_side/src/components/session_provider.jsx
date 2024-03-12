import React, { createContext, useContext, useState, useEffect } from "react";
import { createAuthenticatedClient } from '../utils/authenticated_client_factory';
import { AuthClient } from "../generated/auth_grpc_web_pb";
import { LoginRequest } from "../generated/auth_pb";
import { HubConnectionBuilder } from "@microsoft/signalr";

import { GreeterClient } from "../generated/greeter_grpc_web_pb";


const SessionContext = createContext();

export const useSession = () => useContext(SessionContext);

export const SessionProvider = ({ children, baseAddress }) => {
    const [username, setUsername] = useState(null);
    const [password, setPassword] = useState(null);
    const [grpcClients, setGrpcClients] = useState({});
    const [isInProcess, setIsInProcess] = useState(false);
    const [isSessionEstablished, setIsSessionEstablished] = useState(false);

    async function establishSession(token) {
        // FIXME: Create a connection-token to establish a SignalR connection, don"t use the token directly
        const messageHubUrl = `${baseAddress}hubs/session-hub?access_token=${token}`;

        const hubConnection = new HubConnectionBuilder()
            .withUrl(messageHubUrl)
            .withAutomaticReconnect()
            .build();

        // TODO: Set up event handlers for the hubConnection

        await hubConnection.start();

        // TODO: Evaluate if it is safe to store the token in local storage - I don't think it is
        localStorage.setItem("token", token);

        const clients = {
            authClient: createAuthenticatedClient(AuthClient, baseAddress, token),
            greeterClient: createAuthenticatedClient(GreeterClient, baseAddress, token),
        };

        setGrpcClients(clients);
        setIsSessionEstablished(true);
    }

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

                const authHeader = response.getToken();
                const token = authHeader.substring("Bearer ".length);

                await establishSession(token);
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
        async function renewToken() {
            const token = localStorage.getItem("token");

            if (!token)
                return;

            const authClient = createAuthenticatedClient(AuthClient, baseAddress, token);

            try {
                await new Promise((resolve, reject) => {
                    authClient.renewToken({}, (err, response) => {
                        if (err) reject(err);
                        else resolve(response);
                    });
                });

                await establishSession(token);
            } catch (error) {
                console.error("Token renewal failed:", error);
                
                localStorage.removeItem("token");
            }
        }

        renewToken();
    }, []);

    return (
        <SessionContext.Provider value={{ username, setUsername, password, setPassword, grpcClients, isSessionEstablished, isInProcess }}>
            {children}
        </SessionContext.Provider>
    );
};
