import React, { useState } from "react";
import "../../assets/styles/NewChannelForm.css";
import { useMutation } from "@tanstack/react-query";
import { createChannel } from "../../api/channels.api";
import { useModal } from "../../providers/ModalProvider";

type NewChannelRequest = {
  name: string;
  category: string;
  description: string;
};

export function NewChannelForm() {
  const { hideModal } = useModal();
  const [isPrivateChannel, setIsPrivateChannel] = useState(false);
  const [isPrivateString, setIsPrivateString] = useState("Public");
  const [newChannel, setNewChannel] = useState<NewChannelRequest>({
    name: "",
    category: "uncategorized",
    description: "none",
  });
  const createChannelMutation = useMutation(() => {
    return createChannel(newChannel);
  });

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createChannelMutation.mutateAsync();
  };

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setNewChannel((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  const onToggle = () => {
    setIsPrivateChannel(!isPrivateChannel);
    setIsPrivateString(isPrivateChannel ? "Public" : "Private");
  };
  return (
    <>
      <form className="new-channel-form" onSubmit={(e) => onSubmit(e)}>
        <label htmlFor="name" className="input-label">
          Channel Name
        </label>
        <input
          type="text"
          id="name"
          name="name"
          placeholder="channel-name"
          value={newChannel.name}
          onChange={(e) => onChange(e)}
          className="rounded-input"
        />
        <div className="toggle-header">
          <h1 className="toggle-heading">{isPrivateString}</h1>
          <div className="toggle-container">
            <input
              type="checkbox"
              id="toggleCategory"
              name="category"
              checked={isPrivateChannel}
              onChange={() => onToggle()}
              className="toggle-input"
            />
            <label htmlFor="toggleCategory" className="toggle-label">
              <span className="toggle-indicator" />
            </label>
          </div>
        </div>

        <div className="submit-button-container">
          <button type="button" onClick={hideModal}>
            Back
          </button>
          <button type="submit">Submit</button>
        </div>
      </form>
    </>
  );
}
