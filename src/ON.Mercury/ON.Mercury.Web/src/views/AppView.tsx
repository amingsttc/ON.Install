import { QueryClient } from "@tanstack/react-query";
import React from "react";
import MessageLog from "../components/messages/MessageLog";
import Sidebar from "../components/sidebar/sidebar";
import RootLayout from "../layouts/_root";
import MercuryProvider from "../providers/MercuryContextProvider";
import { HubConnection } from "@microsoft/signalr";

type AppViewProps = {
  hubConnection: HubConnection | undefined;
  queryClient: QueryClient;
};

export function AppView({ hubConnection, queryClient }: AppViewProps) {
  return (
    <MercuryProvider queryClient={queryClient} hubConnection={hubConnection}>
      <Sidebar />
      <MessageLog connection={globalThis.hubConnection} userId="123" />
    </MercuryProvider>
  );
}
