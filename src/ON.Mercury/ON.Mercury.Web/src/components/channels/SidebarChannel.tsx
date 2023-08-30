import { Link } from '@solidjs/router';
import './SidebarChannel.scss';
import CustomBulletItem from '../decorations/bullet/CustomBulletItem';

type SidebarChannelProps = {
	id: string;
	name: string;
};

export function SidebarChannel({ id, name }: SidebarChannelProps) {
	return (
		<Link href={`/channels/${id}`} replace={true} class='sidebar-link'>
			<div class='sidebar-channel'>
				<li class='channel-name'>
					<CustomBulletItem bullet='#'>{name}</CustomBulletItem>
				</li>
			</div>
		</Link>
	);
}
