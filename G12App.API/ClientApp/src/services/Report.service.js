import download from 'downloadjs';
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

const GetReports = async (search, pagination, sort = {}) => {
    var response = await FetchPrivate('/api/report/search', {
        method: 'POST',
        body: JSON.stringify({
            "pagination": {
                "page": pagination.page,
                "limit": pagination.limit
            },
            "sort": {
                "sortBy": sort.sortBy,
                "order": sort.order
            },
            "filter": {
                "cellId": search.cellId,
                "leadership": search.leadership,
                "g12Leadership": search.g12Leadership,
                "startDate": search.startDate,
                "endDate": search.endDate,
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

const DownloadExportReports = async (search, pagination, sort = {}) => {
    FetchPrivate('/api/export/cell-reports', {
        method: 'POST',
        body: JSON.stringify({
            "pagination": {
                "page": pagination.page,
                "limit": pagination.limit
            },
            "sort": {
                "sortBy": sort.sortBy,
                "order": sort.order
            },
            "filter": {
                "cellId": search.cellId,
                "leadership": search.leadership,
                "g12Leadership": search.g12Leadership,
                "startDate": search.startDate,
                "endDate": search.endDate,
            }
        }),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    }).then(function (resp) {
        console.log(resp);
        return resp.blob();
    }).then(function (blob) {
        return download(blob, "Relatorios.xlsx");
    }).catch(function (err) {
        console.log(err);
    });
}

export { LogReport, GetReports, DeleteReport, DownloadExportReports }