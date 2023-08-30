import { Route, Routes } from '@solidjs/router';
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
