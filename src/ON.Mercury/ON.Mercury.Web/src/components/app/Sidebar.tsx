import { For, createEffect } from 'solid-js';
import './Sidebar.scss';
import { SidebarChannel } from '../channels/SidebarChannel';
import { Link } from '@solidjs/router';
import { useGlobalContext } from '../../state/GlobalProvider';

export function Sidebar() {
	const { channels, currentMember } = useGlobalContext();
	let username = '';

	createEffect(() => {
		if (currentMember()) {
			return () => {
				username = currentMember().username;
			};
		}
	}, username);

	// TODO: fix username break
	return (
		<div class="sidebar">
			<div class="sidebar-header">
				<h3>ServerName</h3>
			</div>
			<div class="container">
				<ul class="list">
					<Link href="/"> Directory</Link>
					<For each={channels()}>
						{(channel) => {
							const { id, name } = channel;
							return (
								<div class="sidebar-channel">
									<SidebarChannel id={id} name={name} />
								</div>
							);
						}}
					</For>
				</ul>
			</div>
			<div class="sidebar-footer">
				<div class="username">
					<h2>@{username || 'username'}</h2>
				</div>
			</div>
		</div>
	);
}
