import { Role } from './role';

export type Member = {
	id: string;
	username: string;
	roles: Partial<Role>[];
};

export interface CurrentMember extends Member {
	token: string | undefined;
}
