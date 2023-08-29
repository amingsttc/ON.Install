import { Link } from '@solidjs/router';
import './SidebarChannel.scss';

type SidebarChannelProps = {
	id: string;
	name: string;
};

export function SidebarChannel({ id, name }: SidebarChannelProps) {
	return (
		<Link href={`/channels/${id}`} replace={true} class="sidebar-link">
			<div class="sidebar-channel">
				<li class="channel-name">
					<span class="custom-bullet">#</span>
					{name}
				</li>
			</div>
		</Link>
	);
}
