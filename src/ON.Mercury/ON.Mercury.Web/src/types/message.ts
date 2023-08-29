import { Timestamp } from 'google-protobuf/google/protobuf/timestamp_pb';

export type Message = {
	id: string;
	senderId: string;
	channelId: string;
	body: string;
	sentOn: Timestamp;
	modifiedOn: Timestamp;
	deletedOn: Timestamp | null;
};

export type SendMessageRequest = {
	senderId: string;
	channelId: string;
	body: string;
};

export type ReceiveMessage = {
	id: string;
	senderId: string;
	channelId: string;
	body: string;
	sentOn: Timestamp;
	modifiedOn: Timestamp;
	deletedOn: Timestamp | null;
};
