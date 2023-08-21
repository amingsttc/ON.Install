import React, { createContext, useContext } from "react";
import { HubConnection } from "@microsoft/signalr";
import { useQuery } from "@tanstack/react-query";
import { fetchAllChannels } from "../api/channels.api";

type HubContextProviderProps = {
  hubConnection: HubConnection | undefined;
  children;
};

const HubConnectionContext = createContext<HubConnection | undefined>(
  globalThis.hubConnection,
);

function HubContextProvider({
  hubConnection,
  children,
}: HubContextProviderProps) {
  return (
    <HubConnectionContext.Provider value={hubConnection}>
      {children}
    </HubConnectionContext.Provider>
  );
}

export default HubContextProvider;
