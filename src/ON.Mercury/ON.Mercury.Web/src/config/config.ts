import { AppConfig } from "../types/appconfig";

export const config: AppConfig = {
  env: import.meta.env.NODE_ENV,
  mercuryApi: import.meta.env.VITE_MERCURY_BASE_API,
};
