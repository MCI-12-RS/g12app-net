import React, { useEffect, useCallback } from 'react';
import { SearchCells } from "../../services/Cell.service";
import { Form, InputGroup } from "react-bootstrap";
import AsyncSelect from "react-select/async";
import { debounce } from 'lodash';
import './style.css';
import { FormatFirstLastName, TranslateWeekDayShort } from '../../shared/utils';

const CellPicker = ({
    label = 'Célula',
    setValue,
    noOptionMessage,
    placeholder,
    minLength = 3,
    initialValue = 'Insira o nome do líder da célula...',
    selectedValue = null,
    selectedLabel = null,
    changeValueEvent = () => { }
}) => {
    const [option, setOption] = React.useState({ label: selectedLabel, value: selectedValue });

    const getNoOptionsMessage = (data) => {
        if (data.inputValue?.length < minLength) return placeholder ?? "Digite para pesquisar...";
        return noOptionMessage ?? "Nenhuma célula encontrada";
    }

    const loadCells = useCallback(
        debounce((inputValue, callback) => {
            _loadCells(inputValue).then(options => callback(options)).catch((error) => console.log(error));
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
                'Líder: ' + FormatFirstLastName(cell.lead?.name) + (cell.lead?.computedG12Leader ? ' | G12: ' + FormatFirstLastName(cell.lead?.computedG12Leader) : '') +
                (cell.place?.street || cell.place?.neighborhood ? ' | ' : '') +
                (cell.place?.street ? ' Rua: ' + cell.place?.street + (cell.place?.number ? ', ' + cell.place?.number : '') : '') +
                (cell.place?.street && cell.place?.neighborhood ? ' - ' : '') +
                (cell.place?.neighborhood ? cell.place?.neighborhood : '') +
                (' | ' + TranslateWeekDayShort(cell.weekDay) + ' às ' + cell.time.substring(0, 5)),
            value: cell.id,
            item: cell
        }));
    }

    const handleSelectChangeEvent = (option) => {
        setOption(option);
        if (setValue != null) {
            setValue(option?.value);
            changeValueEvent(option);
        }
    }

    useEffect(() => {
        if (option.value == null)
            setOption({ label: initialValue, value: null });
    }, []);

    const randomKey = (Math.random() * 100).toFixed(0).toString();

    return (
        <>
            {label && (<Form.Label>{label}</Form.Label>)}
            <InputGroup>
                <AsyncSelect
                    name={"cell" + randomKey} className="async-select"
                    onChange={handleSelectChangeEvent}
                    noOptionsMessage={getNoOptionsMessage}
                    value={option}
                    defaultOptions={[{ label: initialValue, value: null }]}
                    placeholder="" loadOptions={loadCells} />
            </InputGroup>
        </>
    )
}

export default CellPicker;