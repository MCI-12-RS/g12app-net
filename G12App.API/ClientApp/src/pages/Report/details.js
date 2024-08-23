import React, { useEffect, useState } from 'react';
import { Container, Row, Button, Col, Form, Spinner, FormCheck, InputGroup } from 'react-bootstrap';
import { GetCell } from "../../services/Cell.service";
import MemberPicker from '../../components/MemberPicker';
import CellPicker from '../../components/CellPicker';
import { LogReport } from "../../services/Report.service";
import { useNavigate } from 'react-router-dom';
import { FromBRDate, MoneyToDecimal, ToBRDate, TranslateMomentWeekDay } from '../../shared/utils';
import { useToastContext } from '../../contexts/ToastContext';
import { StringifyResponseErrors } from '../../shared/utils';

import './style.css';
import InputMask from "react-input-mask";
import { CurrencyInput } from 'react-currency-mask';

export const ReportDetails = () => {

    const toastContext = useToastContext();
    const navigate = useNavigate();

    const emptyReport = {
        subject: '',
        frequenters: [],
        cellId: null,
        dateOnly: null,
        timeOnly: null,
        visitors: [],
        contribution: 'R$ 0,00',
        notes: '',
        weekCellSkipped: false
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
        weekDay: '',
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
    const [errors, setErrors] = useState({});
    window.setLoading = setLoading;

    const getSuggestedDate = (cell) => {
        var date = new Date();
        var day = date.getDay();
        var diff = TranslateMomentWeekDay(cell.weekDay) - day;
        date.setDate(date.getDate() + diff);
        var val = ToBRDate(date.toISOString());
        return val;
    };

    const fetchCell = async (cellId) => {
        var result = await GetCell(cellId);
        if (result.isValid) {
            setCell(result.data);
            setReport({
                ...report,
                cellId: result.data.id,
                hostId: result.data.hostId,
                timothyId: result.data.timothyId,
                leadershipId: result.data.leadId,
                dateOnly: getSuggestedDate(result.data),
                timeOnly: (result.data.time || '').substring(0, 5)
            });

        }
    };

    const ValidateForm = () => {
        var errors = {};

        if (!report.cellId) {
            errors.cellId = 'Selecione uma célula digitando o nome do líder para listá-las.';
        }

        if (!report.weekCellSkipped) {
            if (!report.dateOnly) {
                errors.dateOnly = 'Insira a data da célula';
            }

            if (!report.timeOnly) {
                errors.timeOnly = 'Insira o horário da célula';
            }

            if (!report.subject) {
                errors.subject = 'Insira o tema ministrado';
            }
        }

        if (report.weekCellSkipped && report.notes.trim().length === 0) {
            errors.notes = 'Por favor, justifique a interrupção da célula.';
        }

        if(Object.keys(errors).length > 0)
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Verifique os erros no formulário para prosseguir o cadastro.'
            });

        setErrors(errors);
        return Object.keys(errors).length === 0;
    }

    const SubmitReport = async (e) => {
        e.preventDefault();

        if (!ValidateForm()) {
            e.stopPropagation();
            return;
        }

        setLoading(true);

        try {
            let reportToSend = JSON.parse(JSON.stringify(report));
            delete reportToSend.dateOnly;
            delete reportToSend.timeOnly;

            var result = await LogReport({
                ...reportToSend,
                contribution: MoneyToDecimal(report.contribution),
                visitors: report.visitors,
                date: report.weekCellSkipped ?
                    FromBRDate(getSuggestedDate(cell)) + "T" + (cell.time || '').substring(0, 5) + ":00" :
                    FromBRDate(report.dateOnly) + "T" + report.timeOnly + ":00"
            });

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

    const handleVisitorChange = (e) => {
        var index = e.target.parentElement.parentElement.parentElement.getAttribute('index');
        var newVisitors = report.visitors;
        newVisitors[index] = e.target.value;
        setReport({ ...report, visitors: newVisitors });
    };

    const addVisitor = () => {
        setReport({ ...report, visitors: [...report.visitors, ''] });
    }

    const removeVisitor = (e) => {
        var index = e.target.parentElement.parentElement.parentElement.getAttribute('index');
        var newVisitors = report.visitors;
        newVisitors.splice(index, 1);
        setReport({ ...report, visitors: newVisitors });
    };

    return (
        loading ?
            (<Spinner animation="border" />) : (
                <>
                    <h1>Registrar relatório</h1>
                    <hr />
                    <>
                        <Form noValidate onSubmit={SubmitReport}>
                            <Container fluid>
                                <h6>Célula</h6>

                                <Row className="mb-0 pr-0">
                                    <Col>
                                        <CellPicker label="" setValue={(value) => handleSelectChangeEvent('cellId', value)} />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.cellId}
                                        </Form.Control.Feedback>
                                    </Col>
                                </Row>

                                {cell && cell.id && (
                                    <>
                                        <h6>Dados da Célula selecionada</h6>
                                        <Row className="mb-3 pr-0">
                                            <Col xs={12} md={4} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Líder</InputGroup.Text>
                                                    <Form.Control value={cell.lead?.name} disabled readOnly />
                                                </InputGroup>
                                            </Col>

                                            <Col xs={12} md={4} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Timóteo</InputGroup.Text>
                                                    <Form.Control value={cell.timothy?.name} disabled readOnly />
                                                </InputGroup>
                                            </Col>

                                            <Col xs={12} md={4} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Anfitrião</InputGroup.Text>
                                                    <Form.Control value={cell.host?.name} disabled readOnly />
                                                </InputGroup>
                                            </Col>
                                        </Row>

                                        <Row className="mb-3 pr-0">
                                            <Col xs={12} md={6} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Líder G12</InputGroup.Text>
                                                    <Form.Control value={cell.lead?.computedG12Leader} disabled readOnly />
                                                </InputGroup>
                                            </Col>

                                            <Col xs={12} md={6} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Modalidade</InputGroup.Text>
                                                    <Form.Control value={translateCellType(cell.cellType)} disabled readOnly />
                                                </InputGroup>
                                            </Col>
                                        </Row>

                                        <Row className="mb-4 pr-0">
                                            <Col xs={12} md={4} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Logradouro</InputGroup.Text>
                                                    <Form.Control value={cell.place?.street} disabled readOnly />
                                                </InputGroup>
                                            </Col>

                                            <Col xs={12} md={4} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Número</InputGroup.Text>
                                                    <Form.Control value={cell.place?.number} disabled readOnly />
                                                </InputGroup>
                                            </Col>

                                            <Col xs={12} md={4} className="mb-3 pr-0">
                                                <InputGroup>
                                                    <InputGroup.Text>Bairro</InputGroup.Text>
                                                    <Form.Control value={cell.place?.neighborhood} disabled readOnly />
                                                </InputGroup>
                                            </Col>
                                        </Row>
                                    </>
                                )}

                            </Container>

                            <Container fluid>

                                <Row>
                                    <h6>Dados do Relatório</h6>
                                </Row>

                                <Row className="mb-3 pr-0">
                                    <Col>
                                        <Form.Group className="mt-1">
                                            <FormCheck type="checkbox" id="weekCellSkipped" name="weekCellSkipped"
                                                defaultChecked={report.weekCellSkipped} value={report.weekCellSkipped} label="Não houve célula"
                                                onChange={(event) => handleChangeEvent({ target: { name: 'weekCellSkipped', value: event.target.checked } })} />
                                        </Form.Group>
                                    </Col>
                                </Row>

                                <Row className="pr-0">
                                    <Col xs={12} md={6}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label id="basic-addon2">Data:</Form.Label>
                                            <InputMask mask="99/99/9999" value={report.dateOnly} onChange={handleChangeEvent} disabled={report.weekCellSkipped}>
                                                <Form.Control type="text" name="dateOnly" aria-describedby="basic-addon2" />
                                            </InputMask>
                                            <Form.Control.Feedback type="invalid">
                                                {errors.dateOnly}
                                            </Form.Control.Feedback>
                                        </Form.Group>
                                    </Col>

                                    <Col xs={12} md={6} className="mb-3">
                                        <Form.Group>
                                            <Form.Label id="basic-addon2">Horário:</Form.Label>
                                            <InputMask mask="99:99" value={report.timeOnly} onChange={handleChangeEvent} disabled={report.weekCellSkipped}>
                                                <Form.Control type="text" name="timeOnly" aria-describedby="basic-addon2" />
                                            </InputMask>
                                            <Form.Control.Feedback type="invalid">
                                                {errors.timeOnly}
                                            </Form.Control.Feedback>
                                        </Form.Group>
                                    </Col>
                                </Row>

                                <Row className="pr-0">
                                    <Col xs={12} md={8} className="mb-3">
                                        <Form.Group>
                                            <Form.Label id="basic-addon3">Tema:</Form.Label>
                                            <Form.Control id="basic-url" name="subject" value={report.subject} aria-describedby="basic-addon3"
                                                autoComplete='off' onChange={handleChangeEvent} disabled={report.weekCellSkipped} />
                                            <Form.Control.Feedback type="invalid">
                                                {errors.subject}
                                            </Form.Control.Feedback>
                                        </Form.Group>
                                    </Col>
                                    <Col xs={12} md={4} className="mb-3">
                                        <Form.Group>
                                            <Form.Label id="basic-addon3">Oferta:</Form.Label>
                                            <CurrencyInput
                                                value={report.contribution} disabled={report.weekCellSkipped}
                                                onChangeValue={(event, originalValue, maskedValue) => {
                                                    handleChangeEvent({ target: { name: 'contribution', value: maskedValue } });
                                                }}
                                                InputElement={<Form.Control id="basic-url" name="contribution" aria-describedby="basic-addon3"
                                                    autoComplete='off' />}
                                            />
                                        </Form.Group>
                                    </Col>
                                </Row>

                                <Row className="pr-0">
                                    <Col className="mb-3">
                                        <MemberPicker label="Participantes" isMulti={true}
                                            setValue={(value) => handleSelectChangeEvent('members', value)}
                                            value={report.frequenters} disabled={report.weekCellSkipped} />
                                    </Col>
                                </Row>

                                <Row className="mb-0 pr-0">
                                    <Col>
                                        <Form.Group>
                                            <Form.Label>Observações</Form.Label>
                                            <Form.Control as="textarea" aria-label="Observações" name="notes" value={report.notes} aria-describedby="basic-addon3"
                                                autoComplete='off' onChange={handleChangeEvent} />
                                            <Form.Control.Feedback type="invalid">
                                                {errors.notes}
                                            </Form.Control.Feedback>
                                        </Form.Group>
                                    </Col>
                                </Row>

                            </Container>

                            <Container fluid>
                                <div>
                                    <Row className="mb-2">
                                        <Col>
                                            <h6>Visitantes</h6>
                                        </Col>
                                        <Col>
                                            <Button variant="success" className="float-right" onClick={addVisitor}>+</Button>
                                        </Col>
                                    </Row>
                                    {report.visitors.map((visitor, index) => (
                                        <Row key={index} index={index}>
                                            <Col className="mb-3">
                                                <InputGroup>
                                                    <Form.Control id="basic-url" name={"visitors[" + index + "]"} value={visitor} aria-describedby="basic-addon3"
                                                        autoComplete='off' onChange={handleVisitorChange} />
                                                    <Button tabIndex={-1} variant="danger" className="float-right" onClick={removeVisitor}>-</Button>
                                                </InputGroup>
                                            </Col>
                                        </Row>
                                    ))}
                                </div>
                            </Container>

                            <Container fluid>
                                <Row className="mb-0 pr-0 d-flex">

                                    <Col xs={12}>
                                        <div className="float-end d-flex">
                                            <Button variant="primary" className="ms-3" type="submit">
                                                Enviar relatório
                                            </Button>
                                        </div>
                                    </Col>

                                </Row>
                            </Container>
                        </Form>
                    </>
                </>
            )
    );
}