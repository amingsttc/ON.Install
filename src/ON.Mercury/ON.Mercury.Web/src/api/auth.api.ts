import axios, { AxiosResponse } from 'axios';
import { devConfig } from '../config/config';
import { headers } from './headers';
import { Member } from '../types/member';

type fetchAuthenticatedUserResponse = {
	isSuccess: boolean;
	errors: string;
	member: Member;
};

export async function fetchAllMembers(): Promise<Member[]> {
	const result: AxiosResponse = await axios.get(
		`${devConfig.apiPath}/auth/members`,
		{ headers }
	);

	const members: Member[] = result.data;

	return members;
}

export async function fetchAuthenticatedUser(): Promise<Member> {
	const result: AxiosResponse<fetchAuthenticatedUserResponse> =
		await axios.get(`${devConfig.apiPath}/auth`, {
			headers,
		});
	const member: Member = result.data.member;
	return member;
}
