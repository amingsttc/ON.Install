import { useQuery } from "@tanstack/react-query";
import { config } from "../config/config";

export async function fetchAllChannels() {
  const result = await fetch(`${config.mercuryApi}/channels`, {
    headers: {
      Authorization: config.authToken,
    },
  });
  console.log(result);
  return result;
}
