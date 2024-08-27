import React, { useEffect } from 'react';
import { Form, Col, Row, Accordion, Container } from 'react-bootstrap';
import { Link } from "react-router-dom";
import { SearchCells } from '../../services/Cell.service.js';
import { debounce } from 'lodash';
import Grid from '../../components/Grid';
import { isMobile } from 'react-device-detect';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faList, faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import { FormatFirstLastName, RenderGridAddress, TranslateCellType, TranslateMinistryLevel, TranslateWeekDay, CleanSearchForm } from '../../shared/utils.js';

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
      name: '#',
      properties: { md: 1 },
      className: "text-left d-none d-md-block",
      render: (cell) => (cell.id.toString() ?? '0').padStart(4, '0')
    },
    {
      name: 'Liderança',
      properties: { sm: 3 },
      render: (cell) => <span>
        <span><b className="d-none d-md-inline">Líder: </b>{FormatFirstLastName(cell.lead?.name)} <small>({TranslateMinistryLevel(cell.lead?.ministryLevel)})</small></span><br />
        {cell.lead?.computedG12Leader && (<span><b className="d-none d-md-inline">G12: </b>{FormatFirstLastName(cell.lead?.computedG12Leader)}</span>)}
      </span>
    },
    {
      name: 'Localização',
      properties: { sm: 3 },
      render: (cell) => RenderGridAddress(cell.place)
    },
    {
      name: 'Modalidade',
      render: (cell) => TranslateCellType(cell.cellType)
    },
    {
      name: 'Dia da semana',
      render: (cell) => <> {TranslateWeekDay(cell.weekDay)} <br /> {(cell.time || '').substring(0, 5)} </>
    },
    {
      name: '',
      className: 'text-right actions',
      render: (cell) => 
        <>
          <Link to={"/cells/" + cell.id} title="Dados da célula"><FontAwesomeIcon icon={faMagnifyingGlass} /></Link>
          <Link to={"/cells/" + cell.id + "/reports"} title="Ver relatórios"><FontAwesomeIcon icon={faList} /></Link>
        </>
    }

  ];

  const search = async () => {
    setLoading(true);

    const result = await SearchCells(searchData, pagination, {});
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
    setPagination({ ...pagination, page: 1 });
  }, [searchData]);

  useEffect(() => {
    search();
  }, [pagination]);

  return (
    <>
      <div>
        <h1>Células</h1>
        <hr />

        <Container fluid className="filter-box">

          <Accordion className="mb-3" {... !isMobile && ({ defaultActiveKey: ['0'], alwaysOpen: true })}>
            <Accordion.Item eventKey="0">
              <Accordion.Header>
                Filtros
              </Accordion.Header>

              <Accordion.Body>

                <Row className="clean-form">
                <Link onClick={() => CleanSearchForm(setSearchData)}>Limpar filtros</Link>
                </Row>

                <Row className="pr-3">
                  <Form.Group className="mb-3" as={Col}>
                    <Form.Label>Liderança</Form.Label>
                    <Form.Control id="basic-url" name="lead" autoComplete='off' onKeyUp={handleSearchEvent} />
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label>G12</Form.Label>
                    <Form.Control id="basic-url" name="leadership" autoComplete='off' onKeyUp={handleSearchEvent} />
                  </Form.Group>
                </Row>

                <Row className=" pr-3">
                  <Form.Group className="mb-3" as={Col}>
                    <Form.Label>Rua</Form.Label>
                    <Form.Control id="basic-url" name="street" autoComplete='off' onKeyUp={handleSearchEvent} />
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label>Bairro</Form.Label>
                    <Form.Control id="basic-url" name="neighborhood" autoComplete='off' onKeyUp={handleSearchEvent} />
                  </Form.Group>
                </Row>

                <Row className="pr-3">
                  <Form.Group className="mb-3" as={Col}>
                    <Form.Label>Cidade</Form.Label>
                    <Form.Control id="basic-url" name="city" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label>Modalidade</Form.Label>
                    <Form.Select name="cellType" aria-describedby="basic-addon2" onChange={handleSearchEvent}>
                      <option value="">Todas</option>
                      <option value="Evangelistic">Evangelística</option>
                      <option value="Discipleship">Discipuladora</option>
                      <option value="Macrocell">Macrocélula</option>
                    </Form.Select>
                  </Form.Group >

                  <Form.Group className="mb-3 pr-3" xs={12} md={4} as={Col}>
                    <Form.Label>Dia da Semana</Form.Label>
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
                  </Form.Group>
                </Row>
              </Accordion.Body>
            </Accordion.Item>
          </Accordion>

        </Container>

        <div className="row">
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