import { config } from "../config/config";
import { Channel } from "@mercury/types/channel";

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
