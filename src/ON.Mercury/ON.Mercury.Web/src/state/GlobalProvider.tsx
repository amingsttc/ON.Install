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
import { NotificationLog } from './notifications';

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
	activeChannel: Accessor<string>;
	setActiveChannel: Setter<string>;
	notifications: Accessor<NotificationLog>;
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
	const [activeChannel, setActiveChannel] = createSignal<string>('');
	const [notifications, setNotifications] = createSignal<NotificationLog>([]);

	const addMessage = (message: Message) => {
		let msgs = messages()[message.channelId];
		msgs.push(message);
		setMessages({ [message.channelId]: msgs });
		if (message.channelId !== activeChannel()) {
			updateNotificationCount(message.channelId);
		}
	};

	// TODO: Get this server side as a full state variable with connection statuses
	const getMemberUsernameById = (memberId: string) => {
		const member = members().find(
			(member: Member) => member.id === memberId
		);
		return member ? member.username : 'Unknown Member';
	};

	const updateNotificationCount = (channelId: string) => {
		setNotifications((prevNotifications) => {
			const updatedNotifications = [...prevNotifications];
			const existingEntry = updatedNotifications.find(
				(entry) => entry.channel === channelId
			);

			if (existingEntry) {
				existingEntry.count += 1;
			} else {
				updatedNotifications.push({
					channel: channelId,
					count: 1,
				});
			}

			return updatedNotifications;
		});
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
				activeChannel,
				setActiveChannel,
				notifications,
			}}>
			{props.children}
		</GlobalContext.Provider>
	);
}

export const useGlobalContext = () => useContext(GlobalContext)!;
