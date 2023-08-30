import { JSXElement } from 'solid-js';

type CustomBulletProps = {
	bullet: string;
	children: JSXElement;
};

export default function CustomBulletItem({
	bullet,
	children,
}: CustomBulletProps) {
	return (
		<li class='item'>
			<span class='custom-bullet'>{bullet}</span>
			{children}
		</li>
	);
}
