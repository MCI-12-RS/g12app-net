import React, { useEffect } from 'react';
import { InputGroup, Form, Col, Row, Spinner, Accordion } from 'react-bootstrap';
import { Link } from "react-router-dom";
import { SearchMembers } from '../../services/Member.service';
import { debounce } from 'lodash';
import Grid from '../../components/Grid';
import { isMobile } from 'react-device-detect';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons'
import { RenderGridAddress, TranslateMinistryLevel } from '../../shared/utils';

export const Members = () => {

  const [members, setMembers] =
    React.useState([]);
  const [pagination, setPagination] =
    React.useState({ page: 1, limit: 10 });
  const [searchData, setSearchData] =
    React.useState({ name: '', leadershipName: '', email: '', neighborhood: '', city: '', state: '' });
  const [loading, setLoading] = React.useState(false);
  const [total, setTotal] = React.useState(0);
  const [sort, setSort] = React.useState({ sortBy: 'ministryLevel', order: false });

  const getLeadership = (member) => {
    switch (member.ministryLevel) {
      case 0: case 1: return '-';
      case 12: return <><b>{member.computedLeadershipName}</b></>;
      case 144: return <><b>{member.computedG12Leader}</b></>;
      default: return <><b>{member.computedG12Leader}</b> / {member.computedLeadershipName}</>;
    }
  }

  const columns = [
    {
      name: '#',
      properties: { md: 1 },
      className: 'text-left',
      render: (member) => (member.id.toString() ?? '0').padStart(4, '0')
    },
    {
      name: 'Nome',
      properties: { sm: 3 },
      render: (member) => member.name
    },
    {
      name: 'Nível',
      render: (member) => TranslateMinistryLevel(member.ministryLevel)
    },
    {
      name: 'Liderança',
      properties: { sm: 2 },
      render: (member) => getLeadership(member)
    },
    {
      name: 'Ministério',
      render: (member) => <span>
        <span className="text-nowrap"><strong>{member.totalDisciples || 0}</strong> Discípulos</span><br />
        <span className="text-nowrap"><strong>{member.totalCells || 0}</strong> Células</span>
      </span>
    },
    {
      name: 'Endereço',
      properties: { sm: 2 },
      render: (member) => RenderGridAddress(member.homeAddress)
    },
    {
      name: '',
      className: 'text-right actions',
      render: (member) => <Link to={"/members/" + member.id}><FontAwesomeIcon icon={faMagnifyingGlass} /></Link>
    }
  ];

  const search = async () => {
    setLoading(true);

    const result = await SearchMembers(searchData, pagination, sort);
    if (result.isValid) {
      setMembers(result.data);
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
  }, [pagination, searchData, sort]);

  return (
    <>
      <div>
        <h1>Pessoas</h1>
        <hr />


        <Accordion className="mb-3" { ... !isMobile && ({defaultActiveKey:['0'], alwaysOpen: true})}>
          <Accordion.Item eventKey="0">
            <Accordion.Header>
              Filtros
            </Accordion.Header>

            <Accordion.Body>
              <Row className="mb-3 pr-0">
                <Form.Group className="mb-3" as={Col}>
                  <Form.Label>Nome</Form.Label>
                  <Form.Control id="basic-url" name="name" autoComplete='off' onKeyUp={handleSearchEvent} />
                </Form.Group>

                <Form.Group className="mb-3 pr-0" as={Col}>
                  <Form.Label id="basic-addon3">Liderança</Form.Label>
                  <Form.Control id="basic-url" name="leadershipName" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
                </Form.Group>
              </Row>

              <Row className="mb-3 pr-0">
                <Form.Group className="mb-3" as={Col}>
                  <Form.Label id="basic-addon3">Bairro</Form.Label>
                  <Form.Control id="basic-url" name="neighborhood" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
                </Form.Group>

                <Form.Group className="mb-3 pr-0" as={Col}>
                  <Form.Label id="basic-addon3">Cidade</Form.Label>
                  <Form.Control id="basic-url" name="city" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
                </Form.Group>
              </Row>
            </Accordion.Body>
          </Accordion.Item>
        </Accordion>

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
