
const SearchCells = async (search, pagination, sort) => {
    var response = await fetch('/api/cell/search', {
        method: 'POST',
        body: JSON.stringify({
            "pagination": {
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
    })

    var obj = await response.json();

    return obj;
}

const LogReport = async (report) => {
    var response = await fetch('/api/report/log', {
        method: 'POST',
        body: JSON.stringify(report),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        }
    })
}

export {LogReport}