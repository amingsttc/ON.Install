import { AxiosHeaders, AxiosHeaderValue } from 'axios';
import { devConfig } from '../config/config';

export const headers: AxiosHeaders = new AxiosHeaders({
	Authorization: `Bearer ${devConfig.token}`,
	'Content-Type': 'application/json',
	Accept: 'application/json',
});
