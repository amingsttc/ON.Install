import { Sidebar } from '../components/app/Sidebar';
import { MessageLog } from '../components/messages/MessageLog';

export function ChatView() {
	return (
		<>
			<Sidebar />
			<MessageLog />
		</>
	);
}
