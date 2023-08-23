import { QueryClient, useQuery } from "@tanstack/react-query";
import React, { useContext, useEffect, useState } from "react";
import MessageLog from "../components/messages/MessageLog";
import Sidebar from "../components/sidebar/sidebar";
import RootLayout from "../layouts/_root";
import MercuryProvider from "../providers/MercuryContextProvider";
import { HubConnection } from "@microsoft/signalr";
import {
  ChannelsContext,
  MembersContext,
  RolesContext,
} from "../providers/Contexts";
import { fetchAllChannels } from "../api/channels.api";
import { fetchAllRoles } from "../api/roles.api";
import { fetchAllMembers } from "../api/member.api";

type AppViewProps = {
  hubConnection: HubConnection | undefined;
};

export function AppView({ hubConnection }: AppViewProps) {
  const channelsContext = useContext(ChannelsContext);
  const rolesContext = useContext(RolesContext);
  const membersContext = useContext(MembersContext);
  const queryChannels = useQuery(["channels"], {
    queryFn: async () => {
      var found = await fetchAllChannels();
      if (found) {
        channelsContext.push(...found);
      }
      return found;
    },
    enabled: true,
  });

  const queryRoles = useQuery(["roles"], {
    queryFn: async () => {
      var found = await fetchAllRoles();
      if (found) {
        rolesContext.push(...found);
      }

      return found;
    },
    enabled: true,
  });

  const queryMembers = useQuery(["members"], {
    queryFn: async () => {
      var found = await fetchAllMembers();
      if (found) {
        membersContext.push(...found);
      }

      return found;
    },
    enabled: true,
  });
  return (
    <MercuryProvider
      hubConnection={hubConnection}
      channels={channelsContext}
      roles={rolesContext}
      members={membersContext}
    >
      <Sidebar />
      <MessageLog connection={globalThis.hubConnection} userId="123" />
    </MercuryProvider>
  );
}
