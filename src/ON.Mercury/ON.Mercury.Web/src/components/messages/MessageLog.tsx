import { useParams } from "react-router-dom";
//import { createSelector } from '@reduxjs/toolkit';
import React, { useEffect, useState } from "react";
import { HubConnection } from "@microsoft/signalr";
// import { useAppSelector } from '../../App/hooks';
// import { SendMessageDto } from '../../../lib/dto/message.dto';
// import { RootState } from '../../App/store';
// import { ProfileDto } from '../../../lib/dto/profile.dto';
import MessageItem from "./MessageItem";
import "@styles/MessageLog.css";
import { selectLoggedInUser } from "../../features/app/appSlice";
import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { useQuery } from "@tanstack/react-query";
import { Message } from "../../types/message";
import { fetchMessages } from "../../api/message.api";
import {
  MessageMapEntry,
  selectChannel,
  setMessages,
} from "../../features/messages/messagesSlice";

interface MessageLogProps {
  connection: HubConnection;
}

export default function MessageLog({ connection }: MessageLogProps) {
  const dispatch = useAppDispatch();
  let channelId = useParams().id;
  let messages: Message[] = useAppSelector((state) =>
    selectChannel(state, channelId as string),
  );

  const loggedInUser = useAppSelector(selectLoggedInUser);
  const [newMessage, setNewMessage] = useState("");
  const messageQuery = useQuery(["messages"], {
    queryFn: async () => {
      if (channelId) {
        messages = await fetchMessages(channelId);
        const id = channelId as string;

        const stateEntry: MessageMapEntry = {
          channel: id,
          messages: [...messages],
        };
        dispatch(setMessages(stateEntry));
      }

      return messages;
    },
    enabled: false,
  });

  const handleKeyPress = async (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && newMessage !== "") {
      await handleSubmit();
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewMessage(e.target.value);
  };

  const handleSubmit = async () => {
    const msg = {
      channelId: channelId as string,
      senderId: loggedInUser?.id,
      body: newMessage,
    };

    await connection.invoke("SendMessage", msg);

    setNewMessage("");
  };

  useEffect(() => {
    if (!messages) {
      messageQuery.refetch();
    }
  }, [messageQuery]);

  if (!messageQuery.isSuccess) {
    return (
      <div>
        <h1>Loading</h1>
      </div>
    );
  }

  return (
    <>
      <div className="message-log">
        {messages &&
          messages.map((message) => <MessageItem message={message} />)}
      </div>
      <div className="message-input-container">
        <input
          type="text"
          className="message-input"
          placeholder="Enter your text"
          value={newMessage}
          onChange={handleChange}
          onKeyPress={handleKeyPress}
        />
        <button className="message-submit" onClick={handleSubmit}>
          Submit
        </button>
      </div>
    </>
  );
}
