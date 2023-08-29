/* @refresh reload */
import { render } from 'solid-js/web';

import './index.scss';
import App from './App';
import { Router } from '@solidjs/router';
import { GlobalProvider } from './state/GlobalProvider';

const root = document.getElementById('root');

render(
	() => (
		<Router>
			<GlobalProvider>
				<App />
			</GlobalProvider>
		</Router>
	),
	root!
);
