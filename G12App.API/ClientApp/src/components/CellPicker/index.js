import React from "react";
import { SearchCells } from "../../services/Cell.service";
import { Col, InputGroup } from "react-bootstrap";
import AsyncSelect from "react-select/dist/declarations/src/Async";
import './style.css';

const CellPicker = ({
    label = 'Célula',
    setValue,
    noOptionMessage,
    placeholder,
    minLength = 3,
    noInIds,
    selectedValue = null,
    selectedLabel = null,
}) => {
    const [option, setOption] = React.useState({ label: selectedLabel, value: selectedValue });

    const getNoOptionsMessage = (data) => {
        if (data.inputValue?.length < minLength) return placeholder ?? "Digite para pesquisar...";
        return noOptionMessage ?? "Nenhum membro encontrado";
    }

    const loadCells = async (inputValue) => {
        if (!inputValue || inputValue.length < minLength) {
            return [];
        }

        var result = await SearchCells(
            { name: inputValue, noInIds: noInIds },
            { page: 1, sortOrder: 'asc' },
            { sortBy: 'lead.name', sortOrder: 'asc' })

        if (!result.isValid) return [];

        return result.data.map((cell) => ({
            label: cell.lead?.name,
            value: cell.id
        }));
    }

    const handleSelectChangeEvent = (option) => {
        setOption(option);
        if (setValue != null)
            setValue(option?.value);
    }

    const randomKey = (Math.random() * 100).toFixed(0).toString();

    return (
        <InputGroup className="mb-3" as={Col}>
            <InputGroup.Text id="basic-addon1">{label}</InputGroup.Text>
            <AsyncSelect
                name={"cell" + randomKey} aria-describedby="basic-addon1" className="async-select"
                onChange={handleSelectChangeEvent}
                noOptionsMessage={getNoOptionsMessage}
                value={option}
                defaultOptions={[{ label: null, value: null }]}
                placeholder="" loadOptions={loadCells}>

            </AsyncSelect>
        </InputGroup>
    )
}

export default CellPicker;