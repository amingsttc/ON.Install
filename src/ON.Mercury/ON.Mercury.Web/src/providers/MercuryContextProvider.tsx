import { HubConnection } from "@microsoft/signalr";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import React, { useState } from "react";
import {
  ChannelsContext,
  RolesContext,
  MembersContext,
  HubConnectionContext,
} from "./Contexts";

type MercuryProviderProps = {
  hubConnection: HubConnection | undefined;
  queryClient: QueryClient;
  children;
};

function MercuryProvider({
  hubConnection,
  queryClient,
  children,
}: MercuryProviderProps) {
  const [channels, setChannels] = useState([]);
  const [roles, setRoles] = useState([]);
  const [members, setMembers] = useState([]);

  return (
    <QueryClientProvider client={queryClient}>
      <HubConnectionContext.Provider value={hubConnection}>
        <ChannelsContext.Provider value={channels}>
          <RolesContext.Provider value={roles}>
            <MembersContext.Provider value={members}>
              {children}
            </MembersContext.Provider>
          </RolesContext.Provider>
        </ChannelsContext.Provider>
      </HubConnectionContext.Provider>
    </QueryClientProvider>
  );
}

export default MercuryProvider;
