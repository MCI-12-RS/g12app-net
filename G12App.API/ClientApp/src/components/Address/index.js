import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Container } from 'react-bootstrap';
import states from './states.json'

export const GetEmptyAddress = () => ({
    street: '',
    number: '',
    neighborhood: '',
    city: '',
    state: '',
    postalCode: '',
    notes: ''
});

export const AddressDetails = ({
    address = {},
    setAddress = () => {}
}) => {

    const handleChangeEvent = (event) => {
        setAddress({ ...address, [event.target.name]: event.target.value });        
    }
    
    return (
        <>        
            <Container fluid>
                <h6>Endereco</h6>
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={8}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Rua</InputGroup.Text>
                            <Form.Control name="street" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                            value={address.street} />
                        </InputGroup>
                    </Col>

                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Numero</InputGroup.Text>
                            <Form.Control type="text" name="number"
                                aria-describedby="basic-addon2" maxLength={10} value={address.number}
                                onChange={handleChangeEvent} />
                        </InputGroup>
                    </Col>
                </Row>
                
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={4}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Bairro</InputGroup.Text>
                            <Form.Control name="neighborhood" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                                value={address.neighborhood} />
                        </InputGroup>
                    </Col>

                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Cidade</InputGroup.Text>
                            <Form.Control type="text" name="city" 
                                aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={address.city} />
                        </InputGroup>
                    </Col>
                    
                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Estado</InputGroup.Text>
                            <Form.Select name="state" defaultValue={address.state} aria-describedby="basic-addon2" onChange={handleChangeEvent}>
                                {states.map((state) => (
                                    <option key={state.acronym} value={state.acronym}>{state.name}</option>
                                ))}
                            </Form.Select>
                        </InputGroup>
                    </Col>
                </Row>
                
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={4}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">CEP</InputGroup.Text>
                            <Form.Control name="postalCode" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                                value={address.postalCode} />
                        </InputGroup>
                    </Col>

                    <Col xs={8}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Complemento</InputGroup.Text>
                            <Form.Control type="text" name="notes" aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={address.notes} />
                        </InputGroup>
                    </Col>
                </Row>
            </Container>
        </>
    )
}