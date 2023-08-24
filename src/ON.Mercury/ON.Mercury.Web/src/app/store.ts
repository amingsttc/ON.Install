import { configureStore } from "@reduxjs/toolkit";
import channelsReducer from "../features/channels/channelsSlice";
import appReducer from "../features/app/appSlice";

export const store = configureStore({
  reducer: {
    app: appReducer,
    channels: channelsReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
