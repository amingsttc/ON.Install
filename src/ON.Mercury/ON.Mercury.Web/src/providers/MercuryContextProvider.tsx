import { HubConnection } from "@microsoft/signalr";
import React, { useState } from "react";
import {
  ChannelsContext,
  RolesContext,
  MembersContext,
  HubConnectionContext,
} from "./Contexts";
import { Channel } from "../types/channel";
import { Role } from "../types/roles";
import { Member } from "../types/member";

type MercuryProviderProps = {
  hubConnection: HubConnection | undefined;
  channels: Channel[];
  roles: Role[];
  members: Member[];
  children;
};

function MercuryProvider({
  hubConnection,
  channels,
  roles,
  members,
  children,
}: MercuryProviderProps) {
  //const [channels, setChannels] = useState<Channel[]>([]);
  //const [roles, setRoles] = useState([]);
  // const [members, setMembers] = useState([]);

  return (
    <HubConnectionContext.Provider value={hubConnection}>
      <ChannelsContext.Provider value={channels}>
        <RolesContext.Provider value={roles}>
          <MembersContext.Provider value={members}>
            {children}
          </MembersContext.Provider>
        </RolesContext.Provider>
      </ChannelsContext.Provider>
    </HubConnectionContext.Provider>
  );
}

export default MercuryProvider;
