import { FetchPrivate } from '../shared/utils';

const SearchCells = async (search, pagination, sort) => {
    var response = await FetchPrivate('/api/cell/search', {
        method: 'POST',
        body: JSON.stringify({
            "pagination":{
                "page": pagination.page,
                "limit": pagination.limit
            },
            "sort": {
                "sortBy": "",
                "sortOrder": ""
            },
            "filter": {
              "lead": search.lead,
              "leadership": search.leadership,
              "host": search.host,
              "timoyhy": search.timoyhy,
              "weekDay": search.weekDay == "" ? null : search.weekDay,
              "homeAddress_Neighborhood": search.neighborhood,
              "homeAddress_City": search.city,
              "homeAddress_State": search.state,
              "homeAddress_Street": search.street,
              "cellType": search.cellType == "" ? null : search.cellType,
            }
        }),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

const GetCell = async (id) => {
    var response = await FetchPrivate('/api/cell/' + id, {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8'
        },
    });

    var obj = await response.json();

    return obj;
    
}

const CreateCell = async (cell) => {
    var response = await FetchPrivate('/api/cell', {
        method: 'POST',
        body: JSON.stringify(cell),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

const UpdateCell = async (cell) => {
    var response = await FetchPrivate('/api/cell/' + cell.id, {
        method: 'PUT',
        body: JSON.stringify(cell),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

const DeleteCell = async (id) => {
    var response = await FetchPrivate('/api/cell/' + id, {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

const GetLastReportsByCellId = async (id) => {
    var response = await FetchPrivate('/api/cell/' + id + 'reports', {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8'
        },
    });

    var obj = await response.json();

    return obj;
    
}
export {SearchCells, GetCell, CreateCell, UpdateCell, DeleteCell, GetLastReportsByCellId };
