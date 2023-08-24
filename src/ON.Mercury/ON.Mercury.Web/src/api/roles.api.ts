import { config } from "../config/config";
import { Role } from "@mercury/types/roles";

export async function fetchAllRoles(): Promise<Role[]> {
  const result = await fetch(`${config.mercuryApi}/roles`, {
    headers: {
      Authorization: config.authToken,
    },
    mode: "cors",
    method: "get",
  });

  return await result.json();
}
