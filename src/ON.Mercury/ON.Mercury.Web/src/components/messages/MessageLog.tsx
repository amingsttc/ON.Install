import { For, createEffect, createSignal } from 'solid-js';
import './MessageLog.scss';
import { Message, SendMessageRequest } from '../../types/message';
import { useParams } from '@solidjs/router';
import { getMessages } from '../../api/channels.api';
import { MessageItem } from './MessageItem';
import { useGlobalContext } from '../../state/GlobalProvider';

export function MessageLog() {
	let messageLogRef!: HTMLDivElement;
	let channelId = useParams().id;
	const [isLockedToBottom, setIsLockedToBottom] = createSignal<boolean>(true);
	const [showScrollButton, setShowScrollButton] = createSignal<boolean>(true);
	const [newMessage, setNewMessage] = createSignal<string>('');
	let channelMessages: Message[] = [];
	const { messages, setMessages, hubConnection, currentMember } =
		useGlobalContext();
	let connection = hubConnection();

	const scrollToBottom = () => {
		if (messageLogRef && isLockedToBottom()) {
			const messageLog = messageLogRef;
			messageLog.scrollTop = messageLog.scrollHeight;
		}
	};

	createEffect(async () => {
		channelId = useParams().id;
		channelMessages = messages()[channelId];
		if (!channelMessages || channelMessages.length === 0) {
			channelMessages = await getMessages(channelId);
			setMessages({ [channelId]: channelMessages });
		} else {
			scrollToBottom();
		}

		if (!connection) {
			connection = hubConnection();
		}
	}, [messages, channelId, channelMessages]);

	const handleScrollToBottomClick = () => {
		setIsLockedToBottom(true);
		scrollToBottom();
		setShowScrollButton(false);
	};

	const handleScroll = () => {
		const messageLog = messageLogRef;
		if (messageLog) {
			const isScrolledToBottom =
				messageLog.scrollHeight - messageLog.scrollTop <=
				messageLog.clientHeight + 10;
			setIsLockedToBottom(isScrolledToBottom);

			// Toggle the visibility of the scroll button
			setShowScrollButton(!isScrolledToBottom);
		}
	};

	const handleSubmit = async () => {
		if (newMessage()) {
			const msg: SendMessageRequest = {
				channelId: channelId,
				senderId: currentMember().id,
				body: newMessage(),
			};

			await hubConnection().invoke('SendMessage', JSON.stringify(msg));
			setNewMessage('');
		}
	};

	const handleKeyPress = async (e: KeyboardEvent) => {
		if (e.key === 'Enter' && newMessage() !== '') {
			await handleSubmit();
		}
	};

	const handleChange = (e: Event) => {
		const target = e.target as HTMLInputElement;
		setNewMessage(target.value);
	};

	createEffect(() => {
		const messageLog = messageLogRef;
		if (messageLog) {
			messageLog.addEventListener('scroll', handleScroll);
		}
		return () => {
			if (messageLog) {
				messageLog.removeEventListener('scroll', handleScroll);
			}
		};
	}, []);

	return (
		<div class='message-log' ref={messageLogRef}>
			<For each={messages()[channelId]}>
				{(message) => {
					return <MessageItem message={message} />;
				}}
			</For>
			<div class='message-input-container'>
				<input
					type='text'
					class='message-input'
					placeholder='Enter your text'
					value={newMessage()}
					onChange={(e) => handleChange(e)}
					onKeyPress={(e) => handleKeyPress(e)}
				/>
				<button
					class='message-submit'
					onClick={async () => await handleSubmit()}
				>
					Submit
				</button>
			</div>
		</div>
	);
}
