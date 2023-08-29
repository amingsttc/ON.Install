import { Sidebar } from '../components/app/Sidebar';
import { ChannelDirectory } from '../components/channels/directory/ChannelDirectory';

export function DirectoryView() {
	return (
		<>
			<Sidebar />
			<ChannelDirectory />
		</>
	);
}
