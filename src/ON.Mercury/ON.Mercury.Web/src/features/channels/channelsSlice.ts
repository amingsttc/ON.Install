import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import type { RootState } from "../../app/store";
import { Channel } from "../../types/channel";

interface ChannelsState {
  channels: Channel[];
}

const initialState: ChannelsState = {
  channels: [],
};

export const channelsSlice = createSlice({
  name: "channels",
  initialState,
  reducers: {
    setChannels: (state, action: PayloadAction<Channel[]>) => {
      state.channels = action.payload;
    },
    addChannel: (state, action: PayloadAction<Channel>) => {
      state.channels.push(action.payload);
    },
    removeChannel: (state, action: PayloadAction<Channel>) => {
      state.channels.splice(state.channels.indexOf(action.payload), 1);
    },
  },
});

export const { setChannels, addChannel, removeChannel } = channelsSlice.actions;
export const selectChannels = (state: RootState) => state.channels.channels;
export default channelsSlice.reducer;
