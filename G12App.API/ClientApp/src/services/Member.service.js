import { FetchPrivate } from '../shared/utils';

const SearchMembers = async (search, pagination, sort = {}) => {
    var response = await FetchPrivate('/api/member/search', {
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
                "name": search.name,
                "leadershipName": search.leadershipName,
                "email": search.email,
                "homeAddress_Neighborhood": search.neighborhood,
                "homeAddress_City": search.city,
                "homeAddress_State": search.state,
                "ministryLevel": search.ministryLevel ? parseInt(search.ministryLevel) : null,
                "notInId": search.notInIds,
                "idle": search.idle ? /true/i.test(search.idle) : null,
                "hasNoLeadership": search.hasNoLeadership ? /true/i.test(search.hasNoLeadership) : null,
                "baptized": search.baptized ? /true/i.test(search.baptized) : null,
                "cellLink": search.cellLink ? search.cellLink : null,
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

    var response = await FetchPrivate('/api/member/' + id, {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

const CreateMember = async (member) => {
    var response = await FetchPrivate('/api/member', {
        method: 'POST',
        body: JSON.stringify(member),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

const UpdateMember = async (member) => {
    var response = await FetchPrivate('/api/member/' + member.id, {
        method: 'PUT',
        body: JSON.stringify(member),
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    let obj = await response.json();

    return obj;
}

const DeleteMember = async (id) => {
    var response = await FetchPrivate('/api/member/' + id, {
        method: 'DELETE',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

export { SearchMembers, GetMember, CreateMember, UpdateMember, DeleteMember };
