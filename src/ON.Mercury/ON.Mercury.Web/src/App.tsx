import { Route, Routes, useParams } from '@solidjs/router';
import 'solid-devtools';
import './App.scss';
import { ChatView } from './views/ChatView';
import { createEffect } from 'solid-js';
import { fetchAllChannels } from './api/channels.api';
import { fetchAllRoles } from './api/roles.api';
import { fetchAllMembers, fetchAuthenticatedUser } from './api/auth.api';
import { devConfig } from './config/config';
import { buildConnection } from './signalR/buildConnection';
import { DirectoryView } from './views/DirectoryView';
import { useGlobalContext } from './state/GlobalProvider';
import { Message } from './types/message';
import { Channel } from './types/channel';
import { NewChannelForm } from './components/channels/new-channel/NewChannelForm';
import { Role } from './types/role';

function App() {
	const {
		channels,
		setChannels,
		roles,
		setRoles,
		members,
		setMembers,
		hubConnection,
		setHubConnection,
		addMessage,
		currentMember,
		setCurrentMember,
	} = useGlobalContext();

	createEffect(async () => {
		if (!currentMember()) {
			const currentMemberFound = await fetchAuthenticatedUser();
			if (currentMemberFound) {
				setCurrentMember({
					id: currentMemberFound.id,
					username: currentMemberFound.username,
					roles: currentMemberFound.roles ?? [],
					token: devConfig.token,
				});
			}
		}

		if (channels().length === 0) {
			const channelsFound = await fetchAllChannels();
			if (channelsFound) {
				setChannels(channelsFound);
			}
		}

		if (roles().length === 0) {
			const rolesFound = await fetchAllRoles();
			if (rolesFound) {
				setRoles(rolesFound);
			}
		}

		if (members().length === 0) {
			const membersFound = await fetchAllMembers();
			if (membersFound) {
				setMembers(membersFound);
			}
		}
	});

	createEffect(async () => {
		if (!hubConnection()) {
			const conn = await buildConnection();
			conn.on('ReceiveMessage', (message: Message) => {
				addMessage(message);
			});
			conn.on('ChannelCreated', (channel: Channel) => {
				setChannels((prev) => [...prev, channel]);
			});
			conn.on('ChannelUpdated', (channel: Channel) => {
				console.log(channel);
			});
			conn.on('ChannelDeleted', (channelId: string) => {
				console.log(channelId);
			});
			conn.on('RoleCreated', (role: Role) => {
				setRoles((prev) => [...prev, role]);
			});
			conn.on('RoleUpdated', (role: Role) => {
				console.log(role);
			});
			conn.on('RoleDeleted', (roleId: string) => {
				console.log(roleId);
			});
			conn.on('MessageUpdated', (message: Message) => {
				console.log(message);
			});
			conn.on('MessageDeleted', (messageId: string) => {
				console.log(messageId);
			});
			setHubConnection(conn);
		}
	});

	return (
		<Routes>
			<Route path="/" component={DirectoryView} />
			<Route path="/channels/:id" component={ChatView} />
			<Route path="/channels/new" component={NewChannelForm} />
		</Routes>
	);
}

export default App;
