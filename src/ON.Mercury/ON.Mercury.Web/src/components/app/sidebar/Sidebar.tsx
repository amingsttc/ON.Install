import { For, createEffect, createMemo } from 'solid-js';
import './Sidebar.scss';
import { SidebarChannel } from '../../channels/SidebarChannel';
import { Link } from '@solidjs/router';
import { useGlobalContext } from '../../../state/GlobalProvider';
import CustomBulletItem from '../../decorations/bullet/CustomBulletItem';

import profileDefaultImage from '../../../assets/profile_default.png';
export function Sidebar() {
	const { channels, currentMember, notifications } = useGlobalContext();
	const memoUsername = createMemo(() => {
		return currentMember() ? currentMember().username : 'username';
	});

	return (
		<div class='sidebar'>
			{/* <div class='sidebar-header'>
				<h3>ServerName</h3>
			</div> */}
			<div class='container'>
				<ul class='list'>
					<Link href='/' class='sidebar-link sidebar-channel'>
						<CustomBulletItem bullet='#'>Directory</CustomBulletItem>
					</Link>
					<For each={channels()}>
						{(channel) => {
							const { id, name } = channel;
							return (
								<div>
									<SidebarChannel id={id} name={name} />
								</div>
							);
						}}
					</For>
				</ul>
			</div>
			<div class='sidebar-footer username-container'>
				<div class='message-avatar'>
					<img src={profileDefaultImage} alt='Profile' />
				</div>
				<div>
					<h3>@{memoUsername()}</h3>
				</div>
			</div>
		</div>
	);
}
