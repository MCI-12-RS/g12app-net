import { FetchPrivate, ParseBool } from '../shared/utils';

const GetMinistryChildren = async (id) => {
    var response = await FetchPrivate('/api/ministry/' + id + '/couples', {
        method: 'GET',
        headers: {
            'Content-type': 'application/json; charset=UTF-8',
        },
    });

    var obj = await response.json();

    return obj;
}

export { GetMinistryChildren };
