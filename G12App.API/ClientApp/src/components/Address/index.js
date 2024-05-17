import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Container } from 'react-bootstrap';
import states from './states.json'

export const AddressDetails = ({
    address = {},
    setAddress = () => {}
}) => {

    const [value, setValue] = useState({
        street: '',
        number: '',
        neighborhood: '',
        city: '',
        state: '',
        postalCode: '',
        notes: '',
        ...address        
    });

    const handleChangeEvent = (event) => {
        setValue({ ...value, [event.target.name]: event.target.value });        
    }

    useEffect(() => {
        setAddress(value);
    }, [value]);
    
    return (
        <>        
            <Container fluid>
                <h6>Endereco</h6>
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={8}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Rua</InputGroup.Text>
                            <Form.Control name="street" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                            value={value.street} />
                        </InputGroup>
                    </Col>

                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Numero</InputGroup.Text>
                            <Form.Control type="text" name="number"
                                aria-describedby="basic-addon2" maxLength={10} value={value.number}
                                onChange={handleChangeEvent} />
                        </InputGroup>
                    </Col>
                </Row>
                
                <Row className="mb-3 pr-0 justify-content-md-center">
                    <Col xs={4}>
                        <InputGroup className="mb-3" as={Col}>
                            <InputGroup.Text id="basic-addon1">Bairro</InputGroup.Text>
                            <Form.Control name="neighborhood" aria-describedby="basic-addon1" onChange={handleChangeEvent} 
                                value={value.neighborhood} />
                        </InputGroup>
                    </Col>

                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Cidade</InputGroup.Text>
                            <Form.Control type="text" name="city" 
                                aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={value.city} />
                        </InputGroup>
                    </Col>
                    
                    <Col xs={4}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Estado</InputGroup.Text>
                            <Form.Select name="state" defaultValue={value.state} aria-describedby="basic-addon2" onChange={handleChangeEvent}>
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
                                value={value.postalCode} />
                        </InputGroup>
                    </Col>

                    <Col xs={8}>
                        <InputGroup className="mb-3 pr-0" as={Col}>
                            <InputGroup.Text id="basic-addon2">Referencia</InputGroup.Text>
                            <Form.Control type="text" pattern="\d{2}\/\d{2}\/\d{2}" name="notes" 
                                aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                value={value.notes} />
                        </InputGroup>
                    </Col>
                </Row>
            </Container>
        </>
    )
}