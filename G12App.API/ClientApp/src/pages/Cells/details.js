import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Button, Container, SelectBox } from 'react-bootstrap';
import { Link, useParams } from "react-router-dom";
import { GetCell, GetMember, SearchMembers } from '../../services/Cell.service';
import AsyncSelect from 'react-select/async'
import { debounce } from 'lodash';
import Grid from '../../components/Grid';
//import test from '../../data/states.csv';

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons'

import './style.css';
import states from '../../components/Address/states.json'

export const CellDetails = () => {

    const emptyCell = {
        lead: {
            name: ''
        },
        host: {
            name: ''
        },
        timothy: {
            name: ''
        },
        leadership: {
            name: ''
        },
        place: {
            street: '',
            number: '',
            neighborhood: '',
            city: '',
            state: ''
        }
    };

    const params = useParams();
    const [loading, setLoading] = useState(false);
    const [cell, setCell] = useState(emptyCell);

    useEffect(() => {
        fetchUser();
    }, [params]);

    const fetchUser = async () => {
        setLoading(true);
        if (!params.id) {
            setCell(emptyCell);
            setLoading(false);
            return;
        }

        var result = await GetCell(params.id);
        if (result.isValid) {
            setCell(result.data)
        }
        setLoading(false);
    };

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setCell({ ...cell, [name]: value });
    };

    const getNoOptionsMessage = (data) => {
        if (!(data.inputValue?.length)) return "Digite para pesquisar..."
        return "Nenhum resultado encontrado.";
    }

    const handleDateMask = (e) => {
        let input = e.target;

        if (e.charCode < 47 || e.charCode > 57) {
            e.preventDefault();
        }
        var len = input.value.length;

        if (len !== 1 || len !== 3) {
            if (e.charCode == 47) {
                e.preventDefault();
            }
        }

        if (len === 2) {
            input.value += '/';
        }

        if (len === 5) {
            input.value += '/';
        }
        return true;
    };

    return (
        loading ?
            (<div>Carregando...</div>) : (
                <>
                    {(params.id ? <h1>Alterar cadastro de Células</h1> : <h1>Cadastrar nova Célula</h1>)}
                    <hr />
                    <Container fluid>
                        <h6>Participantes</h6>
                        <hr />
                        <Row className="mb-3" as={Col}>
                            <Col>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">Líder</InputGroup.Text>
                                    <Form.Control name="lead" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell.lead?.name} />
                                </InputGroup>
                            </Col>

                            <Col>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">Timóteo</InputGroup.Text>
                                    <Form.Control name="timothy" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell.timothy?.name} />
                                </InputGroup>
                            </Col>

                            <Col>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">Anfitrião</InputGroup.Text>
                                    <Form.Control name="host" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell.host?.name} />
                                </InputGroup>
                            </Col>
                        </Row>
                    </Container>

                    <Container fluid>
                        <h6>Endereço</h6>
                        <hr />
                        <Row className="mb-3" as={Col}>
                            <Col xs={8}>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">Rua</InputGroup.Text>
                                    <Form.Control name="street" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell['street']} />
                                </InputGroup>
                            </Col>

                            <Col xs={2}>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">Número</InputGroup.Text>
                                    <Form.Control name="number" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell['number']} />
                                </InputGroup>
                            </Col>

                            <Col xs={2}>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">CEP</InputGroup.Text>
                                    <Form.Control name="postalcode" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell['postalcode']} />
                                </InputGroup>
                            </Col>
                        </Row>

                        <Row>
                            <Col>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">Bairro</InputGroup.Text>
                                    <Form.Control name="neighborhood" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell['neighborhood']} />
                                </InputGroup>
                            </Col>

                            <Col>
                                <InputGroup className="mb-3" as={Col}>
                                    <InputGroup.Text id="basic-addon1">Cidade</InputGroup.Text>
                                    <Form.Control name="city" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={cell['city']} />
                                </InputGroup>
                            </Col>

                            <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Estado</InputGroup.Text>
                            <Form.Select name="homeAddress.state" aria-describedby="basic-addon2" onChange={handleChangeEvent}>
                                {states.map((state) => (
                                    <option value={state.acronym} selected={cell.place.state == state.acronym}>{state.name}</option>
                                ))}
                            </Form.Select>
                        </InputGroup>
                    </Col>
                        </Row>
                    </Container>
                </>
            )
    )
}