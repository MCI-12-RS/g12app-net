import { FetchPrivate } from '../shared/utils';

const LogReport = async (report) => {
    var response = await FetchPrivate('/api/report/log', {
        method: 'POST',
        body: JSON.stringify(report),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        }
    });

    var obj = await response.json();

    return obj;
}

export {LogReport}