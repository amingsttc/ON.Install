import React from "react";
import Sidebar from "../components/sidebar/sidebar";
import {
  QueryClient,
  QueryClientProvider,
  useMutation,
} from "@tanstack/react-query";
import HubContextProvider from "../providers/HubContextProvider";
import axios from "axios";

type RootLayoutProps = {
  children;
  queryClient: QueryClient;
};
export const authMutation = useMutation(
  async () => {
    return await axios.post(
      "http://localhost/api/mercury/auth",
      {},
      {
        headers: {
          Authorization: globalThis.token,
        },
      },
    );
  },
  {
    mutationKey: ["authenticate"],
  },
);
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
