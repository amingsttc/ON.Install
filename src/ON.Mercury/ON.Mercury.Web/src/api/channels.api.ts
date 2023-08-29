import axios, { AxiosResponse } from 'axios';

import { devConfig } from '../config/config';
import { headers } from './headers';
import { Channel, CreateChannelRequest } from '../types/channel';
import { Message } from '../types/message';

export async function fetchAllChannels(): Promise<Channel[]> {
	const result: AxiosResponse = await axios.get(
		`${devConfig.apiPath}/channels`,
		{ headers }
	);
	const channels: Channel[] = result.data;
	return channels;
}

// TODO: Fix the server not accepting the request
export async function createChannel(
	request: CreateChannelRequest
): Promise<Channel> {
	const result: AxiosResponse = await axios.post(
		`${devConfig.apiPath}/channels`,
		request,
		{
			headers: headers,
		}
	);

	const channel: Channel = result.data;
	return channel;
}

export async function getMessages(channelId: string) {
	const result: AxiosResponse = await axios.get(
		`${devConfig.apiPath}/channels/${channelId}/messages`,
		{ headers }
	);
	const messages: Message[] = result.data;

	return messages;
}
