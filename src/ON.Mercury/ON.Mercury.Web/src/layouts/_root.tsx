import React from "react";
import Sidebar from "../components/sidebar/sidebar";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import HubContextProvider from "../providers/HubContextProvider";

type RootLayoutProps = {
  children;
  queryClient: QueryClient;
};

function RootLayout({ children, queryClient }: RootLayoutProps) {
  return (
    <QueryClientProvider client={queryClient}>
      <HubContextProvider hubConnection={globalThis.hubConnection}>
        {children}
      </HubContextProvider>
    </QueryClientProvider>
  );
}

export default RootLayout;
