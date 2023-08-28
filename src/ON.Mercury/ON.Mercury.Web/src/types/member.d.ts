export type Member = {
  id: string;
  username: string;
  roles: Role[];
};

export type GetCurrentMemberResponse = {
  isSuccess: boolean;
  errors: string;
  member: Member;
};
