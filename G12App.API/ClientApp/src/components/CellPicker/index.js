import React, { useEffect, useCallback } from 'react';
import { SearchCells } from "../../services/Cell.service";
import { Col, InputGroup } from "react-bootstrap";
import AsyncSelect from "react-select/async";
import { debounce } from 'lodash';
import './style.css';

const CellPicker = ({
    label = 'Célula',
    setValue,
    noOptionMessage,
    placeholder,
    minLength = 3,
    noInIds,
    initialValue = 'Insira o nome do líder da célula...',
    selectedValue = null,
    selectedLabel = null,
}) => { 
    const [option, setOption] = React.useState({ label: selectedLabel, value: selectedValue });

    const getNoOptionsMessage = (data) => {
        if (data.inputValue?.length < minLength) return placeholder ?? "Digite para pesquisar...";
        return noOptionMessage ?? "Nenhuma célula encontrada";
    }

    const loadCells = useCallback(
        debounce((inputValue, callback) => {
            _loadCells(inputValue).then(options => callback(options))
        }, 500),
        []
    );

    const _loadCells = async (inputValue) => {
        if (!inputValue || inputValue.length < minLength) {
            return [];
        }

        var result = await SearchCells(
            { lead: inputValue },
            { page: 1, limit: 10 },
            { sortBy: 'lead.name', sortOrder: 'asc' })

        if (!result.isValid) return [];

        return result.data.map((cell) => ({
            label: 
            'Líder: ' + cell.lead?.name +
            (cell.place?.street ? ' / Rua: ' + cell.place?.street : '') +
            (cell.place?.neighborhood ? ' / Bairro: ' + cell.place?.neighborhood : ''),
            value: cell.id
        }));
    }

    const handleSelectChangeEvent = (option) => {
        setOption(option);
        if (setValue != null)
            setValue(option?.value);
    }

    useEffect(() => {
        if (option.value == null)
            setOption({ label: initialValue, value: null });
    }, []);

    const randomKey = (Math.random() * 100).toFixed(0).toString();

    return (
        <InputGroup className="mb-3" as={Col}>
            <InputGroup.Text id="basic-addon1">{label}</InputGroup.Text>
            <AsyncSelect
                name={"cell" + randomKey} aria-describedby="basic-addon1" className="async-select"
                onChange={handleSelectChangeEvent}
                noOptionsMessage={getNoOptionsMessage}
                value={option}
                defaultOptions={[{ label: initialValue, value: null }]}
                placeholder="" loadOptions={loadCells}>
            </AsyncSelect>
        </InputGroup>
    )
}

export default CellPicker;