import "./globals.css";
import { createRoot } from "react-dom/client";
import { SessionProvider } from './components/session-provider';
import App from "./components/app";

createRoot(document.getElementById("root")).render(
    <SessionProvider>
        <App />
    </SessionProvider>,
);
