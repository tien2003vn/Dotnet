import { StrictMode } from "react";
import { createRoot } from "react-dom/client";
import { BrowserRouter as Router } from "react-router-dom";
import App from "./App.jsx";
import GlobalStyles from "./components/GlobalStyles/index.jsx";
import Store from "./components/Redux/Store";
import { Provider } from "react-redux";

createRoot(document.getElementById("root")).render(
    <StrictMode>
        <Provider store={Store}>
            <Router future={{ v7_startTransition: true }}>
                <GlobalStyles>
                    <App />
                </GlobalStyles>
            </Router>
        </Provider>
    </StrictMode>
);
