import { FetchPrivate } from '../shared/utils';

const GetDashboardData = async () => {
    var response = await FetchPrivate('/api/dashboard', {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8'
        },
    });

    var obj = await response.json();

    return obj;
}

export { GetDashboardData };