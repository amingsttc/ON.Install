import {
	Accessor,
	Setter,
	createContext,
	useContext,
	createSignal,
} from 'solid-js';
import { Channel } from '../types/channel';
import { Message } from '../types/message';
import { Role } from '../types/role';
import { CurrentMember, Member } from '../types/member';
import { HubConnection } from '@microsoft/signalr';

interface ContextProps {
	channels: Accessor<Channel[]>;
	setChannels: Setter<Channel[]>;
	messages: Accessor<Record<string, Message[]>>;
	setMessages: Setter<Record<string, Message[]>>;
	addMessage: (message: Message) => void;
	roles: Accessor<Role[]>;
	setRoles: Setter<Role[]>;
	members: Accessor<Member[]>;
	getMemberUsernameById: (memberId: string) => string;
	setMembers: Setter<Member[]>;
	hubConnection: Accessor<HubConnection>;
	setHubConnection: Setter<HubConnection>;
	currentMember: Accessor<CurrentMember>;
	setCurrentMember: Setter<CurrentMember>;
}

const GlobalContext = createContext<ContextProps>();

export function GlobalProvider(props: any) {
	const [channels, setChannels] = createSignal<Channel[]>([]);
	const [messages, setMessages] = createSignal<Record<string, Message[]>>({});
	const [roles, setRoles] = createSignal<Role[]>([]);
	const [members, setMembers] = createSignal<Member[]>([]);
	const [hubConnection, setHubConnection] = createSignal<HubConnection>(
		undefined!
	);
	const [currentMember, setCurrentMember] = createSignal<CurrentMember>(
		undefined!
	);

	const addMessage = (message: Message) => {
		let msgs = messages()[message.channelId];
		msgs.push(message);
		setMessages({ [message.channelId]: msgs });
	};

	// TODO: Get this server side as a full state variable with connection statuses
	const getMemberUsernameById = (memberId: string) => {
		const member = members().find(
			(member: Member) => member.id === memberId
		);
		return member ? member.username : 'Unknown Member';
	};

	return (
		<GlobalContext.Provider
			value={{
				channels,
				setChannels,
				messages,
				setMessages,
				addMessage,
				roles,
				setRoles,
				members,
				getMemberUsernameById,
				setMembers,
				hubConnection,
				setHubConnection,
				currentMember,
				setCurrentMember,
			}}>
			{props.children}
		</GlobalContext.Provider>
	);
}

export const useGlobalContext = () => useContext(GlobalContext)!;
