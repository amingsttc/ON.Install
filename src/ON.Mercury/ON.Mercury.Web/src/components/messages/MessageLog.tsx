import { useParams } from "react-router-dom";
//import { createSelector } from '@reduxjs/toolkit';
import React, { useState } from "react";
import { HubConnection } from "@microsoft/signalr";
// import { useAppSelector } from '../../App/hooks';
// import { SendMessageDto } from '../../../lib/dto/message.dto';
// import { RootState } from '../../App/store';
// import { ProfileDto } from '../../../lib/dto/profile.dto';
import MessageItem from "./MessageItem";
import "./MessageLog.css";
import { selectLoggedInUser } from "../../features/app/appSlice";
import { useAppSelector } from "../../app/hooks";

interface MessageLogProps {
  connection: HubConnection;
}

const getUsernameBySenderId = (profiles: any[], senderId: string) => {
  const profile = profiles.find((profile) => profile.id === senderId);
  return profile ? profile.username : null;
};

// export const selectMessagesByChannel = createSelector(
// 	[
// 		(state: RootState) => state.channels.channels,
// 		(state: RootState) => state.profiles.profiles, // Include the profiles in the selector
// 		(channelId: string) => channelId,
// 	],
// 	(channels, profiles, channelId) => {
// 		const channel = channels.find(
// 			(channel) => channel.channelId === channelId
// 		);
// 		if (channel) {
// 			const messagesWithUsername = channel.chatMessages?.map(
// 				(message) => ({
// 					...message,
// 					username: getUsernameBySenderId(profiles, message.senderId),
// 				})
// 			);

// 			return messagesWithUsername || [];
// 		}
// 		return [];
// 	}
// );

export default function MessageLog({ connection }: MessageLogProps) {
  const channelId = useParams().id;
  const loggedInUser = useAppSelector(selectLoggedInUser);
  // const messages = useAppSelector((state) =>
  // 	selectMessagesByChannel.resultFunc(
  // 		state.channels.channels,
  // 		state.profiles.profiles,
  // 		channelId as string
  // 	)
  // );
  const messages = [];
  const [newMessage, setNewMessage] = useState("");

  const handleKeyPress = async (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter" && newMessage !== "") {
      await handleSubmit();
    }
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setNewMessage(e.target.value);
  };

  const handleSubmit = async () => {
    // const msg: SendMessageDto = {
    const msg = {
      channelId: channelId as string,
      senderId: loggedInUser?.id,
      body: newMessage,
    };

    await connection.invoke("SendMessage", JSON.stringify(msg));

    setNewMessage("");
  };

  return (
    <>
      <div className="message-log">
        {messages.map((message) => (
          // <MessageItem
          //   key={message.messageId}
          //   username={message.username as string}
          //   message={message}
          // />
          <h1>a</h1>
        ))}
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
