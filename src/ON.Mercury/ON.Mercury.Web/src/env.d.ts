/// <reference types="vite/client" />
interface ImportMetaEnv {
	readonly VITE_MERCURY_API_PATH: string;
	readonly VITE_AUTH_TOKEN: string;
}

interface ImportMeta {
	readonly env: ImportMetaEnv;
}
