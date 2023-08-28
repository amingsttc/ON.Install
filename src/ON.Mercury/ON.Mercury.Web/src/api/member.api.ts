import { config } from "../config/config";
import { GetCurrentMemberResponse, Member } from "@mercury/types/member";

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

export async function fetchCurrentMember(): Promise<Member> {
  const result = await fetch(`${config.mercuryApi}/auth`, {
    headers: {
      Authorization: config.authToken,
    },
    mode: "cors",
    method: "get",
  });

  const res: GetCurrentMemberResponse = await result.json();
  return res.member;
}
