import React from "react";
import "./ChannelList.css";
import { useAppSelector } from "../../app/hooks";
import { selectChannels } from "../../features/channels/channelsSlice";
import { Channel } from "../../types/channel";
import { Link } from "react-router-dom";

export function ChannelList() {
  const channels = useAppSelector(selectChannels);
  return (
    <div className="channel-list">
      {channels.map((channel: Channel) => (
        <Link to={`/channels/${channel.id}`}>
          <div className="list-item">
            <h1>
              {channel.name} {channel.category}
            </h1>
            <p>{channel.description}</p>
          </div>
        </Link>
      ))}
    </div>
  );
}
