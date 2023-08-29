import axios, { AxiosResponse } from 'axios';
import { devConfig } from '../config/config';
import { headers } from './headers';
import { Role } from '../types/role';

export async function fetchAllRoles(): Promise<Role[]> {
	const result: AxiosResponse = await axios.get(
		`${devConfig.apiPath}/roles`,
		{
			headers,
		}
	);
	const roles: Role[] = result.data;
	return roles;
}
