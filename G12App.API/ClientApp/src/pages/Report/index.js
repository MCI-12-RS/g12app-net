import React, { useEffect, useState } from 'react';
import { InputGroup, Container, Row, Button, Col, Form } from 'react-bootstrap';
import { GetCell, SearchCells } from "../../services/Cell.service";
import MemberPicker from '../../components/MemberPicker';
import CellPicker from '../../components/CellPicker';
import { LogReport } from "../../services/Report.service";
import { useNavigate } from 'react-router-dom';
import { debounce } from 'lodash';
import { FromBRDate } from '../../shared/utils';

import './style.css';
import moment from "moment";
import { AddressDetails } from "../../components/Address";
import InputMask from "react-input-mask";
import { useParams } from 'react-router-dom';

export const Report = () => {

    const [cells, setCells] =
        React.useState([]);
    const [pagination, setPagination] =
        React.useState({ page: 1, limit: 20 });
    const [total, setTotal] = React.useState(0);
    const [searchData, setSearchData] =
        React.useState({
            lead: '', leadership: '', host: '', timothy: '',
            street: '', cellType: null, weekDay: null, neighborhood: '', city: '', state: ''
        });

    const emptyReport = {
        subject: '',
        visitors: null,
        frequenters: null,
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
    const params = useParams();
    const [loading, setLoading] = useState(false);
    const [cell, setCell] = useState(emptyCell);
    const [report, setReport] = useState(emptyReport);
    const navigate = useNavigate();


    const fetchCell = async (cellId) => {
        setLoading(true);

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
        setLoading(false);
    };

    const search = async () => {
        setLoading(true);

        const result = await SearchCells(searchData, pagination);
        if (result.isValid) {
            setCells(result.data);
            setTotal(result.totalRecords ?? result.data.length);
        }

        setLoading(false);
    };

    const debounceEventTrigger = debounce(field => {
        var currentValue = searchData[field.name] || '';
        if (field.value != currentValue)
            setSearchData({ ...searchData, [field.name]: field.value });
    }, 500);

    const handleSearchEvent = (event) => {
        var field = event.target;
        debounceEventTrigger(field);
    };

    const submitReport = async () => {
        setLoading(true);

        /*try {*/
        var result;

        result = await LogReport({ ...report, date: FromBRDate(report.dateOnly) + "T" + report.timeOnly + ":00" });
        if (result.isValid) {
            alert('Relatório enviado com sucesso!');
            navigate('/reports/' + result.data.id)
        } else {
            alert('Erro ao enviar relatório');
        }
        //}
        /*catch (e) {
            alert('Erro ao enviar relatório.');
            console.error(e);
        }*/
    };

    const handleSelectChangeEvent = (name, value) => {
        fetchCell(value)

    };


    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setReport({ ...report, [name]: value });
    };

    const translateCellType = (cellType) => {
        if (cellType == "Evangelistic")
            return "Evangelística";
        if (cellType == "Discipleship")
            return "Discipuladora"
    };

    return (
        <>
            <h1>Registrar relatório</h1>
            <>
                <Form>
                    <Container fluid>
                        <h6>Selecione a célula</h6>

                        <Row className="mb-3 pr-0">
                            <Col>
                                <CellPicker label="Insira o nome do líder:"
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
                                <InputGroup.Text id="basic-addon1">Líder G12: {cell.lead?.leadership?.name}</InputGroup.Text>
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
                                <Form.Control id="basic-url" name="subject" aria-describedby="basic-addon3" autoComplete='off' onKeyUp={handleChangeEvent} />
                            </InputGroup>
                        </Row>

                        <Row className="mb-3 pr-0">
                            <Col>
                                <MemberPicker label="Participantes" isMulti={true}
                                    setValue={(value) => handleSelectChangeEvent('members', value)} />
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
                        <Button variant="primary" className="ms-3" type="submit" onClick={submitReport}>
                            Enviar relatório
                        </Button>
                    </Container>
                </Form>
            </>
        </>
    )
}