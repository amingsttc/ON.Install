import {
  HubConnection,
  HubConnectionBuilder,
  JsonHubProtocol,
} from "@microsoft/signalr";
import { store } from "../app/store";
import { addMessage } from "../features/messages/messagesSlice";
import { Message } from "@mercury/types/message";

export function buildSignalR(url: string, token: string): HubConnection {
  var connection: HubConnection = new HubConnectionBuilder()
    .withUrl(url, {
      accessTokenFactory: () => token,
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

  connection.on("ReceiveMessage", (message: Message) => {
    console.log(message);
    store.dispatch(
      addMessage({ channel: message.channelId, message: message }),
    );
  });

  //connection.start();

  return connection;
}
