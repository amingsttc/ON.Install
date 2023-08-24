import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { Message } from "@mercury/types/message";
import { RootState } from "src/app/store";

// TODO: Fix non-serializable value in state error
interface MessagesState {
  messages: Map<string, Message[]>;
}

const initialState: MessagesState = {
  messages: new Map<string, Message[]>(),
};

type MapEntry = {
  channel: string;
  messages: Message[];
};

export type MessageMapEntry = MapEntry;

export const messagesSlice = createSlice({
  name: "messages",
  initialState,
  reducers: {
    setMessages: (state: MessagesState, action: PayloadAction<MapEntry>) => {
      if (state.messages.has(action.payload.channel)) {
        const channel = state.messages.get(action.payload.channel);
        channel?.concat(action.payload.messages);
      } else {
        state.messages.set(action.payload.channel, action.payload.messages);
      }
    },
  },
});

export const { setMessages } = messagesSlice.actions;
export const selectMessagesState = (state: RootState) =>
  state.messages.messages;
export default messagesSlice.reducer;
