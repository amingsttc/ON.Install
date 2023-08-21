import { channel } from "diagnostics_channel";
import React, { useRef, useState } from "react";
import "./ChannelItem.css";

export default function ChannelItem() {
  const [showChannelContextMenu, setShowChannelContextMenu] = useState(false);
  // const deleteChannel = useDeleteChannelMutation();
  const contextMenuRef = useRef<HTMLDivElement>(null);
  const handleDeleteChannel = async () => {
    //await deleteChannel.mutateAsync(channel.channelId);
    setShowChannelContextMenu(false);
  };

  const handleContextMenu = (e: React.MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    e.stopPropagation();
    setShowChannelContextMenu(true);
    const clickX = e.clientX;
    const clickY = e.clientY;
    const contextMenu = document.getElementById("channel-context-menu");

    if (contextMenu) {
      contextMenu.style.top = `${clickY}px`;
      contextMenu.style.left = `${clickX}px`;
    }
  };

  const handleOutsideClick = (e: MouseEvent) => {
    if (
      contextMenuRef.current &&
      !contextMenuRef.current.contains(e.target as Node)
    ) {
      setShowChannelContextMenu(false);
    }
  };

  const handleContextMenuItemClick = (e: React.MouseEvent<HTMLDivElement>) => {
    // Prevent the click event from bubbling up to the parent (Sidebar) context menu
    e.stopPropagation();
  };

  // Add a global event listener to handle outside clicks and close the context menu
  React.useEffect(() => {
    document.addEventListener("click", handleOutsideClick);

    return () => {
      document.removeEventListener("click", handleOutsideClick);
    };
  }, []);

  return (
    // <Link
    // 	to={`/channels/${channel.channelId}`}
    // 	className="sidebar-link"
    // 	key={channel.channelId}>
    <div
      className="sidebar-channel"
      onContextMenu={handleContextMenu}
      onMouseLeave={() => setShowChannelContextMenu(false)}
    >
      <li className="channel-name">
        <span className="custom-bullet">#</span>
        {channel.name}
      </li>
      {showChannelContextMenu && (
        <div
          ref={contextMenuRef}
          id="channel-context-menu"
          className="context-menu"
        >
          <div
            className="context-menu-item"
            onClick={() => setShowChannelContextMenu(false)}
          >
            Edit Channel
          </div>
          <div className="context-menu-item" onClick={handleDeleteChannel}>
            Delete Channel
          </div>
        </div>
      )}
    </div>
    //</Link>
  );
}
