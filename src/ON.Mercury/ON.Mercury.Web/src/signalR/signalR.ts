import {
  HubConnection,
  HubConnectionBuilder,
  JsonHubProtocol,
} from "@microsoft/signalr";

export function buildSignalR(url: string, token: string): HubConnection {
  var connection: HubConnection = new HubConnectionBuilder()
    .withUrl(url, {
      headers: {
        Authorization: token,
      },
      withCredentials: false,
    })
    .withHubProtocol(new JsonHubProtocol())
    .withAutomaticReconnect()
    .build();

  connection.onclose(() => {
    globalThis.hubConnection = undefined;
  });

  connection.start();

  return connection;
}
