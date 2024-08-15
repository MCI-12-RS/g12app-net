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

const GetReports = async(search, pagination, sort = {}) => {
    var response = await FetchPrivate('/api/report/search', {
        method: 'POST',
        body: JSON.stringify({
            "pagination":{
                "page": pagination.page,
                "limit": pagination.limit
            },
            "sort": {
                "sortBy": sort.sortBy,
                "order": sort.order
            },
            "filter": {
              "cellId": search.cellId
            }
        }),
        headers: {
            'Content-type': 'application/json; charset=UTF-8'
        }
    });

    var obj = await response.json();

    return obj;
}

const DeleteReport = async (id) => {      
    var response = await FetchPrivate('/api/report/' + id, {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

export { LogReport, GetReports, DeleteReport }