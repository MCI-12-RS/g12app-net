import React, { useEffect } from 'react';
import { InputGroup, Form, Col, Row, Button } from 'react-bootstrap';
import { Link } from "react-router-dom";
import { SearchCells } from '../../services/Cell.service.js';
import { debounce } from 'lodash';
import Grid from '../../components/Grid';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';

export const Cells = () => {

  const [cells, setCells] =
    React.useState([]);
  const [pagination, setPagination] =
    React.useState({ page: 1, limit: 20 });
  const [searchData, setSearchData] =
    React.useState({
      lead: '', leadership: '', host: '', timothy: '',
      street: '', cellType: null, weekDay: null, neighborhood: '', city: '', state: ''
    });
  const [loading, setLoading] = React.useState(false);
  const [total, setTotal] = React.useState(0);

  const columns = [
    {
      name: 'Liderança',
      render: (cell) => <>
        <span><b>Líder: </b>{cell.lead?.name}</span><br />
        <span><b>G12: </b>{cell.lead?.computedG12Leader}</span>
      </>
    },
    {
      name: 'Localização',
      render: (cell) => <>
        <span><b>Rua: </b>{cell.place?.street}</span><br />
        <span><b>Bairro: </b>{cell.place?.neighborhood}</span><br />
        <span><b>Cidade: </b>{cell.place?.city}</span>
      </>
    },
    {
      name: 'Modalidade',
      render: (cell) => translateCellType(cell.cellType)
    },
    {
      name: 'Dia da semana',
      render: (cell) => <> {translateWeekDay(cell.weekDay)} <br /> {(cell.time || '').substring(0, 5)} </>
    },
    {
      name: '',
      render: (cell) => <Link to={"/cells/" + cell.id}><FontAwesomeIcon icon={faMagnifyingGlass} /></Link>
    }

  ];

  const translateCellType = (cellType) => {
    if (cellType === "Evangelistic")
      return "Evangelística";
    if (cellType === "Discipleship")
      return "Discipuladora"
  };

  const translateWeekDay = (weekDay) => {
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

  const search = async () => {
    setLoading(true);

    const result = await SearchCells(searchData, pagination);
    if (result.isValid) {
      setCells(result.data);
      setTotal(result.totalRecords ?? result.data.length);
    }

    setLoading(false);
  };

  const debounceEventTrigger = debounce(field => {
    var currentValue = searchData[field.name] || '';
    if (field.value != currentValue)
      setSearchData({ ...searchData, [field.name]: field.value });
  }, 500);

  const handleSearchEvent = (event) => {
    var field = event.target;
    debounceEventTrigger(field);
  };

  useEffect(() => {
    search();
  }, [pagination, searchData]);

  return (
    <>
      <div>
        <h1>Células</h1>
        <hr />

        <div className="mb-3" as={Col}>
          <h6>Filtros</h6>

          <Row className="mb-3 pr-0">
            <InputGroup className="mb-3" as={Col}>
              <InputGroup.Text id="basic-addon3">Liderança</InputGroup.Text>
              <Form.Control id="basic-url" name="lead" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>

            <InputGroup className="mb-3 pr-0" as={Col}>
              <InputGroup.Text id="basic-addon3">G12</InputGroup.Text>
              <Form.Control id="basic-url" name="leadership" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>
          </Row>

          <Row className="mb-3 pr-0">
            <InputGroup className="mb-3" as={Col}>
              <InputGroup.Text id="basic-addon3">Rua</InputGroup.Text>
              <Form.Control id="basic-url" name="street" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>

            <InputGroup className="mb-3 pr-0" as={Col}>
              <InputGroup.Text id="basic-addon3">Bairro</InputGroup.Text>
              <Form.Control id="basic-url" name="neighborhood" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>
          </Row>

          <Row className="mb-3 pr-0">
            <InputGroup className="mb-3" as={Col}>
              <InputGroup.Text id="basic-addon3">Cidade</InputGroup.Text>
              <Form.Control id="basic-url" name="city" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>

            <InputGroup className="mb-3 pr-0" as={Col}>
              <InputGroup.Text id="basic-addon2">Modalidade</InputGroup.Text>
              <Form.Select name="cellType" aria-describedby="basic-addon2" onChange={handleSearchEvent}>
                <option value="">Todas</option>
                <option value="Evangelistic">Evangelística</option>
                <option value="Discipleship">Discipuladora</option>
                <option value="Macrocell">Macrocélula</option>
              </Form.Select>
            </InputGroup>

            <InputGroup className="mb-3 pr-0" as={Col}>
              <InputGroup.Text id="basic-addon2">Dia da Semana</InputGroup.Text>
              <Form.Select name="weekDay" aria-describedby="basic-addon2" onChange={handleSearchEvent}>
                <option value="">Selecione o dia</option>
                <option value="Monday">Segunda-feira</option>
                <option value="Tuesday">Terça-feira</option>
                <option value="Wednesday">Quarta-feira</option>
                <option value="Thursday">Quinta-feira</option>
                <option value="Friday">Sexta-feira</option>
                <option value="Saturday">Sábado</option>
                <option value="Sunday">Domingo</option>
              </Form.Select>
            </InputGroup>
          </Row>
        </div>

        <div className="row">
          <h6>&nbsp;</h6>
          <Grid
            columns={columns}
            data={cells}
            textTotal='células cadastradas'
            loading={loading}
            pagination={pagination}
            setPagination={setPagination}
            totalRecords={total} />
        </div>
      </div>
    </>
  )
}