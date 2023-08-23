import React, { useEffect } from "react";
import Sidebar from "../components/sidebar/sidebar";
import {
  QueryClient,
  QueryClientProvider,
  useMutation,
  useQuery,
} from "@tanstack/react-query";
import HubContextProvider from "../providers/HubContextProvider";
import axios from "axios";
import { fetchAllChannels } from "../api/channels.api";

type RootLayoutProps = {
  children;
  queryClient: QueryClient;
};
function RootLayout({ children, queryClient }: RootLayoutProps) {
  const channelQuery = useQuery(["channels"], {
    queryFn: async () => await fetchAllChannels,
  });

  useEffect(() => {
    console.log(channelQuery);
  });
  return (
    <QueryClientProvider client={queryClient}>
      <HubContextProvider hubConnection={globalThis.hubConnection}>
        {children}
      </HubContextProvider>
    </QueryClientProvider>
  );
}

export default RootLayout;
