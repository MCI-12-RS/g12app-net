
const SearchMembers = async (search, pagination, sort) => {
    
    var response = await fetch('/api/member/search', {
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
              "name": search.name,
              "leadershipName": search.leadershipName,
              "email": search.email,
              "homeAddress_Neighborhood": search.neighborhood,
              "homeAddress_City": search.city,
              "homeAddress_State": search.state,
            }
          }),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

const GetMember = async (id) => {
    
    var response = await fetch('/api/member/' + id, {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

export { SearchMembers, GetMember };
