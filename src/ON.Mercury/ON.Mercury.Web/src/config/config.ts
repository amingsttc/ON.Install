import { DevConfig } from '../types/config';

export const devConfig: DevConfig = {
	apiPath: import.meta.env.VITE_MERCURY_API_PATH,
	token: import.meta.env.VITE_AUTH_TOKEN,
};
