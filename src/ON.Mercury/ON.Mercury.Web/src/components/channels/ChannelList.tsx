import React from "react";
import "@styles/ChannelList.css";
import { useAppSelector } from "../../app/hooks";
import { selectChannels } from "../../features/channels/channelsSlice";
import { CategoryListEntry, Channel } from "../../types/channel";
import { Link } from "react-router-dom";
import { useModal } from "../../providers/ModalProvider";
import { NewChannelForm } from "./CreateChannelForm";

const groupChannels = (channels: Channel[]) => {
  const groupedChannels: { [category: string]: Channel[] } = channels.reduce(
    (result, channel) => {
      const category = channel.category || "Uncategorized";

      if (!result[category]) {
        result[category] = [];
      }

      result[category].push(channel);
      return result;
    },
    {} as { [category: string]: Channel[] },
  );

  const categories = Object.keys(groupedChannels).map((category) => ({
    category,
    channels: groupedChannels[category],
  }));

  return categories;
};

export function ChannelList() {
  const categories = groupChannels(useAppSelector(selectChannels));
  const { showModal } = useModal();
  const openModal = () => {
    const content = <NewChannelForm />;
    showModal(content);
  };

  // TODO: Fix each element should have a unique key warning on <div className="list-item" key={category.category}>
  return (
    <div className="channel-list">
      <button className="list-item" onClick={openModal}>
        create channel
      </button>
      {categories.map((category: CategoryListEntry) => (
        <div className="list-item" key={category.category}>
          <h1>{category.category}</h1>
          {category.channels.map((channel: Channel) => (
            <Link to={`/channels/${channel.id}`}>
              <div className="list-item">
                <h1>{channel.name}</h1>
              </div>
            </Link>
          ))}
        </div>
      ))}
    </div>
  );
}
