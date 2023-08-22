import React, { useEffect, useState } from "react";
import "./assets/App.css";
import { QueryClient, useQueries, useQuery } from "@tanstack/react-query";
import { buildSignalR } from "./signalR/signalR";
import { config } from "./config/config";
import { AppView } from "./views/AppView";
import LoadingView from "./views/LoadingView";
import SettingsView from "./views/SettingsView";
import Cookies from "js-cookie";
import { fetchAllChannels } from "./api/channels.api";

const queryClient = new QueryClient();

function App() {
  const [token, setToken] = useState(Cookies.get("token"));
  const [isLoading, setIsLoading] = useState(false);
  const [showServerSettings, setShowServerSettings] = useState(false);

  useEffect(() => {
    Cookies.set("token", config.authToken, { domain: "http://localhost:5173" });
    if (token === undefined) {
      setIsLoading(true);
      console.log(Cookies.get());
    } else {
      setIsLoading(false);
      if (globalThis.hubConnection === undefined) {
        globalThis.hubConnection = buildSignalR(
          `${config.mercuryApi}/hub`,
          token as string,
        );

        globalThis.hubConnection.start();
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
