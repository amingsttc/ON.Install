import './Topbar.scss';
import hamburger from '../../../assets/Hamburger_icon.svg';
import { useGlobalContext } from '../../../state/GlobalProvider';

interface TopbarProps {
	backgroundColor: string;
	textColor: string;
	serverName: string;
}

export function Topbar({
	backgroundColor,
	textColor,
	serverName = 'Server Name',
}: TopbarProps) {
	const { activeChannel } = useGlobalContext();
	return (
		<div
			class='topbar-container'
			style={`background-color: ${backgroundColor}; color:  ${textColor}`}
		>
			<div class='topbar-section'>
				<img src={hamburger} class='hamburger-icon' />
				<h2 class='section-heading'>{serverName}</h2>
			</div>
			<div class='topbar-section'>
				<h2 class='section-heading'>
					{activeChannel() ? activeChannel() : ''}
				</h2>
			</div>
		</div>
	);
}
