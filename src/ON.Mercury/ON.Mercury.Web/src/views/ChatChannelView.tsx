import React, { useEffect } from "react";
import { useAppDispatch } from "../app/hooks";
import MessageLog from "../components/messages/MessageLog";
import Sidebar from "../components/sidebar/sidebar";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router-dom";
import { fetchMessages } from "../api/message.api";
import {
  MessageMapEntry,
  setMessages,
} from "../features/messages/messagesSlice";
import { Message } from "postcss";

export function ChatChannelView() {
  const dispatch = useAppDispatch();

  const { channelId } = useParams<{ channelId: string }>();
  // const messageQuery = useQuery(["messages"], {
  //   queryFn: async () => {
  //     let messages: Message[] = [];
  //     if (channelId) {
  //       messages = await fetchMessages(channelId);
  //       const id = channelId as string;

  //       const stateEntry: MessageMapEntry = {
  //         channel: id,
  //         messages,
  //       };
  //       dispatch(setMessages(stateEntry));
  //     }

  //     console.log(messages);

  //     return messages;
  //   },
  //   enabled: false,
  // });

  // if (messageQuery.isFetching || messageQuery.isRefetching) {
  //   return (
  //     <>
  //       <Sidebar />
  //       <h1>hi</h1>
  //     </>
  //   );
  // }

  return (
    <>
      <Sidebar />
      <MessageLog connection={globalThis.hubConnection} />
    </>
  );
}
