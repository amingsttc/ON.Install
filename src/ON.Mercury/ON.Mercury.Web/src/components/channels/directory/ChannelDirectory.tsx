import { For } from 'solid-js';
import './ChannelDirectory.scss';
import { Link } from '@solidjs/router';
import { useGlobalContext } from '../../../state/GlobalProvider';

export function ChannelDirectory() {
	const { channels } = useGlobalContext();

	return (
		<div class="channel-list">
			<For each={channels()}>
				{(channel) => {
					const { id, name } = channel;
					return (
						<div>
							<Link href={`/channels/${id}`} class="list-item">
								{name}
							</Link>
						</div>
					);
				}}
			</For>
		</div>
	);
}
