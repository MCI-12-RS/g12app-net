import React, { useEffect } from 'react';
import { InputGroup, Container, Row, Col } from 'react-bootstrap';

import './style.css';
import moment from "moment";
import { AddressDetails } from "../../components/Address";
import InputMask from "react-input-mask";

export const Report = () => {
    
    const emptyReport = {
        subject: '',
        visitors: null,
        frequenters: null,
        cellId: null
    };

    return (
        <>
        <h1>Registrar relatório</h1>
        <>
        <Container fluid>
            <h6>Dados da Célula</h6>
            <Row className="mb-3 pr-0" as={Col}>
                <InputGroup.Text id="basic-addon1">Líder</InputGroup.Text>
            </Row>
        </Container>
        </>
        </>
    )
}