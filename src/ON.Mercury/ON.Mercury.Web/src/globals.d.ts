import { HubConnection } from "@microsoft/signalr";

declare global {
  const hubConnection: HubConnection | undefined;
  const token: string | undefined;
}

export {};
