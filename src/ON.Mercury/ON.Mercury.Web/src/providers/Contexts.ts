import { HubConnection } from "@microsoft/signalr";
import { createContext } from "react";
import { Channel } from "../types/channel";
import { Role } from "../types/roles";
import { Member } from "../types/member";

export const ChannelsContext = createContext<Channel[]>([]);
export const RolesContext = createContext<Role[]>([]);
export const MembersContext = createContext<Member[]>([]);
export const HubConnectionContext = createContext<HubConnection | undefined>(
  globalThis.hubConnection,
);
