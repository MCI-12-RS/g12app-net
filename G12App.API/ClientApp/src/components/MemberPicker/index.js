import React from 'react';
import { Col, InputGroup } from 'react-bootstrap';
import AsyncSelect from 'react-select/async';
import { SearchMembers } from '../../services/Member.service';
import { debounce } from 'lodash';
import './style.css';

const NoLeadershipText = 'Sem liderança (nível base)';

const MemberPicker = ({
    label = 'Membro',
    ministryLevel,
    setValue,
    noOptionsMessage,
    placeholder,
    minLength = 2, 
    notInIds = [],
    selectedValue = null,
    selectedLabel = null,
    isMulti = false,
}) => {

    const [option, setOption] = React.useState({ label: selectedLabel, value: selectedValue });

    const getNoOptionsMessage = (data) => {
        if(data.inputValue?.length < minLength) return placeholder ?? "Digite para pesquisar...";
        return noOptionsMessage ?? "Nenhum membro encontrado";
    }
        
    const loadMembers = async (inputValue) => {
        if (!inputValue || inputValue.length < minLength) {
            return [];
        }
        
        var result = await SearchMembers(
            { name: inputValue, ministryLevel: ministryLevel, notInIds: notInIds }, 
            { page: 1, limit: 10 }, 
            { sortBy: 'name', sortOrder: 'asc' })

        if(!result.isValid) return [];

        return result.data.map((member) => ({
            label: member.name,
            value: member.id
        }));
    }

    const handleSelectChangeEvent = (option) => {
        setOption(option);
        if(setValue != null)
            setValue(option?.value);
    }

    const randomKey = (Math.random() * 1000).toFixed(0).toString();

    return (        
        <InputGroup className="mb-3" as={Col}>
            <InputGroup.Text id="basic-addon1">{label}</InputGroup.Text>
            <AsyncSelect 
                name={"member" + randomKey} aria-describedby="basic-addon1" className="async-select" 
                onChange={handleSelectChangeEvent} 
                noOptionsMessage={getNoOptionsMessage}
                value={option}
                defaultOptions={[ {label: NoLeadershipText, value: null} ]}
                placeholder="" loadOptions={loadMembers}
                isMulti={isMulti}>
                    
            </AsyncSelect>
        </InputGroup>
    )
}

export default MemberPicker;