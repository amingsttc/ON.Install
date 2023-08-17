import React, { useEffect, useState } from "react";
import "./App.css";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { buildSignalR } from "./signalR/signalR";
import HubContextProvider from "./components/providers/HubContextProvider";

const queryClient = new QueryClient();

function App() {
  const [token, setToken] = useState(globalThis.token);

  useEffect(() => {
    console.log(globalThis.hubConnection);
    if (token === undefined) {
      console.log(false);
    } else {
      if (globalThis.hubConnection === undefined) {
        globalThis.hubConnection = buildSignalR(
          "http://localhost:8015/api/v1/hub",
          token as string,
        );
      }
    }
  }, [token, setToken, globalThis.hubConnection]);

  return (
    <QueryClientProvider client={queryClient}>
      <HubContextProvider hubConnection={globalThis.hubConnection}>
        <h1>Base View</h1>
      </HubContextProvider>
    </QueryClientProvider>
  );
}

export default App;
