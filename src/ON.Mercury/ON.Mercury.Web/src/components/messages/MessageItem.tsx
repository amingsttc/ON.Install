import { Timestamp } from 'google-protobuf/google/protobuf/timestamp_pb';
import { useGlobalContext } from '../../state/GlobalProvider';
import { Message } from '../../types/message';
import './MessageItem.scss';
import { DateTime } from 'luxon';

type MessageItemProps = {
	message: Message;
};

export function MessageItem({ message }: MessageItemProps) {
	const { getMemberUsernameById } = useGlobalContext();

	const getLocalTime = (timestamp: Timestamp) => {
		const time = DateTime.fromISO(timestamp.toString()).toLocal();
		return `${time.month}/${time.day} ${time.hour}:${time.minute}`;
	};

	return (
		<div class='message-item'>
			<div class='username-date-container'>
				<div class='message-username'>
					{getMemberUsernameById(message.senderId)}
				</div>
				<div class='message-sent-on'>{getLocalTime(message.sentOn)}</div>
			</div>
			<div class='message-body'>{message.body}</div>
		</div>
	);
}
