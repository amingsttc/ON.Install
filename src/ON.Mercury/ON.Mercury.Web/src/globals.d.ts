import { HubConnection } from "@microsoft/signalr";

declare global {
  var hubConnection: HubConnection | undefined;
  var token: string | undefined;
}

export {};
