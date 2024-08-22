import moment from 'moment';

export const BrazilCurrencyFormat = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
});

export const HandleDateMask = (e) => {
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

export const ToBRDate = (dateString) => {
    var date = moment(dateString);
    if(date.isValid())
        return date.format('DD/MM/YYYY');
    return null;
}

export const FromBRDate = (dateString) => {
    var date = moment(dateString, 'DD/MM/YYYY');
    if(date.isValid()) 
        return date.format('YYYY-MM-DD');
    return null;
}

export const StringifyResponseErrors = (errors) => {    
    if(errors == null)
        return <>Ocorreu um erro!</>;

    if(Array.isArray(errors)) {
        return errors.map(x => <>{x.errorMessage}<br /></>);
    }
    
    if(typeof(errors) === typeof({})) {
        return Object.keys(errors).map(x => <>{errors[x].errorMessage}<br /></>);
    }

    if(errors == null)
        return <>Ocorreu um erro!</>;
};

export const FetchPrivate = async (url, options = {}) => {
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

export const SetupAuth = (auth) => {
    localStorage.setItem('auth', JSON.stringify(auth));
    localStorage.setItem('authDate', new Date());
    localStorage.setItem('jwtToken', auth?.user?.token);
}

export const CleanupAuth = () => {
    localStorage.removeItem('auth');
    localStorage.removeItem('authDate');
    localStorage.removeItem('jwtToken');
}

export const FormatMemberPickerLabel = (member) => {
    if(member == null) return '';
    var leader = (member.computedG12Leader ?? member.computedLeadershipName);
    if (leader) leader += ' / ';
    return (leader ?? '') + member.name;
};

export const TranslateCellType = (cellType) => {
    if (cellType === "Evangelistic")
        return "Evangelística";
    if (cellType === "Discipleship")
        return "Discipuladora"
};

export const TranslateWeekDay = (weekDay) => {
    if (weekDay === "Monday")
        return "Segunda-Feira";
    if (weekDay === "Tuesday")
        return "Terça-Feira";
    if (weekDay === "Wednesday")
        return "Quarta-Feira";
    if (weekDay === "Thursday")
        return "Quinta-Feira";
    if (weekDay === "Friday")
        return "Sexta-Feira";
    if (weekDay === "Saturday")
        return "Sábado";
    if (weekDay === "Sunday")
        return "Domingo";
}

export const TranslateMomentWeekDay = (weekDay) => {
    if (weekDay === "Monday")
        return 1;
    if (weekDay === "Tuesday")
        return 2;
    if (weekDay === "Wednesday")
        return 3;
    if (weekDay === "Thursday")
        return 4;
    if (weekDay === "Friday")
        return 5;
    if (weekDay === "Saturday")
        return 6;
    if (weekDay === "Sunday")
        return 0;
}

export const MoneyToDecimal = (value) => {
    let decimal = value || '';
    
    decimal = decimal.replace('R$', '')
                     .replace(/\./g, '')
                     .replace(/,/g, '.');
                     
    return parseFloat(decimal);
}

export const DecimalToMoney = (value) => {  
    return BrazilCurrencyFormat.format(value);
}

export const RenderGridAddress = (address) => {
    return <span>
        {!address?.street && !address?.neighborhood && !address?.city ? 'Não informado' : ''}
        {address?.street && (<><span><b>Rua: </b>{address?.street}{address?.number ? ', ' + address?.number : ''}</span><br /></>)}
        {address?.neighborhood && (<><span><b>Bairro: </b>{address?.neighborhood}</span><br /></>)}
        {address?.city && (<><span><b>Cidade: </b>{address?.city}{address?.state ? ' / ' + address?.state : ''}</span></>)}
      </span>
}

export const TranslateMinistryLevel = (ministryLevel) => {
    switch(ministryLevel){
      case 1: return 'Pastores';
      case 12: return 'G12';
      default: return ministryLevel;
    }
  };