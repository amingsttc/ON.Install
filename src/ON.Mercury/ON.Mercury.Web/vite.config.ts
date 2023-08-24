import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  resolve: {
    alias: {
      "@mercury": "./src/",
      "@styles": "./src/assets/styles",
      "@components": "./src/components",
      "@channels": "./src/components/channels",
      "@mercury/types/": "./src/types",
    },
  },
  plugins: [react()],
});
