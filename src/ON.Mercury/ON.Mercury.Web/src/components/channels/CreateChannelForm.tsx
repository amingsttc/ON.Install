import React, { useState } from "react";
import "@styles/NewChannelForm.css";
import { useMutation } from "@tanstack/react-query";
import { createChannel } from "../../api/channels.api";

type NewChannelRequest = {
  name: string;
  category: string;
  description: string | undefined;
};

export function NewChannelForm() {
  const [newChannel, setNewChannel] = useState<NewChannelRequest>({
    name: "",
    category: "Uncategorized",
    description: "",
  });
  const createChannelMutation = useMutation((newChannel) => {
    return createChannel(newChannel as unknown as NewChannelRequest);
  });

  const onSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    await createChannelMutation.mutateAsync();
    //await createChannel.mutateAsync(newChannel);
  };

  const onChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setNewChannel((prevState) => ({
      ...prevState,
      [name]: value,
    }));
  };

  return (
    <>
      <form className="new-channel-form" onSubmit={onSubmit}>
        <label htmlFor="name">Name:</label>
        <input
          type="text"
          id="name"
          name="name"
          value={newChannel.name}
          onChange={onChange}
        />
        <label htmlFor="category">Category:</label>
        <input
          type="text"
          id="category"
          name="category"
          value={newChannel.category || ""}
          onChange={onChange}
        />
        <label htmlFor="description">Description:</label>
        <input
          type="text"
          id="description"
          name="description"
          value={newChannel.description || ""}
          onChange={onChange}
        />
        <div>
          <button type="submit">Submit</button>
        </div>
      </form>
    </>
  );
}
