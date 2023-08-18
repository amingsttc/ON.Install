import React, { useEffect, useState } from "react";
import "./assets/App.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { buildSignalR } from "./signalR/signalR";
import HubContextProvider from "./components/providers/HubContextProvider";
import RootLayout from "./layouts/_root";
import { config } from "./config/config";

const queryClient = new QueryClient();

function App() {
  const [token, setToken] = useState(globalThis.token);

  useEffect(() => {
    if (token === undefined) {
    } else {
      if (globalThis.hubConnection === undefined) {
        globalThis.hubConnection = buildSignalR(
          `${config.mercuryApi}/hub`,
          token as string,
        );
      }
    }
  }, [token, setToken, globalThis.hubConnection]);

  return (
    <QueryClientProvider client={queryClient}>
      <HubContextProvider hubConnection={globalThis.hubConnection}>
        <RootLayout>
          <h1>Base View</h1>
        </RootLayout>
      </HubContextProvider>
    </QueryClientProvider>
  );
}

export default App;
