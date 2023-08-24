import { useQuery } from "@tanstack/react-query";
import React, { useEffect } from "react";
import MessageLog from "../components/messages/MessageLog";
import Sidebar from "../components/sidebar/sidebar";
import { HubConnection } from "@microsoft/signalr";
import { fetchAllChannels } from "../api/channels.api";
import { fetchAllRoles } from "../api/roles.api";
import { fetchAllMembers, fetchCurrentMember } from "../api/member.api";
import { useAppDispatch } from "../app/hooks";
import { setChannels } from "../features/channels/channelsSlice";
import {
  setRoles,
  setMembers,
  setLoggedInUser,
} from "../features/app/appSlice";

type AppViewProps = {
  hubConnection: HubConnection | undefined;
};

export function AppView({ hubConnection }: AppViewProps) {
  const dispatch = useAppDispatch();
  const queryChannels = useQuery(["channels"], {
    queryFn: async () => {
      var found = await fetchAllChannels();
      if (found) {
        dispatch(setChannels(found));
      }
      return found;
    },
    enabled: true,
    refetchOnWindowFocus: false,
    refetchOnReconnect: true,
  });

  const queryRoles = useQuery(["roles"], {
    queryFn: async () => {
      var found = await fetchAllRoles();
      if (found) {
        dispatch(setRoles(found));
      }

      return found;
    },
    enabled: true,
    refetchOnWindowFocus: false,
    refetchOnReconnect: true,
  });

  const queryMembers = useQuery(["members"], {
    queryFn: async () => {
      var found = await fetchAllMembers();
      if (found) {
        dispatch(setMembers(found));
      }

      return found;
    },
    enabled: true,
    refetchOnWindowFocus: false,
    refetchOnReconnect: true,
  });

  const queryLoggedInUser = useQuery(["app:loggedInUser"], {
    queryFn: async () => {
      var found = await fetchCurrentMember();
      if (found) {
        dispatch(setLoggedInUser(found));
      }

      return found;
    },
    enabled: false,
    refetchOnWindowFocus: false,
    refetchOnReconnect: true,
  });

  useEffect(() => {
    queryLoggedInUser.refetch();
  });

  return (
    <>
      <Sidebar />
      <MessageLog connection={globalThis.hubConnection} userId="123" />
    </>
  );
}
