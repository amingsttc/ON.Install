import React from "react";
import { useAppDispatch } from "../app/hooks";
import MessageLog from "../components/messages/MessageLog";
import Sidebar from "../components/sidebar/sidebar";

export function ChatChannelView() {
  const dispatch = useAppDispatch();

  return (
    <>
      <Sidebar />
      <MessageLog connection={globalThis.hubConnection} />
    </>
  );
}
