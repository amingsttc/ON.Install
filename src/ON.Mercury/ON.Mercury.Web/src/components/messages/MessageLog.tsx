import { useParams } from "react-router-dom";
//import { createSelector } from '@reduxjs/toolkit';
import React, { useEffect, useRef, useState } from "react";
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
  const messageLogRef = useRef<HTMLDivElement>(null);
  const [isLockedToBottom, setIsLockedToBottom] = useState(true);
  const dispatch = useAppDispatch();
  let channelId = useParams().id;
  let messages: Message[] = useAppSelector((state) =>
    selectChannel(state, channelId as string),
  );
  const [showScrollButton, setShowScrollButton] = useState(false);
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

  const scrollToBottom = () => {
    if (messageLogRef.current && isLockedToBottom) {
      const messageLog = messageLogRef.current;
      messageLog.scrollTop = messageLog.scrollHeight;
    }
  };

  const handleScrollToBottomClick = () => {
    setIsLockedToBottom(true);
    scrollToBottom();
    setShowScrollButton(false); // Hide the button after clicking
  };

  const handleScroll = () => {
    const messageLog = messageLogRef.current;
    if (messageLog) {
      const isScrolledToBottom =
        messageLog.scrollHeight - messageLog.scrollTop <=
        messageLog.clientHeight + 10;
      setIsLockedToBottom(isScrolledToBottom);

      // Toggle the visibility of the scroll button
      setShowScrollButton(!isScrolledToBottom);
    }
  };

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
    } else {
      scrollToBottom();
    }
  }, [messages, messageQuery]);

  useEffect(() => {
    const messageLog = messageLogRef.current;
    if (messageLog) {
      messageLog.addEventListener("scroll", handleScroll);
    }
    return () => {
      if (messageLog) {
        messageLog.removeEventListener("scroll", handleScroll);
      }
    };
  }, []);

  if (!messageQuery.isSuccess) {
    return (
      <div>
        <h1>Loading</h1>
      </div>
    );
  }

  return (
    <>
      <div className="message-log" ref={messageLogRef}>
        {messages &&
          messages.map((message) => <MessageItem message={message} />)}
        {!isLockedToBottom && showScrollButton && (
          <div
            className="scroll-to-bottom-button"
            onClick={handleScrollToBottomClick}
          >
            <div className="arrow-down-icon">▼</div>
          </div>
        )}
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
