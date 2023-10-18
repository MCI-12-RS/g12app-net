import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Button, Container, SelectBox } from 'react-bootstrap';
import { Link, useParams } from "react-router-dom";
import { GetMember, SearchMembers } from '../../services/Member.service';
import AsyncSelect from 'react-select/async'
import { debounce } from 'lodash';
import Grid from '../../components/Grid';
//import test from '../../data/states.csv';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons'

import './style.css';
import states from '../../components/Address/states.json'

export const MemberDetails = () => {   

    const emptyMember = {
        name: '',
        birthDate: '',
        homeAddress: {
            street: '',
            number: '',
            neighborhood: '',
            city: '',
            state: ''
        }
    };

    const params = useParams();
    const [loading, setLoading] = useState(false);
    const [member, setMember] = useState(emptyMember);

    useEffect(() => {
        fetchUser();
    }, [params]);

    const fetchUser = async () => {
        setLoading(true);
        if(!params.id) {
            setMember(emptyMember);
            setLoading(false);
            return;
        }

        var result = await GetMember(params.id);
        if(result.isValid){
            const member = result.data;
            if (member.birthDate){
                var date = new Date(member.birthDate);
                member.birthDate = (date.getDate() + 1).toString().padStart(2, '0') + '/' + 
                                   (date.getMonth() + 1).toString().padStart(2, '0') + '/' +
                                   date.getFullYear();
            }
            setMember(result.data);
        }
        setLoading(false);
    };

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setMember({ ...member, [name]: value });
    };

    const handleSelectChangeEvent = (name, data) => {
        setMember({ ...member, [name]: data.value });
    };

    const loadG12 = async (inputValue) => {
        let result = await SearchMembers({ name: inputValue }, {page: 1, limit: 10});
        if(!result.isValid) return [];

        return result.data.map((member) => ({
            label: member.name,
            value: member.id
        }));
    }

    const getNoOptionsMessage = (data) => {
        if(!(data.inputValue?.length)) return "Digite para pesquisar..."
        return "Nenhum resultado encontrado.";
    }
        
    const handleDateMask = (e) => {
        let input = e.target;
        
        if(e.charCode < 47 || e.charCode > 57) {
            e.preventDefault();
        }

        var len = input.value.length;

        if(len !== 1 || len !== 3) {
            if(e.charCode == 47) {
                e.preventDefault();
            }
        }

        if(len === 2) {
            input.value += '/';
        }

        if(len === 5) {
            input.value += '/';
        }
        return true;
    }; 

    return (
        loading ? 
        (<div>Carregando...</div>) : (
        <>
            {(params.id ? <h1>Alterar cadastro de pessoa</h1> : <h1>Cadastrar nova pessoa</h1>)}
            <hr />
            <Container fluid>
                <h6>Dados pessoais</h6>
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Nome</InputGroup.Text>
                            <Form.Control name="name" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={member['name']} />
                        </InputGroup>
                    </Col>

                    <Col>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Data de Nascimento</InputGroup.Text>
                            <Form.Control type="text" pattern="\d{2}\/\d{2}\/\d{2}" name="birthDate" onKeyPress={handleDateMask} 
                                aria-describedby="basic-addon2" maxLength={10} value={member['birthDate']}  onChange={handleChangeEvent} />
                        </InputGroup>
                    </Col>
                </Row>
            </Container>
                
            <Container fluid>
                <h6>Ministerial</h6>
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Liderança G12</InputGroup.Text>
                            <AsyncSelect 
                                name="leadership" aria-describedby="basic-addon1" className="async-select" 
                                onChange={(data) => handleSelectChangeEvent('leadership', data)} 
                                noOptionsMessage={getNoOptionsMessage}
                                placeholder="" loadOptions={loadG12}>
                            </AsyncSelect>
                        </InputGroup>
                    </Col>

                    <Col>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Lider</InputGroup.Text>
                            <Form.Control name="name" aria-describedby="basic-addon1" onChange={handleChangeEvent} />
                        </InputGroup>
                    </Col>
                </Row>
            </Container>
            
            <Container fluid>
                <h6>Endereco</h6>
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={8}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Rua</InputGroup.Text>
                            <Form.Control name="homeAddress.street" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                            value={member.homeAddress?.street} />
                        </InputGroup>
                    </Col>

                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Numero</InputGroup.Text>
                            <Form.Control type="text" pattern="\d{2}\/\d{2}\/\d{2}" name="homeAddress.number"
                                aria-describedby="basic-addon2" maxLength={10} value={member.homeAddress?.number}
                                onChange={handleChangeEvent} />
                        </InputGroup>
                    </Col>
                </Row>
                
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={4}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Bairro</InputGroup.Text>
                            <Form.Control name="homeAddress.neighborhood" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                                value={member.homeAddress?.neighborhood} />
                        </InputGroup>
                    </Col>

                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Cidade</InputGroup.Text>
                            <Form.Control type="text" pattern="\d{2}\/\d{2}\/\d{2}" name="homeAddress.city" 
                                aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={member.homeAddress?.city} />
                        </InputGroup>
                    </Col>
                    
                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Estado</InputGroup.Text>
                            <Form.Select name="homeAddress.state" aria-describedby="basic-addon2" onChange={handleChangeEvent}>
                                {states.map((state) => (
                                    <option value={state.acronym} selected={member.homeAddress.state == state.acronym}>{state.name}</option>
                                ))}
                            </Form.Select>
                        </InputGroup>
                    </Col>
                </Row>
                
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={4}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">CEP</InputGroup.Text>
                            <Form.Control name="homeAddress.postalCode" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                                value={member.homeAddress?.postalCode} />
                        </InputGroup>
                    </Col>

                    <Col xs={8}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Referencia</InputGroup.Text>
                            <Form.Control type="text" pattern="\d{2}\/\d{2}\/\d{2}" name="homeAddress.notes" 
                                aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={member.homeAddress?.notes} />
                        </InputGroup>
                    </Col>
                </Row>
                
                
                <Row className="mb-3 pr-0 d-flex">
                    
                    <Col xs={12}>
                        {(params.id && <Button variant="danger" type="submit" className="">Deletar cadastro</Button>)}
                        <Button variant="primary" type="submit" className="float-end">Salvar {(params.id ? "alteracoes" : "cadastro")}</Button>
                    </Col>    

                </Row>
            </Container>
        </>
    ));

};