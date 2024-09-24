import React, { useEffect } from 'react';
import { Form, Col, Row, Accordion, Container } from 'react-bootstrap';
import { Link, useLocation } from "react-router-dom";
import { SearchMembers } from '../../services/Member.service';
import { debounce } from 'lodash';
import Grid from '../../components/Grid';
import { isMobile } from 'react-device-detect';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons'
import { FormatFirstLastName, RenderGridAddress, TranslateMinistryLevel, CleanSearchForm, FromBRDate } from '../../shared/utils';
import ReactInputMask from 'react-input-mask';

export const Members = () => {

  const { state } = useLocation();

  const [initialSearch, setInitialSearch] = React.useState(true);

  const [members, setMembers] =
    React.useState([]);
  const [pagination, setPagination] =
    React.useState({ page: 1, limit: 10 });
  const [searchData, setSearchData] =
    React.useState({
      name: '',
      leadershipName: '',
      neighborhood: '',
      city: '',
      idle: state?.idle,
      hasNoLeadership: state?.hasNoLeadership,
      baptized: state?.baptized,
      ministryLevel: '',
      cellLink: state?.cellLink,
      createdDateFrom: state?.createdDateFrom || null,
      createdDateUntil: null
    });
  const [loading, setLoading] = React.useState(false);
  const [total, setTotal] = React.useState(0);
  const [sort] = React.useState({ sortBy: 'ministryLevel', order: false });

  const getLeadership = (member) => {
    switch (member.ministryLevel) {
      case 0: case 1: case 9999999: return (member.hasNoLeadership ? 'Sem liderança' : '-');
      case 12: return <span className="text-nowrap"><b>{FormatFirstLastName(member.computedLeadershipName)}</b></span>;
      case 144: return <span className="text-nowrap"><b>{FormatFirstLastName(member.computedG12Leader)}</b></span>;
      default: return <span className="text-nowrap"><b>{FormatFirstLastName(member.computedG12Leader)}</b> / {FormatFirstLastName(member.computedLeadershipName)}</span>;
    }
  }

  const columns = [
    {
      name: '#',
      properties: { sm: 1 },
      className: "text-left d-none d-md-block",
      render: (member) => (member.id.toString() ?? '0').padStart(4, '0')
    },
    {
      name: 'Nome',
      properties: { sm: 2 },
      render: (member) => member.name
    },
    {
      name: 'Nível',
      properties: { sm: 1 },
      render: (member) => member.hasNoLeadership ? '-' : TranslateMinistryLevel(member.ministryLevel)
    },
    {
      name: 'Liderança',
      properties: { sm: 3 },
      classFunction: (member) => member.ministryLevel === 0 || member.ministryLevel === 1 ? 'd-none' : '',
      render: (member) => getLeadership(member)
    },
    {
      name: 'Ministério',
      properties: { sm: 2 },
      render: (member) => <span>
        <span className="text-nowrap"><strong>{member.totalDisciples || 0}</strong> Discípulos</span><br />
        <span className="text-nowrap"><strong>{member.totalCells || 0}</strong> Células</span>
      </span>
    },
    {
      name: 'Endereço',
      properties: { sm: 2 },
      className: "d-none d-md-block",
      render: (member) => RenderGridAddress(member.homeAddress)
    },
    {
      name: '',
      properties: { sm: 1 },
      className: 'text-right actions',
      render: (member) => <span>
        <Link to={"/members/" + member.id}><FontAwesomeIcon icon={faMagnifyingGlass} /></Link>
      </span>
    }
  ];

  const search = async () => {
    setLoading(true);

    let payload = {
      ...searchData,
      createdDateFrom: searchData.createdDateFrom ? FromBRDate(searchData.createdDateFrom) : null,
      createdDateUntil: searchData.createdDateUntil ? FromBRDate(searchData.createdDateUntil) : null
    }

    const result = await SearchMembers(payload, pagination, sort);
    if (result.isValid) {
      setMembers(result.data);
      setTotal(result.totalRecords ?? result.data.length);
    }

    setInitialSearch(false);
    setLoading(false);
  };

  const debounceSearch = debounce(() => {
    setPagination({ ...pagination, page: 1 });
  }, 500);

  const handleSearchEvent = (event) => {
    var field = event.target;
    var currentValue = searchData[field.name] || '';
    if (field.value !== currentValue)
      setSearchData({ ...searchData, [field.name]: field.value });
  };

  useEffect(() => {
    if (!initialSearch)
      debounceSearch();
  }, [searchData]);

  useEffect(() => {
    search();
  }, [pagination]);

  return (
    <>
      <div>
        <h1>Pessoas</h1>
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
                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label>Nome</Form.Label>
                    <Form.Control id="basic-url" name="name" value={searchData.name} autoComplete='off' onChange={handleSearchEvent} />
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label id="basic-addon3">Liderança</Form.Label>
                    <Form.Control id="basic-url" name="leadershipName" value={searchData.leadershipName} aria-describedby="basic-addon3" autoComplete='off' onChange={handleSearchEvent} />
                  </Form.Group>
                </Row>

                <Row className="pr-3">
                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label id="basic-addon3">Bairro</Form.Label>
                    <Form.Control id="basic-url" name="neighborhood" value={searchData.neighborhood} aria-describedby="basic-addon3" autoComplete='off' onChange={handleSearchEvent} />
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label id="basic-addon3">Cidade</Form.Label>
                    <Form.Control id="basic-url" name="city" value={searchData.city} aria-describedby="basic-addon3" autoComplete='off' onChange={handleSearchEvent} />
                  </Form.Group>
                </Row>

                <Row className="pr-3">
                  <Form.Group className="mb-3 pr-3" xs={12} md={2} as={Col}>
                    <Form.Label>Situação</Form.Label>
                    <Form.Select name="idle" aria-describedby="basic-addon2" value={searchData.idle} onChange={handleSearchEvent}>
                      <option value="">Indiferente</option>
                      <option value="false">Ativo</option>
                      <option value="true">Inativo</option>
                    </Form.Select>
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" xs={12} md={2} as={Col}>
                    <Form.Label>Possui Liderança</Form.Label>
                    <Form.Select name="hasNoLeadership" aria-describedby="basic-addon2" value={searchData.hasNoLeadership} onChange={handleSearchEvent}>
                      <option value="">Indiferente</option>
                      <option value="false">Sim</option>
                      <option value="true">Não</option>
                    </Form.Select>
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" xs={12} md={2} as={Col}>
                    <Form.Label>Batizado</Form.Label>
                    <Form.Select name="baptized" aria-describedby="basic-addon2" value={searchData.baptized} onChange={handleSearchEvent}>
                      <option value="">Indiferente</option>
                      <option value="true">Sim</option>
                      <option value="false">Não</option>
                    </Form.Select>
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" xs={12} md={3} as={Col}>
                    <Form.Label>Nível Ministerial</Form.Label>
                    <Form.Select name="ministryLevel" aria-describedby="basic-addon2" value={searchData.ministryLevel} onChange={handleSearchEvent}>
                      <option value="">Indiferente</option>
                      <option value="1">Pastores</option>
                      <option value="12">G12</option>
                      <option value="144">144</option>
                      <option value="1728">1728</option>
                      <option value="20736">20736</option>
                    </Form.Select>
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" xs={12} md={3} as={Col}>
                    <Form.Label>Vínculo Celular</Form.Label>
                    <Form.Select name="cellLink" aria-describedby="basic-addon2" value={searchData.cellLink} onChange={handleSearchEvent}>
                      <option value="">Indiferente</option>
                      <option value="leader">Líder</option>
                      <option value="timothy">Timóteo</option>
                      <option value="host">Anfitrião</option>
                      <option value="attendee">Participante</option>
                      <option value="none">Sem vínculo</option>
                    </Form.Select>
                  </Form.Group>
                </Row>

                <Row className="pr-3">
                  <Form.Group className="mb-3" as={Col}>
                    <Form.Label>Cadastrado a partir de</Form.Label>
                    <ReactInputMask mask="99/99/9999" onChange={handleSearchEvent} defaultValue={searchData.createdDateFrom}>
                      <Form.Control id="basic-url" name="createdDateFrom" autoComplete='off' />
                    </ReactInputMask>
                  </Form.Group>

                  <Form.Group className="mb-3 pr-3" as={Col}>
                    <Form.Label>Cadastrado até</Form.Label>
                    <ReactInputMask mask="99/99/9999" onChange={handleSearchEvent} defaultValue={searchData.createdDateUntil}>
                      <Form.Control id="basic-url" name="createdDateUntil" autoComplete='off' />
                    </ReactInputMask>
                  </Form.Group>
                </Row>

              </Accordion.Body>
            </Accordion.Item>
          </Accordion>
        </Container>

        <div className="row">
          <Grid
            columns={columns}
            data={members}
            textTotal='pessoas cadastradas'
            loading={loading}
            pagination={pagination}
            setPagination={setPagination}
            totalRecords={total} />
        </div>
      </div>
    </>
  );

}
