import { useAppDispatch, useAppSelector } from "../../app/hooks";
import { selectChannels } from "../../features/channels/channelsSlice";
import { Channel } from "../../types/channel";
import ChannelItem from "../channels/ChannelItem";
import "@styles/Sidebar.css";
import React, { useState } from "react";
import {
  selectLoggedInUsername,
  setLoggedInUser,
} from "../../features/app/appSlice";
import { redirect, useHref, useNavigate } from "react-router";
import { Link } from "react-router-dom";
import { HubConnection } from "@microsoft/signalr";
import CustomBulletItem from "../lists/CustomBulletItem";

function Sidebar() {
  const routerHref = useHref;
  const navigate = useNavigate();
  const dispatch = useAppDispatch();
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
    // dispatch(setLoggedInUser(undefined));
    // let connection: HubConnection = globalThis.hubConnection;
    // connection.stop();
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
          <Link to={`/`} className="sidebar-link">
            <CustomBulletItem bullet="#">Directory</CustomBulletItem>
          </Link>
          {channels.map((channel: Channel) => (
            <ChannelItem key={channel.id} channel={channel} />
          ))}
        </ul>
      </div>
      <div className="sidebar-footer">
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
              navigate("/channels/new");
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
