export type Channel = {
	id: string;
	name: string;
	category: string | 'uncategorized';
	description: string | undefined;
	roles: string[];
};

export type CreateChannelRequest = Omit<Channel, 'id'>;
