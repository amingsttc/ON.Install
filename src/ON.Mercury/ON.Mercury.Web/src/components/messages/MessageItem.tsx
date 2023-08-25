import React, { useState } from "react";
// import { MessageDto } from "../../../lib/dto/message.dto";
import "@styles/MessageItem.css";
import { Message } from "../../types/message";
import { useAppSelector } from "../../app/hooks";
import { selectUsernameById } from "../../features/app/appSlice";
import { useContextMenu } from "../../providers/ContextMenuProvider";
// import { useDeleteMessageMutation } from "../../features/channelSlice";
// import { useAppSelector } from "../../App/hooks";

interface MessageItemProps {
  message: Message;
}

// TODO Only show delete and edit if
//    - It's the current users message
//    - The user is an admin or owner
const MessageItem: React.FC<MessageItemProps> = ({ message }) => {
  const { body, sentOn } = message;
  //const [showContextMenu, setShowContextMenu] = useState(false);
  const { showContextMenu, setShowContextMenu } = useContextMenu();
  // const deleteMessage = useDeleteMessageMutation();
  // TODO: Format to local time as well
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);

    // Helper function to add leading zeros to single-digit values
    const addLeadingZero = (value: number) => {
      return value < 10 ? `0${value}` : value.toString();
    };

    const formattedDate = `${addLeadingZero(
      date.getMonth() + 1,
    )}/${addLeadingZero(
      date.getDate(),
    )}/${date.getFullYear()} at ${addLeadingZero(
      date.getHours(),
    )}:${addLeadingZero(date.getMinutes())}`;
    return formattedDate;
  };

  const handleDeleteMessage = async () => {
    //await deleteMessage.mutateAsync(message.messageId);
    //setShowContextMenu(false);
  };

  return (
    <div
      className="message-item"
      onContextMenu={(e) => {
        e.preventDefault();
        const clickX = e.clientX;
        const clickY = e.clientY;

        // Calculate the position of the context menu based on mouse coordinates
        const contextMenu = document.getElementById("context-menu");
        if (contextMenu) {
          contextMenu.style.top = `${clickY}px`;
          contextMenu.style.left = `${clickX}px`;
        }

        setShowContextMenu(true);
      }}
      onMouseLeave={() => setShowContextMenu(false)}
    >
      <div className="username-date-container">
        <div className="message-username">
          {useAppSelector(
            (state) =>
              selectUsernameById(state, message.senderId as string)?.username,
          )}
        </div>
        <div className="message-sent-on">{formatDate(sentOn)}</div>
      </div>
      <div className="message-body">{body}</div>
    </div>
  );
};

export default MessageItem;
