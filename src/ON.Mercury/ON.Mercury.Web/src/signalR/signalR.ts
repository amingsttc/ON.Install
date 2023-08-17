import {
  HubConnection,
  HubConnectionBuilder,
  JsonHubProtocol,
} from "@microsoft/signalr";

export function buildSignalR(url: string, token: string): HubConnection {
  var connection: HubConnection = new HubConnectionBuilder()
    .withUrl(url, {
      accessTokenFactory: () => token,
    })
    .withHubProtocol(new JsonHubProtocol())
    .withAutomaticReconnect()
    .build();

  connection.start();

  connection.onclose(() => {
    globalThis.hubConnection = undefined;
  });

  return connection;
}
