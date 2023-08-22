import React, { useEffect, useState } from "react";
import "./assets/App.css";
import {
  QueryClient,
  useMutation,
  useQueries,
  useQuery,
} from "@tanstack/react-query";
import { buildSignalR } from "./signalR/signalR";
import { config } from "./config/config";
import { AppView } from "./views/AppView";
import LoadingView from "./views/LoadingView";
import SettingsView from "./views/SettingsView";
import Cookies from "js-cookie";
import { fetchAllChannels } from "./api/channels.api";
import axios from "axios";
import { authMutation } from "./layouts/_root";

const queryClient = new QueryClient();

globalThis.token = localStorage.getItem("jwt");

function App() {
  const [token, setToken] = useState(globalThis.token);
  const [isLoading, setIsLoading] = useState(false);
  const [showServerSettings, setShowServerSettings] = useState(false);

  useEffect(() => {
    if (token === undefined) {
      setIsLoading(true);
    } else {
      let c = authMutation.mutateAsync();
      console.log(c);
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
