import "./globals.css";
import { createRoot } from "react-dom/client";
import { SessionProvider } from "./components/session_provider";
import App from "./components/app";

// This gets replaced by the webpack DefinePlugin with the actual value of the environment variable during build
// You can find the build script in package.json
const baseAddress = process.env.SERVICE_BASE_ADDRESS;

createRoot(document.getElementById("root")).render(
    <SessionProvider baseAddress={baseAddress}>
        <App />
    </SessionProvider>
);
