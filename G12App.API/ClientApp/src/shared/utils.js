import moment from 'moment';

const HandleDateMask = (e) => {
    let input = e.target;
    
    if(e.charCode < 47 || e.charCode > 57) {
        e.preventDefault();
        return false;
    }

    var len = input.value.length;

    if(len !== 1 || len !== 3) {
        if(e.charCode === 47) {
            e.preventDefault();
        }
    }

    if(len === 2) {
        input.value += '/';
    }

    if(len === 5) {
        input.value += '/';
    }
    return true;
}; 

const ToBRDate = (dateString) => {
    var date = moment(dateString);
    if(date.isValid())
        return date.format('DD/MM/YYYY');
    return null;
}

const FromBRDate = (dateString) => {
    var date = moment(dateString, 'DD/MM/YYYY');
    if(date.isValid()) 
        return date.format('YYYY-MM-DD');
    return null;
}

const StringifyResponseErrors = (errors) => {
    if(errors == null)
        return '';

    var response = [];
    for (let key of Object.keys(errors)) {
        for (let error of errors[key]) {
            response.push(error);
            response.push(<br />);
        }
    }
    
    return response;
};

const FetchPrivate = async (url, options = {}) => {
    var token = localStorage.getItem('jwtToken');

  if (token) {
    options.headers = {
      ...options.headers,
      Authorization: `Bearer ${token}`,
    };
  }

  var result = await fetch(url, options);
  if(result.status === 401) {
    CleanupAuth();
    window.location.href = '/login';
  }

  return result;
};

const SetupAuth = (auth) => {
    localStorage.setItem('auth', JSON.stringify(auth));
    localStorage.setItem('authDate', new Date());
    localStorage.setItem('jwtToken', auth?.user?.token);
}

const CleanupAuth = () => {
    localStorage.removeItem('auth');
    localStorage.removeItem('authDate');
    localStorage.removeItem('jwtToken');
}

const FormatMemberPickerLabel = (member) => {
    if(member == null) return '';
    var leader = (member.computedG12Leader ?? member.computedLeadershipName);
    if (leader) leader += ' / ';
    return (leader ?? '') + member.name;
};

export { HandleDateMask, ToBRDate, FromBRDate, StringifyResponseErrors, FetchPrivate, SetupAuth, CleanupAuth, FormatMemberPickerLabel };