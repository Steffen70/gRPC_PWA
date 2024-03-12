import "./globals.css";
import { createRoot } from "react-dom/client";
import { SessionProvider } from "./components/session_provider";
import App from "./components/app";

// TODO: Add the base address during the build process
const baseAddress = "https://localhost:5001/";

createRoot(document.getElementById("root")).render(
    <SessionProvider baseAddress={baseAddress}>
        <App />
    </SessionProvider>,
);
