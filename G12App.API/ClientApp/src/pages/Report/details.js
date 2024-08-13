import React, { useEffect, useState } from 'react';
import { InputGroup, Container, Row, Button, Col, Form, Spinner } from 'react-bootstrap';
import { GetCell } from "../../services/Cell.service";
import MemberPicker from '../../components/MemberPicker';
import CellPicker from '../../components/CellPicker';
import { LogReport } from "../../services/Report.service";
import { useNavigate } from 'react-router-dom';
import { FromBRDate } from '../../shared/utils';
import { useToastContext } from '../../contexts/ToastContext';
import { StringifyResponseErrors } from '../../shared/utils';

import './style.css';
import InputMask from "react-input-mask";

export const ReportDetails = () => {

    const toastContext = useToastContext();
    const navigate = useNavigate();

    const emptyReport = {
        subject: '',
        visitors: '',
        frequenters: [],
        cellId: null,
        dateOnly: null,
        timeOnly: null

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
    const [loading, setLoading] = useState(false);
    const [cell, setCell] = useState(emptyCell);
    const [report, setReport] = useState(emptyReport);
    window.setLoading = setLoading;

    const fetchCell = async (cellId) => {
        var result = await GetCell(cellId);
        if (result.isValid) {
            setCell(result.data);
            setReport({
                ...report,
                cellId: result.data.id,
                hostId: result.data.hostId,
                timothyId: result.data.timothyId,
                leadershipId: result.data.leadId
            });

        }
    };

    const SubmitReport = async () => {
        setLoading(true);

        try {
            let reportToSend = JSON.parse(JSON.stringify(report));
            delete reportToSend.dateOnly;
            delete reportToSend.timeOnly;

            var result = await LogReport({ ...reportToSend, date: FromBRDate(report.dateOnly) + "T" + report.timeOnly + ":00" });

            if (result.isValid) {

                toastContext.addToast({
                    title: 'Sucesso', hideDelay: 5000,
                    description: 'Relatório enviado com sucesso!'
                });

                navigate('/cells/' + report.cellId);

            } else {
                
                toastContext.addToast({
                    title: 'Erro', hideDelay: 10000,
                    description: StringifyResponseErrors(result.errors)
                });

                setLoading(false);
            }
            
        }
        catch (e) {
            console.error(e);
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Erro de sistema ao enviar relatório.'
            });
            setLoading(false);
        }
    };

    const handleSelectChangeEvent = (name, value) => {
        if (name === 'cellId')
            fetchCell(value);
        if (name === 'members')
            setReport({ ...report, frequenters: value });
    };


    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setReport({ ...report, [name]: value });
    };

    const translateCellType = (cellType) => {
        if (cellType === "Evangelistic")
            return "Evangelística";
        if (cellType === "Discipleship")
            return "Discipuladora"
    };

    return (
        loading ?
            (<Spinner animation="border" />) : (
                <>
                    <h1>Registrar relatório</h1>
                    <hr />
                    <>
                        <Form>
                            <Container fluid>
                                <h6>Selecione a célula</h6>

                                <Row className="mb-3 pr-0">
                                    <Col>
                                        <CellPicker label="Célula:"
                                            setValue={(value) => handleSelectChangeEvent('cellId', value)} />
                                    </Col>
                                </Row>


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
                                    <Col>
                                        <InputGroup.Text id="basic-addon1">Líder G12: {cell.lead?.computedG12Leader}</InputGroup.Text>
                                    </Col>

                                    <Col>
                                        <InputGroup.Text id="basic-addon1">Modalidade: {translateCellType(cell.cellType)}</InputGroup.Text>
                                    </Col>
                                </Row>

                                <Row className="mb-3 pr-0" as={Col}>
                                    <Col xs={7}>
                                        <InputGroup.Text id="basic-addon1">Rua: {cell.place?.street}</InputGroup.Text>
                                    </Col>

                                    <Col xs={2}>
                                        <InputGroup.Text id="basic-addon1">Número: {cell.place?.number}</InputGroup.Text>
                                    </Col>

                                    <Col>
                                        <InputGroup.Text id="basic-addon1">Bairro: {cell.place?.neighborhood}</InputGroup.Text>
                                    </Col>
                                </Row>

                                <Row>
                                    <h6>Dados do Relatório</h6>
                                </Row>

                                <Row>
                                    <Col>
                                        <InputGroup className="mb-3 pr-0" as={Col}>
                                            <InputGroup.Text id="basic-addon2">Data:</InputGroup.Text>
                                            <InputMask mask="99/99/9999" value={report.dateOnly} onChange={handleChangeEvent}>
                                                <Form.Control type="text" name="dateOnly"
                                                    aria-describedby="basic-addon2" />
                                            </InputMask>
                                        </InputGroup>
                                    </Col>

                                    <Col>
                                        <InputGroup className="mb-3 pr-0" as={Col}>
                                            <InputGroup.Text id="basic-addon2">Horário:</InputGroup.Text>
                                            <InputMask mask="99:99" value={report.timeOnly} onChange={handleChangeEvent}>
                                                <Form.Control type="text" name="timeOnly"
                                                    aria-describedby="basic-addon2" />
                                            </InputMask>
                                        </InputGroup>
                                    </Col>
                                </Row>

                                <Row className="mb-3 pr-0">
                                    <InputGroup className="mb-3" as={Col}>
                                        <InputGroup.Text id="basic-addon3">Tema:</InputGroup.Text>
                                        <Form.Control id="basic-url" name="subject" value={report.subject} aria-describedby="basic-addon3" autoComplete='off' onChange={handleChangeEvent} required="true" />
                                    </InputGroup>
                                </Row>

                                <Row className="mb-3 pr-0">
                                    <Col>
                                        <MemberPicker label="Participantes" isMulti={true}
                                            setValue={(value) => handleSelectChangeEvent('members', value)} 
                                            value={report.frequenters}/>
                                    </Col>
                                </Row>
                                <Row>

                                    <Col>
                                        <InputGroup className="mb-3" as={Col}>
                                            <InputGroup.Text id="basic-addon3">Visitantes:</InputGroup.Text>
                                            <Form.Control id="basic-url" name="visitors" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleChangeEvent} />
                                        </InputGroup>
                                    </Col>
                                </Row>
                            </Container>

                            <Container fluid>
                                <Button variant="primary" className="ms-3" type="submit" onClick={SubmitReport}>
                                    Enviar relatório
                                </Button>
                            </Container>
                        </Form>
                    </>
                </>
            )
    );
}