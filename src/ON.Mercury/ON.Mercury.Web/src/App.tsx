import React, { useEffect, useState } from "react";
import "./assets/App.css";
import { QueryClient } from "@tanstack/react-query";
import { buildSignalR } from "./signalR/signalR";
import { config } from "./config/config";
import { AppView } from "./views/AppView";
import LoadingView from "./views/LoadingView";
import SettingsView from "./views/SettingsView";

const queryClient = new QueryClient();
globalThis.token = config.authToken;
function App() {
  const [token, setToken] = useState(globalThis.token);
  const [isLoading, setIsLoading] = useState(false);
  const [showServerSettings, setShowServerSettings] = useState(false);

  useEffect(() => {
    if (token === undefined) {
      setIsLoading(true);
    } else {
      setIsLoading(false);
      if (globalThis.hubConnection === undefined) {
        globalThis.hubConnection = buildSignalR(
          `${config.mercuryApi}/hub`,
          token as string,
        );
      }
    }
  }, [token, setToken, globalThis.hubConnection]);

  return (
    (!isLoading && <AppView queryClient={queryClient} />) ||
    (isLoading && <LoadingView />) ||
    (showServerSettings && <SettingsView />)
  );
}

export default App;
