import { config } from "../config/config";
import { Channel } from "@mercury/types/channel";

type NewChannelRequest = {
  name: string;
  category: string;
  description: string;
};

export async function fetchAllChannels() {
  const result = await fetch(`${config.mercuryApi}/channels`, {
    headers: {
      Authorization: config.authToken,
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    mode: "cors",
    method: "get",
  });

  const channels = (await result.json()) as Channel[];

  return channels;
}

export async function createChannel(newChannel: NewChannelRequest) {
  try {
    const result = await fetch(`${config.mercuryApi}/channels`, {
      headers: {
        Authorization: config.authToken,
        "Content-Type": "application/json",
        Accept: "application/json",
      },
      mode: "cors",
      method: "post",
      body: JSON.stringify(newChannel),
    });

    if (!result.ok) {
      throw new Error(`Failed to create channel. Status: ${result.status}`);
    }

    const responseData = await result.json();
    return responseData;
  } catch (error) {
    console.error("Error creating channel:", error);
    throw error; // Rethrow the error for further handling if needed
  }
}
