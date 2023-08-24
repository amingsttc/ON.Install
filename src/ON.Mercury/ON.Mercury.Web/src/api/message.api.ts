import { GetMessagesResponse, Message } from "@mercury/types/message";
import { config } from "../config/config";

export async function fetchMessages(channelId: string): Promise<Message[]> {
  const result = await fetch(
    `${config.mercuryApi}/channels/${channelId}/messages`,
    {
      headers: {
        Authorization: config.authToken,
      },
      mode: "cors",
      method: "get",
    },
  );

  const res: GetMessagesResponse = await result.json();
  return res.messages;
}

// export const useFetchMessagesQuery = async (channelId: string) => {
//   const messages = await fetchMessages(channelId);
// }
