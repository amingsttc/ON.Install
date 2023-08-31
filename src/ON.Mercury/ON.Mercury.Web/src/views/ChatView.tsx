import { useParams } from '@solidjs/router';
import { Sidebar } from '../components/app/Sidebar';
import { MessageLog } from '../components/messages/MessageLog';
import { createEffect } from 'solid-js';
import { useGlobalContext } from '../state/GlobalProvider';

export function ChatView() {
	// TODO: Fix notifications not updating
	const { activeChannel, setActiveChannel, notifications } =
		useGlobalContext();
	const params = useParams();
	let messageNotifications = notifications();

	createEffect(() => {
		let id = params['id'];
		if (activeChannel() !== id) {
			setActiveChannel(id);
		}

		if (messageNotifications.length === 0) {
			messageNotifications.push({
				channel: activeChannel(),
				count: 0,
			});
		}

		if (
			messageNotifications.filter((m) => m.channel === activeChannel())
				.length === 0
		) {
			messageNotifications.push({
				channel: activeChannel(),
				count: 0,
			});
		}

		console.log(messageNotifications);
	}, [params, messageNotifications]);

	return (
		<>
			<Sidebar />
			<MessageLog />
		</>
	);
}
