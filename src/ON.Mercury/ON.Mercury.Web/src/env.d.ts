/// <reference types="vite/client" />
interface ImportMetaEnv {
  readonly NODE_ENV: string;
  readonly VITE_MERCURY_BASE_API: string;
  readonly VITE_AUTH_TOKEN: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
