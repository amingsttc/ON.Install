import React, { useEffect, useState } from "react";
import "./assets/App.css";
import { buildSignalR } from "./signalR/signalR";
import { config } from "./config/config";
import { AppView } from "./views/AppView";
import { createBrowserRouter, RouterProvider } from "react-router-dom";

globalThis.token = localStorage.getItem("jwt");

const router = createBrowserRouter([
  {
    path: "/",
    element: <AppView hubConnection={globalThis.hubConnection} />,
  },
]);

function App() {
  const [token, setToken] = useState(globalThis.token);
  const [isLoading, setIsLoading] = useState(false);
  const [showServerSettings, setShowServerSettings] = useState(false);

  useEffect(() => {
    if (token === undefined) {
      setIsLoading(true);
    } else {
      if (globalThis.hubConnection === undefined) {
        globalThis.hubConnection = buildSignalR(
          `${config.mercuryApi}/hub`,
          token as string,
        );

        globalThis.hubConnection.start();
      }
    }
  }, [token, setToken, globalThis.hubConnection]);

  return <RouterProvider router={router} />;
}

export default App;
