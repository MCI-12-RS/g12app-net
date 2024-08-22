import React, { useState, useEffect } from "react";
import { Form, Col, Row, Container } from 'react-bootstrap';
import states from './states.json'

export const GetEmptyAddress = () => ({
    street: '',
    number: '',
    neighborhood: '',
    city: '',
    state: 'RS',
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
                        <Form.Label className="mb-3" as={Col}>
                            <Form.Group  id="basic-addon1">Logradouro</Form.Group >
                            <Form.Control name="street" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                            value={address.street} />
                        </Form.Label>
                    </Col>

                    <Col xs={4}>
                        <Form.Label className="mb-3 pr-0" as={Col}>
                            <Form.Group  id="basic-addon2">Número</Form.Group >
                            <Form.Control type="text" name="number"
                                aria-describedby="basic-addon2" maxLength={10} value={address.number}
                                onChange={handleChangeEvent} />
                        </Form.Label>
                    </Col>
                </Row>
                
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={4}>
                        <Form.Label className="mb-3" as={Col}>
                            <Form.Group  id="basic-addon1">CEP</Form.Group >
                            <Form.Control name="postalCode" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                                value={address.postalCode} />
                        </Form.Label>
                    </Col>

                    <Col xs={8}>
                        <Form.Label className="mb-3 pr-0" as={Col}>
                            <Form.Group  id="basic-addon2">Complemento</Form.Group >
                            <Form.Control type="text" name="notes" aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={address.notes} />
                        </Form.Label>
                    </Col>
                </Row>
                
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={12} md={4}>
                        <Form.Label className="mb-3" as={Col}>
                            <Form.Group  id="basic-addon1">Bairro</Form.Group >
                            <Form.Control name="neighborhood" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                                value={address.neighborhood} />
                        </Form.Label>
                    </Col>

                    <Col xs={12} md={4}>
                        <Form.Label className="mb-3 pr-0" as={Col}>
                            <Form.Group  id="basic-addon2">Cidade</Form.Group >
                            <Form.Control type="text" name="city" 
                                aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={address.city} />
                        </Form.Label>
                    </Col>
                    
                    <Col xs={12} md={4}>
                        <Form.Label className="mb-3 pr-0" as={Col}>
                            <Form.Group  id="basic-addon2">Estado</Form.Group >
                            <Form.Select name="state" defaultValue={address.state} aria-describedby="basic-addon2" onChange={handleChangeEvent}>
                                {states.map((state) => (
                                    <option key={state.acronym} value={state.acronym}>{state.name}</option>
                                ))}
                            </Form.Select>
                        </Form.Label>
                    </Col>
                </Row>
            </Container>
        </>
    )
}