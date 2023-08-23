import { config } from "../config/config";
import { Member } from "../types/member";

export async function fetchAllMembers(): Promise<Member[]> {
  const result = await fetch(`${config.mercuryApi}/auth/members`, {
    headers: {
      Authorization: config.authToken,
    },
    mode: "cors",
    method: "get",
  });

  return await result.json();
}
