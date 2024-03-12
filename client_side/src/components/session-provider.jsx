import { createContext, useContext, useState, useEffect } from "react";

const SessionContext = createContext();

export const useSession = () => useContext(SessionContext);

export const SessionProvider = ({ children }) => {
    const [username, setUsername] = useState(null);
    const [password, setPassword] = useState(null);
    const [grpcClients, setGrpcClients] = useState({});
    const [isInProcess, setIsInProcess] = useState(false);
    const [isSessionEstablished, setIsSessionEstablished] = useState(false);

    useEffect(() => {
        async function login() {
            if (username && password) {
                setIsInProcess(true);

                // await 2s delay
                await new Promise(resolve => setTimeout(resolve, 6000));

                setIsInProcess(false);

                console.log(`Logging in as ${username}`);

                if (username !== 'admin' || password !== 'admin') {
                    setUsername(null);
                    setPassword(null);

                    return;
                }

                // Todo: login via authClient
                // Todo: Open a connection to the session persistance SignalR hub
                // Todo: Implement a login interceptor that adds the token to the metadata

                const clients = {
                    // Todo: Initialize the clients with interceptors
                };
                setGrpcClients(clients);

                setIsSessionEstablished(true);

            } else {
                setGrpcClients({});
                setIsSessionEstablished(false);
            }
        }

        login();
    }, [username, password]);

    return (
        <SessionContext.Provider value={{ username, setUsername, password, setPassword, grpcClients, isSessionEstablished, isInProcess }}>
            {children}
        </SessionContext.Provider>
    );
};