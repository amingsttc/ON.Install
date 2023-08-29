import { HubConnectionBuilder, JsonHubProtocol } from '@microsoft/signalr';
import { devConfig } from '../config/config';

export async function buildConnection() {
	const url = `${devConfig.apiPath}/hub`;
	const connection = new HubConnectionBuilder()
		.withUrl(url, {
			accessTokenFactory: () => devConfig.token,
		})
		.withAutomaticReconnect()
		.withHubProtocol(new JsonHubProtocol())
		.build();

	await connection.start();

	return connection;
}
