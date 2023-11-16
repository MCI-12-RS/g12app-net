import { FetchPrivate } from '../shared/utils';

const LogReport = async (report) => {
    var response = await FetchPrivate('/api/report/log', {
        method: 'POST',
        body: JSON.stringify(report),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        }
    })
}

export {LogReport}