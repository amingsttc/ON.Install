import { configureStore } from "@reduxjs/toolkit";
import channelsReducer from "../features/channels/channelsSlice";
import appReducer from "../features/app/appSlice";
import messagesReducer from "../features/messages/messagesSlice";

export const store = configureStore({
  reducer: {
    app: appReducer,
    channels: channelsReducer,
    messages: messagesReducer,
  },
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
