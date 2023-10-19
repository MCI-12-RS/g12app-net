
const SearchCells = async (search, pagination, sort) => {
    var response = await fetch('/api/cell/search', {
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
              "host": search.host,
              "timoyhy": search.timoyhy,
              "weekDay": search.weekDay,
              "homeAddress_Neighborhood": search.neighborhood,
              "homeAddress_City": search.city,
              "homeAddress_State": search.state,
              "homeAddress_Street": search.street,
              "cellType": search.cellType,
            }
        }),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    })

    var obj = await response.json();

    return obj;
}

const GetCell = async (id) => {
    var response = await fetch('/api/cell/' + id, {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8'
        },
    });

    var obj = await response.json();

    return obj;
    
}

export {SearchCells, GetCell}