import { createSignal } from 'solid-js';
import './NewChannelForm.scss';
import { Role } from '../../../types/role';
import { useGlobalContext } from '../../../state/GlobalProvider';
import { CreateChannelRequest } from '../../../types/channel';
import { createChannel } from '../../../api/channels.api';

export function NewChannelForm() {
	const { roles } = useGlobalContext();
	const [isPrivateChannel, setIsPrivateChannel] = createSignal(false);
	const [isPrivateString, setIsPrivateString] = createSignal(
		isPrivateChannel() ? 'Public' : 'Private'
	);
	const [selectedRoles, setSelectedRoles] = createSignal<Role[]>([]);
	const [newChannel, setNewChannel] = createSignal<CreateChannelRequest>({
		roles: [],
		category: 'uncategorized',
		name: '',
		description: 'none',
	});
	let nameInputRef!: HTMLInputElement;

	const onSubmit = async (e: SubmitEvent) => {
		e.preventDefault();
		if (newChannel().name !== '') {
			await createChannel(newChannel());
		}
	};

	const onChange = (e: Event) => {
		setNewChannel({
			name: nameInputRef.value,
			description: newChannel().description,
			category: newChannel().category,
			roles: newChannel().roles,
		});
	};

	const onToggle = () => {
		setIsPrivateChannel(!isPrivateChannel());
		setNewChannel({
			name: newChannel().name,
			description: newChannel().description,
			category: newChannel().category,
			roles: [],
		});
		setSelectedRoles([]);
	};

	const onSelect = (e: Event) => {
		const target = e.target as HTMLInputElement;
		const roleId = target.id.split('role-')[1];

		if (target.checked) {
			setSelectedRoles((prev) => [...prev, roleId]);
		} else {
			setSelectedRoles((prev) => prev.filter((id) => id !== roleId));
		}

		setNewChannel({
			name: newChannel().name,
			description: newChannel().description,
			category: newChannel().category,
			roles: selectedRoles(),
		});
	};

	const hideModal = () => {};

	return (
		<>
			<form class="new-channel-form" onSubmit={(e) => onSubmit(e)}>
				<label for="name" class="input-label">
					Channel Name
				</label>
				<input
					type="text"
					id="name"
					name="name"
					placeholder="channel-name"
					value={newChannel().name}
					onChange={(e) => onChange(e)}
					ref={nameInputRef}
					class="rounded-input"
				/>
				<div class="toggle-header">
					<h1 class="toggle-heading">
						{isPrivateChannel() ? 'Private' : 'Public'}
					</h1>
					<div class="toggle-container">
						<input
							type="checkbox"
							id="toggleCategory"
							name="category"
							checked={isPrivateChannel()}
							onChange={() => onToggle()}
							class="toggle-input"
						/>
						<label for="toggleCategory" class="toggle-label">
							<span class="toggle-indicator" />
						</label>
					</div>
				</div>
				{isPrivateChannel() && (
					<div class="roles-box">
						<h2>Roles</h2>
						<div class="roles-grid">
							{roles().map((role: Role) => (
								<div class="role-item">
									<input
										type="checkbox"
										id={`role-${role.id}`}
										name={`role-${role.id}`}
										class="role-checkbox"
										onChange={(e) => onSelect(e)}
									/>
									<label
										for={`role-${role.id}`}
										class="role-label">
										{role.name}
									</label>
								</div>
							))}
						</div>
					</div>
				)}
				<div class="submit-button-container">
					<button type="button" onClick={hideModal}>
						Back
					</button>
					<button type="submit">Submit</button>
				</div>
			</form>
		</>
	);
}
