import { HubConnection } from "@microsoft/signalr";
import { createContext } from "react";

export const ChannelsContext = createContext<any[]>([]);
export const RolesContext = createContext<any[]>([]);
export const MembersContext = createContext<any[]>([]);
export const HubConnectionContext = createContext<HubConnection | undefined>(
  globalThis.hubConnection,
);
