import React, { useEffect } from 'react';
import { InputGroup, Form, Col, Row, Spinner } from 'react-bootstrap';
import { Link } from "react-router-dom";
import { SearchMembers } from '../../services/Member.service';
import { debounce } from 'lodash';
import Grid from '../../components/Grid';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons'

export const Members = () => {

  const [members, setMembers] = 
    React.useState([]);
  const [pagination, setPagination] = 
    React.useState({page: 1, limit: 10});
  const [searchData, setSearchData] = 
    React.useState({name: '', leadershipName: '', email: '', neighborhood: '', city: '', state: ''});
  const [loading, setLoading] = React.useState(false);
  const [total, setTotal] = React.useState(0);
  const [sort, setSort] = React.useState({sortBy: 'ministryLevel', order: false});

  const translateMinistryLevel = (ministryLevel) => {
    switch(ministryLevel){
      case 1: return 'Pastores';
      case 12: return 'G12';
      default: return ministryLevel;
    }
  };

  const getLeadership = (member) => {
    switch(member.ministryLevel){
      case 0: case 1: return '-';
      case 12: return <><b>{member.computedLeadershipName}</b></>;
      case 144: return <><b>{member.computedG12Leader}</b></>;
      default: return <><b>{member.computedG12Leader}</b> / {member.computedLeadershipName}</>;
    }
  }

  const columns = [
    {
      name: '#',
      render: (member) => (member.id.toString() ?? '0').padStart(4, '0')
    },
    {
      name: 'Nome',
      render: (member) => member.name
    },
    {
      name: 'Nível',
      render: (member) => translateMinistryLevel(member.ministryLevel)
    },
    {
      name: 'Ministério',
      render: (member) => getLeadership(member)
    },
    {
      name: 'Bairro',
      render: (member) => (member.homeAddress?.neighborhood ?? '').length ? member.homeAddress?.neighborhood : '-'
    },
    {
      name: 'Cidade',
      render: (member) => (member.homeAddress?.city ?? '').length ? member.homeAddress?.city : '-'
    },
    {
      name: '',
      render: (member) => <Link to={"/members/" + member.id}><FontAwesomeIcon icon={faMagnifyingGlass} /></Link>
    }
  ];

  const search = async () => {
    setLoading(true);

    const result = await SearchMembers(searchData, pagination, sort);
    if(result.isValid){
      setMembers(result.data);
      setTotal(result.totalRecords ?? result.data.length);
    }
    
    setLoading(false);
  };

  const debounceEventTrigger = debounce(field => {
    var currentValue = searchData[field.name] || ''; 
    if(field.value != currentValue)
      setSearchData({...searchData, [field.name]: field.value});
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

        <div className="row">
          <h6>Filtros</h6>
          
          <Row className="mb-3 pr-0">
            <InputGroup className="mb-3" as={Col}>
              <InputGroup.Text id="basic-addon3">Nome</InputGroup.Text>
              <Form.Control id="basic-url" name="name" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>

            <InputGroup className="mb-3 pr-0" as={Col}>
              <InputGroup.Text id="basic-addon3">Liderança</InputGroup.Text>
              <Form.Control id="basic-url" name="name" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>
          </Row>
          
          <Row className="mb-3 pr-0">
            <InputGroup className="mb-3" as={Col}>
              <InputGroup.Text id="basic-addon3">Bairro</InputGroup.Text>
              <Form.Control id="basic-url" name="neighborhood" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>

            <InputGroup className="mb-3 pr-0" as={Col}>
              <InputGroup.Text id="basic-addon3">Cidade</InputGroup.Text>
              <Form.Control id="basic-url" name="city" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleSearchEvent} />
            </InputGroup>
          </Row>
        </div>
        
        <div className="row">
          <h5>&nbsp;</h5>
          <Grid 
            columns={columns} 
            data={members} 
            textTotal='pessoas cadastradas'  
            loading={loading} 
            pagination={pagination}
            setPagination={setPagination} 
            totalRecords={total}/>
          </div>
      </div>
    </>
  );
  
}
