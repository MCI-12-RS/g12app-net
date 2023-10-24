import React, { useEffect, useState } from 'react';
import { InputGroup, Container, Row, Col, Form } from 'react-bootstrap';
import { GetCell } from "../../services/Cell.service";

import './style.css';
import moment from "moment";
import { AddressDetails } from "../../components/Address";
import InputMask from "react-input-mask";
import { useParams } from 'react-router-dom';

export const Report = () => {

    const emptyReport = {
        subject: '',
        visitors: null,
        frequenters: null,
        cellId: null
    };

    const emptyCell = {
        lead: {
            name: ''
        },
        leadId: null,
        host: {
            name: ''
        },
        hostId: null,
        timothy: {
            name: ''
        },
        timothyId: null,
        leadership: {
            name: ''
        },
        weekDay: {
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

    return (
        <>
            <h1>Registrar relatório</h1>
            <>
                <Container fluid>
                    <h6>Dados da Célula</h6>
                    <Row className="mb-3 pr-0" as={Col}>
                        <Col>
                            <InputGroup.Text id="basic-addon1">Líder: {cell.lead?.name}</InputGroup.Text>
                        </Col>

                        <Col>
                            <InputGroup.Text id="basic-addon1">Timóteo: {cell.host?.name}</InputGroup.Text>
                        </Col>

                        <Col>
                            <InputGroup.Text id="basic-addon1">Anfitrião: {cell.host?.name}</InputGroup.Text>
                        </Col>
                    </Row>

                    <Row className="mb-3 pr-0" as={Col}>
                        <InputGroup.Text id="basic-addon1">Endereço: {cell.place?.street}, {cell.place?.number} Número: {cell.place?.number} </InputGroup.Text>
                    </Row>
                </Container>
            </>
        </>
    )
}