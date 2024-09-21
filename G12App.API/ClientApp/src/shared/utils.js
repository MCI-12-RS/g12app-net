import moment from 'moment';
import $ from 'jquery';

export const BrazilCurrencyFormat = new Intl.NumberFormat('pt-BR', {
    style: 'currency',
    currency: 'BRL',
    minimumFractionDigits: 2,
    maximumFractionDigits: 2
});

export const HandleDateMask = (e) => {
    let input = e.target;

    if (e.charCode < 47 || e.charCode > 57) {
        e.preventDefault();
        return false;
    }

    var len = input.value.length;

    if (len !== 1 || len !== 3) {
        if (e.charCode === 47) {
            e.preventDefault();
        }
    }

    if (len === 2) {
        input.value += '/';
    }

    if (len === 5) {
        input.value += '/';
    }
    return true;
};

export const ToBRDate = (dateString) => {
    var date = moment(dateString);
    if (date.isValid())
        return date.format('DD/MM/YYYY');
    return null;
}

export const FromBRDate = (dateString) => {
    var date = moment(dateString, 'DD/MM/YYYY');
    if (date.isValid())
        return date.format('YYYY-MM-DD');
    return null;
}

export const StringifyResponseErrors = (errors) => {
    if (errors == null)
        return <>Ocorreu um erro!</>;

    if (Array.isArray(errors)) {
        return errors.map(x => <>{x.errorMessage}<br /></>);
    }

    if (typeof (errors) === typeof ({})) {
        return Object.keys(errors).map(x => <>{errors[x].errorMessage}<br /></>);
    }

    if (errors == null)
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
    if (result.status === 401) {
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
    if (member == null) return '';
    var leader = (member.computedG12Leader ?? member.computedLeadershipName);
    return FormatFirstLastName(member.name) + (leader ? ' (' + FormatFirstLastName(leader) + ')' : '');
};

export const TranslateCellType = (cellType) => {
    if (cellType === "Evangelistic")
        return "Evangelística";
    if (cellType === "Discipleship")
        return "Discipuladora"
    if (cellType === "Macrocell")
        return "Macrocélula"
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

export const TranslateWeekDayShort = (weekDay) => {
    if (weekDay === "Monday")
        return "Segunda";
    if (weekDay === "Tuesday")
        return "Terça";
    if (weekDay === "Wednesday")
        return "Quarta";
    if (weekDay === "Thursday")
        return "Quinta";
    if (weekDay === "Friday")
        return "Sexta";
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
        {address?.street && (<><span>{address?.street}{address?.number ? ', ' + address?.number : ''}</span><br /></>)}
        {address?.neighborhood && (<><span><b>Bairro: </b>{address?.neighborhood}</span><br /></>)}
        {address?.city && (<><span><b>Cidade: </b>{address?.city}{address?.state ? ' / ' + address?.state : ''}</span></>)}
    </span>
}

export const TranslateMinistryLevel = (ministryLevel) => {
    switch (ministryLevel) {
        case 1: return 'Pastores';
        case 12: return 'G12';
        default: return ministryLevel;
    }
};

export const ValidateCPF = (cpf) => {
    var Soma = 0
    var Resto

    var strCPF = String(cpf).replace(/[^\d]/g, '')

    if (strCPF.length !== 11)
        return false

    if ([
        '00000000000',
        '11111111111',
        '22222222222',
        '33333333333',
        '44444444444',
        '55555555555',
        '66666666666',
        '77777777777',
        '88888888888',
        '99999999999',
    ].indexOf(strCPF) !== -1)
        return false

    for (let i = 1; i <= 9; i++)
        Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (11 - i);

    Resto = (Soma * 10) % 11

    if ((Resto === 10) || (Resto === 11))
        Resto = 0

    if (Resto !== parseInt(strCPF.substring(9, 10)))
        return false

    Soma = 0

    for (let i = 1; i <= 10; i++)
        Soma = Soma + parseInt(strCPF.substring(i - 1, i)) * (12 - i)

    Resto = (Soma * 10) % 11

    if ((Resto === 10) || (Resto === 11))
        Resto = 0

    if (Resto !== parseInt(strCPF.substring(10, 11)))
        return false

    return true
};

export const UnifyBlankSpaces = (value) => {
    return value.toString().replace(/\s\s+/g, " ");
}

export const FormatFirstLastName = (fullName) => {
    if (fullName == null) return '';
    var names = UnifyBlankSpaces(fullName).trim().split(' ');
    if (names.length === 1) return names[0];
    return names[0] + ' ' + names[names.length - 1];
};

export const FormatFirstName = (fullName) => {
    if (fullName == null) return '';
    return UnifyBlankSpaces(fullName).trim().split(' ')[0];
}

export const CleanSearchForm = (setSearchData) => {
    $('input').val('');
    $('select').each((i, e) => {
        $(e).val($(e).find('option:first').val());
    });
    setSearchData({});
}

export const ParseBool = (value) => {
    if (/true/i.test(value) || value === true)
        return true;
    if (/false/i.test(value) || value === false)
        return false;
    return null;
}