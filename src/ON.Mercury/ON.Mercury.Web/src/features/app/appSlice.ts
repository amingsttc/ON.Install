import { PayloadAction, createSlice } from "@reduxjs/toolkit";
import { Member } from "../../types/member";
import { Role } from "../../types/roles";
import { RootState } from "../../app/store";

interface AppState {
  roles: Role[];
  members: Member[];
  loggedInUser: Member | undefined;
}

const initialState: AppState = {
  roles: [],
  members: [],
  loggedInUser: undefined,
};

export const appSlice = createSlice({
  name: "app",
  initialState,
  reducers: {
    setRoles: (state, action: PayloadAction<Role[]>) => {
      state.roles = action.payload;
    },
    addRole: (state, action: PayloadAction<Role>) => {
      state.roles.push(action.payload);
    },
    setMembers: (state, action: PayloadAction<Member[]>) => {
      state.members = action.payload;
    },
    addMember: (state, action: PayloadAction<Member>) => {
      state.members.push(action.payload);
    },
    setLoggedInUser: (state, action: PayloadAction<Member | undefined>) => {
      state.loggedInUser = action.payload;
    },
  },
});

export const { setRoles, addRole, setMembers, addMember, setLoggedInUser } =
  appSlice.actions;
export const selectRoles = (state: RootState) => state.app.roles;
export const selectMembers = (state: RootState) => state.app.members;
export const selectLoggedInUsername = (state: RootState) =>
  state.app.loggedInUser?.username;
export const selectLoggedInUser = (state: RootState) => state.app.loggedInUser;
export const selectUsernameById = (state: RootState, id: string) =>
  state.app.members.find((m) => m.id === id);
export default appSlice.reducer;
