import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { Message } from "@mercury/types/message";
import { RootState } from "src/app/store";

// TODO: Fix non-serializable value in state error
interface MessagesState {
  messages: Record<string, Message[]>;
}

const initialState: MessagesState = {
  messages: {},
};

type MapEntry = {
  channel: string;
  messages: Message[];
};

type AddMessageEntry = {
  channel: string;
  message: Message;
};

export type MessageMapEntry = MapEntry;

export const messagesSlice = createSlice({
  name: "messages",
  initialState,
  reducers: {
    setMessages: (state: MessagesState, action: PayloadAction<MapEntry>) => {
      const { channel, messages } = action.payload;

      if (channel in state.messages) {
        state.messages[channel] = state.messages[channel].concat(messages);
      } else {
        state.messages[channel] = messages;
      }
    },
    addMessage: (
      state: MessagesState,
      action: PayloadAction<AddMessageEntry>,
    ) => {
      state.messages[action.payload.channel].push(action.payload.message);
    },
  },
});

export const { setMessages, addMessage } = messagesSlice.actions;
export const selectMessagesState = (state: RootState) =>
  state.messages.messages;
export const selectChannel = (state: RootState, channelId: string) =>
  state.messages.messages[channelId];
export default messagesSlice.reducer;
