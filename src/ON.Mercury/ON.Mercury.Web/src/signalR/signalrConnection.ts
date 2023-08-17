import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
  JsonHubProtocol,
} from "@microsoft/signalr";

export const signalrConnection: HubConnection = new HubConnectionBuilder()
  .withUrl("")
  .configureLogging(LogLevel.Information)
  .withAutomaticReconnect()
  .withHubProtocol(new JsonHubProtocol())
  .build();
