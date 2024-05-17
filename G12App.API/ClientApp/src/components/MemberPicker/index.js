import React, { useEffect, useCallback } from 'react';
import { Col, InputGroup } from 'react-bootstrap';
import AsyncSelect from 'react-select/async';
import { SearchMembers } from '../../services/Member.service';
import { debounce } from 'lodash';
import { FormatMemberPickerLabel } from '../../shared/utils';
import './style.css';

const MemberPicker = ({
    label = 'Membro',
    ministryLevel,
    setValue,
    noOptionsMessage,
    placeholder,
    initialValue = 'Sem liderança (nível base)',
    minLength = 2,
    notInIds = [],
    selectedValue = null,
    selectedLabel = null,
    isMulti = false,
}) => {

    const [option, setOption] = React.useState(null);
    const [defaultOption, setDefaultOption] = React.useState([]);

    const getNoOptionsMessage = (data) => {
        if (data.inputValue?.length < minLength) return placeholder ?? "Digite para pesquisar...";
        return noOptionsMessage ?? "Nenhum membro encontrado";
    }

    const loadMembers = useCallback(
        debounce((inputValue, callback) => {
            _loadMembers(inputValue).then(options => callback(options)).catch((error) => console.log(error));
        }, 500),
        []
    );

    const _loadMembers = async (inputValue) => {
        if (!inputValue || inputValue.length < minLength) {
            return [];
        }

        var result = await SearchMembers(
            { name: inputValue, ministryLevel: ministryLevel, notInIds: notInIds },
            { page: 1, limit: 10 },
            { sortBy: 'name', sortOrder: 'asc' })

        if (!result.isValid) return [];

        return result.data.map((member) => (
            {
                label: FormatMemberPickerLabel(member),
                value: member.id
            }
        ));
    }

    const handleSelectChangeEvent = (option) => {        
        setOption(option);

        if (!isMulti) {        
            if (setValue != null)
                setValue(option?.value);
        } else {
            if (setValue != null)
                setValue(option?.map(x => x.value));
        }
    }

    const randomKey = (Math.random() * 1000).toFixed(0).toString();

    useEffect(() => {
        if (!isMulti) {
            setDefaultOption([{ label: initialValue, value: null }]);

            if (selectedValue != null)
                setOption({ label: selectedLabel, value: selectedValue });
            else if (option?.value == null)
                setOption({ label: initialValue, value: null });
        }
    }, []);

    return (
        <InputGroup className="mb-3" as={Col}>
            <InputGroup.Text id="basic-addon1">{label}</InputGroup.Text>
            <AsyncSelect
                name={"member" + randomKey} aria-describedby="basic-addon1" className="async-select"
                onChange={handleSelectChangeEvent}
                noOptionsMessage={getNoOptionsMessage}
                value={option}
                defaultOptions={defaultOption}
                placeholder="" loadOptions={loadMembers}
                isMulti={isMulti} />
        </InputGroup>
    )
}

export default MemberPicker;