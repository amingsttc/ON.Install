import { useAppSelector } from "../../app/hooks";
import { selectChannels } from "../../features/channels/channelsSlice";
import { Channel } from "../../types/channel";
import ChannelItem from "../channels/ChannelItem";
import "./Sidebar.css";
import React, { useState } from "react";
import { selectLoggedInUsername } from "../../features/app/appSlice";

function Sidebar() {
  const channels = useAppSelector(selectChannels);
  const username = useAppSelector(selectLoggedInUsername);
  const [showSidebarContextMenu, setShowSidebarContextMenu] = useState(false);
  const handleContextMenu = (e: React.MouseEvent<HTMLDivElement>) => {
    e.preventDefault();
    setShowSidebarContextMenu(true);
    const clickX = e.clientX;
    const clickY = e.clientY;
    const contextMenu = document.getElementById("sidebar-context-menu");

    if (contextMenu) {
      contextMenu.style.top = `${clickY}px`;
      contextMenu.style.left = `${clickX}px`;
    }
  };

  const handleLogout = (e: React.MouseEvent<HTMLButtonElement>) => {
    e.preventDefault();
  };

  return (
    <div
      className="sidebar"
      onContextMenu={handleContextMenu}
      onMouseLeave={() => setShowSidebarContextMenu(false)}
    >
      <div className="sidebar-header">
        <h3>ServerName</h3>
      </div>
      <div className="container">
        <ul className="list">
          {/* <Link
            to={`/channels/new`}
            className="sidebar-link"
            key={"new-channel"}
          > */}
          <div className="sidebar-channel">
            <span className="custom-bullet">+</span>
            <li className="channel-name">Add Channel</li>
          </div>
          {channels.map((channel: Channel) => (
            <ChannelItem key={channel.id} channel={channel} />
          ))}
        </ul>
      </div>
      <div className="sidebar-footer">
        {/* <h2>{username}</h2> */}
        <h2>{username}</h2>
        <button className="logout-button" onClick={handleLogout}>
          Logout
        </button>
      </div>
      {showSidebarContextMenu && (
        <div id="sidebar-context-menu" className="context-menu">
          <div
            className="context-menu-item"
            onClick={() => {
              setShowSidebarContextMenu(false);
              // navigate('/channels/new');
            }}
          >
            Create Channel
          </div>
        </div>
      )}
    </div>
  );
}

export default Sidebar;
