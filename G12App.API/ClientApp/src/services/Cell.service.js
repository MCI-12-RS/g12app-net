
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
                "leadId": search.leadId,
                "hostId": search.hostId,
                "timoyhyId": search.timoyhyId,
                "weekDay": search.weekDay,
                "placeId": search.placeId,
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

export {SearchCells}