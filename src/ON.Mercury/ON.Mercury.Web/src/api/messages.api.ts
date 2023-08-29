import axios, { AxiosResponse } from 'axios';
import { devConfig } from '../config/config';
import { headers } from './headers';
import { Message } from '../types/message';

export async function getMessages(channelId: string): Promise<Message[]> {
	const result: AxiosResponse = await axios.get(
		`${devConfig.apiPath}/channels/${channelId}/messages`,
		{
			headers,
		}
	);

	const messages = result.data;
	return messages;
}
