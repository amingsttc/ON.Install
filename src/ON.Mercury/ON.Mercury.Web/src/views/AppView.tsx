import { QueryClient } from "@tanstack/react-query";
import React from "react";
import MessageLog from "../components/messages/MessageLog";
import Sidebar from "../components/sidebar/sidebar";
import RootLayout from "../layouts/_root";

type AppViewProps = {
  queryClient: QueryClient;
};

export function AppView({ queryClient }: AppViewProps) {
  return (
    <RootLayout queryClient={queryClient}>
      <Sidebar />
      <MessageLog connection={globalThis.hubConnection} userId="123" />
    </RootLayout>
  );
}
