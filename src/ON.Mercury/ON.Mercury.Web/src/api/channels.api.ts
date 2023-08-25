import { config } from "../config/config";
import { Channel } from "@mercury/types/channel";

type NewChannelRequest = {
  name: string;
  category: string;
  description: string | undefined;
};

export async function fetchAllChannels() {
  const result = await fetch(`${config.mercuryApi}/channels`, {
    headers: {
      Authorization: config.authToken,
    },
    mode: "cors",
    method: "get",
  });

  const channels = (await result.json()) as Channel[];

  return channels;
}

export async function createChannel(newChannel: NewChannelRequest) {
  const result = await fetch(`${config.mercuryApi}/channels`, {
    headers: {
      Authorization: config.authToken,
      ContentType: "application/json",
    },
    mode: "cors",
    method: "post",
    body: JSON.stringify(newChannel),
  });
  console.log(result);
  return result.body;
}
