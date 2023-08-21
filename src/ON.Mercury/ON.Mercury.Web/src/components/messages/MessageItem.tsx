import React, { useState } from "react";
// import { MessageDto } from "../../../lib/dto/message.dto";
import "./MessageItem.css";
// import { useDeleteMessageMutation } from "../../features/channelSlice";
// import { useAppSelector } from "../../App/hooks";

interface MessageItemProps {
  message: any;
  username: string;
}

// TODO Only show delete and edit if
//    - It's the current users message
//    - The user is an admin or owner
const MessageItem: React.FC<MessageItemProps> = ({ message, username }) => {
  const { body, sentOn } = message;
  const [showContextMenu, setShowContextMenu] = useState(false);
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
    setShowContextMenu(false);
  };

  return (
    <div
      className="message-item"
      onContextMenu={(e) => {
        e.preventDefault();
        setShowContextMenu(true);
        const clickX = e.clientX;
        const clickY = e.clientY;
        const contextMenu = document.getElementById("context-menu");

        if (contextMenu) {
          contextMenu.style.top = `${clickY}px`;
          contextMenu.style.left = `${clickX}px`;
        }
      }}
      onMouseLeave={() => setShowContextMenu(false)}
    >
      <div className="username-date-container">
        <div className="message-username">{username}</div>
        <div className="message-sent-on">{formatDate(sentOn)}</div>
      </div>
      <div className="message-body">{body}</div>

      {showContextMenu && (
        <div id="context-menu" className="context-menu">
          <div
            className="context-menu-item"
            onClick={() => {
              setShowContextMenu(false);
            }}
          >
            Edit Message
          </div>
          <div className="context-menu-item" onClick={handleDeleteMessage}>
            Delete Message
          </div>
        </div>
      )}
    </div>
  );
};

export default MessageItem;
