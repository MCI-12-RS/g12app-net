import moment from 'moment';

const HandleDateMask = (e) => {
    let input = e.target;
    
    if(e.charCode < 47 || e.charCode > 57) {
        e.preventDefault();
        return false;
    }

    var len = input.value.length;

    if(len !== 1 || len !== 3) {
        if(e.charCode == 47) {
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

export { HandleDateMask, ToBRDate, FromBRDate }
