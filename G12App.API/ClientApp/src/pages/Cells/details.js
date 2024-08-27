import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Button, Container, FormCheck, Spinner } from 'react-bootstrap';
import { useParams, useNavigate, useLocation, Link } from "react-router-dom";
import { CreateCell, DeleteCell, GetCell, UpdateCell } from '../../services/Cell.service';
import MemberPicker from '../../components/MemberPicker';
import { useToastContext } from '../../contexts/ToastContext';
import { StringifyResponseErrors, FormatMemberPickerLabel, DecimalToMoney } from '../../shared/utils';
import { AddressDetails } from "../../components/Address";
import InputMask from "react-input-mask";
import Grid from '../../components/Grid';
import { LineChart } from '@mui/x-charts/LineChart';
import { DeleteReport, GetReports } from "../../services/Report.service";
import moment from "moment";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import { faArrowUp, faArrowDown, faCircle, faMagnifyingGlass, faCross, faRemove, faRemoveFormat } from '@fortawesome/free-solid-svg-icons'
import { useAuthenticationContext } from "../../contexts/AuthenticationContext";

import './style.css';

export const CellDetails = () => {

    let toastContext = useToastContext();
    let authContext = useAuthenticationContext();

    const emptyPlace = {
        street: '',
        number: '',
        neighborhood: '',
        city: '',
        state: 'RS'
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
        type: 'Evangelistic',
        place: emptyPlace
    };

    const params = useParams();
    const location = useLocation();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [cell, setCell] = useState(emptyCell);
    const [place, setPlace] = useState(emptyPlace);
    const [registerMore, setRegisterMore] = useState(location.state?.registerMore ?? false);
    const [pagination, setPagination] = useState({ page: 1, limit: 10 });
    const [chartXAxis, setChartXAxis] = useState([]);
    const [errors, setErrors] = useState({});

    const [total, setTotal] = useState(1);
    const [reports, setReports] = useState([]);

    const compare = (prev, curr) => {
        if (prev == null || curr == null) return <></>;
        return (curr > prev ? <strong className="text-success" style={{ fontSize: '1.2em' }}><FontAwesomeIcon icon={faArrowUp} /></strong> :
            curr < prev ? <strong className="text-danger" style={{ fontSize: '1.2em' }}><FontAwesomeIcon icon={faArrowDown} /></strong> :
                <strong className="text-info" style={{ fontSize: '1.2em' }}><FontAwesomeIcon icon={faCircle} /></strong>);
    }

    const displayReportNumbers = (report, reports, index, property) => {
        var prevReport = reports[index + 1];
        var comparer = <></>;
        if (prevReport != null)
            comparer = compare(prevReport[property], report[property]);

        return <>{(report[property] || 0).toString().padStart(2, '0')}&nbsp;&nbsp;&nbsp;{comparer}</>;
    }

    const DeleteReportPrompt = async (e, reportId) => {
        e.preventDefault();
        window.confirm('Tem certeza que deseja deletar o relatório?') && await ConfirmDeleteReport(reportId);
        return false;
    }

    const ConfirmDeleteReport = async (id) => {
        setLoading(true);

        try {
            var result = await DeleteReport(id);

            if (result.isValid) {
                toastContext.addToast({
                    title: 'Sucesso', hideDelay: 5000,
                    description: 'Cadastro deletado com sucesso!'
                });
                navigate('/cells/' + params.id, { refresh: true });
            } else {
                toastContext.addToast({
                    title: 'Erro', hideDelay: 10000,
                    description: <>{'Erro ao deletar cadastro.'}<br />{StringifyResponseErrors(result.errors)}</>
                });
            }

        }
        catch (e) {
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Erro de sistema ao deletar cadastro.'
            });
        }

        setLoading(false);
    }

    const columns = [
        {
            name: '#',
            properties: { md: 1 },
            render: (member) => (member.id.toString() ?? '0').padStart(5, '0')
        },
        {
            name: 'Data da Reunião',
            properties: { md: 2 },
            render: (report) => (<span><strong>{moment(report.date).format('DD/MM/yyyy')}</strong><br />{moment(report.date).format('HH:mm')}</span>)
        },
        {
            name: 'Detalhes',
            properties: { md: 4 },
            render: (report) => {
                return (
                    report.weekCellSkipped ?
                        <>
                            <span><b>Não houve célula.</b></span>
                            <br />
                            <span><b>Observações:</b> {report.notes}</span>
                        </>
                        :
                        <>
                            <span><b>Tema:</b> {report.subject}</span>
                        </>
                )
            }
        },
        {
            name: 'Oferta',
            properties: { md: 2 },
            render: (report) => DecimalToMoney(report.contribution)
        },
        {
            name: 'Participantes',
            className: 'text-center',
            properties: { md: 1 },
            render: (report, index) => displayReportNumbers(report, reports, index, 'numMembers')
        },
        {
            name: 'Visitantes',
            className: 'text-center',
            properties: { md: 1 },
            render: (report, index) => displayReportNumbers(report, reports, index, 'numVisitors')
        },
        {
            name: '',
            properties: { md: 1 },
            className: 'text-right actions',
            render: (report) => authContext.auth.user.role !== 'AppUser' &&
                <FontAwesomeIcon icon={faRemove} className="remove-button" title="Remover relatório" onClick={(e) => DeleteReportPrompt(e, report.id)} />
        }
    ];

    useEffect(() => {
        if (location.state?.registerMore) {
            setRegisterMore(true);
        }
    }, []);

    useEffect(() => {
        fetchCell();
    }, [params]);

    const fetchCell = async () => {
        setLoading(true);

        if (!params.id) {
            setCell(emptyCell);
            setLoading(false);
            return;
        }

        var result = await GetCell(params.id);
        if (result.isValid && result.data != null) {
            setCell(result.data);
            setPlace(result.data.place);
            var reportResult = await GetReports({ cellId: params.id }, pagination, { sortBy: 'date', order: true });
            if (result.isValid) {
                setReports(reportResult.data);
                setTotal(reportResult.totalRecords ?? reportResult.data.length);
                setChartXAxis(reportResult.data.map(x => x.dateOnly).reverse());
            }
        } else {
            navigate('/cells');
        }

        setLoading(false);
    };

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setCell({ ...cell, [name]: value });
    };

    const handleSelectChangeEvent = (name, value) => {
        setCell({ ...cell, [name]: value });
    };

    const ValidateForm = () => {
        let errors = {};

        if (!cell.leadId)
            errors.leadId = 'Selecione o líder da célula';

        if (!cell.timothyId)
            errors.timothyId = 'Selecione o timóteo da célula';

        if (!cell.hostId)
            errors.hostId = 'Selecione o anfitrião da célula';

        if (!cell.weekDay)
            errors.weekDay = 'Selecione o dia da semana';

        if (!cell.time)
            errors.time = 'Defina o horário';

        setErrors(errors);

        if (Object.keys(errors).length > 0)
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Verifique os erros no formulário para prosseguir o cadastro.'
            });

        return Object.keys(errors).length === 0;
    }

    const SaveCell = async (e) => {
        e.preventDefault();

        var form = e.currentTarget;
        if (form.checkValidity() === false) {
            e.stopPropagation();
            return;
        }

        if (!ValidateForm()) {
            return;
        }

        setLoading(true);

        try {
            var result;
            if (params.id) {
                result = await UpdateCell({ ...cell, time: (cell.time + ":00").substring(0, 8) });
            } else {
                result = await CreateCell({ ...cell, time: (cell.time + ":00").substring(0, 8) });
            }

            if (result.isValid) {
                toastContext.addToast({
                    title: 'Sucesso', hideDelay: 5000,
                    description: 'Cadastro salvo com sucesso!'
                });

                if (registerMore)
                    navigate('/new-cell', { state: { registerMore: true } });
                else
                    navigate('/cells/' + result.data.id);
            } else {
                toastContext.addToast({
                    title: 'Erro', hideDelay: 10000,
                    description: <>{'Erro ao salvar cadastro.'}<br />{StringifyResponseErrors(result.errors)}</>
                });
            }

        }
        catch (e) {
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Erro de sistema ao salvar cadastro.'
            });
        }

        setLoading(false);
    }

    const DeleteCellPrompt = async (e) => {
        e.preventDefault();
        window.confirm('Tem certeza que deseja deletar o cadastro?') && await ConfirmDeleteCell();
        return false;
    }

    const ConfirmDeleteCell = async () => {
        setLoading(true);

        try {
            var result = await DeleteCell(cell.id);

            if (result.isValid) {
                toastContext.addToast({
                    title: 'Sucesso', hideDelay: 5000,
                    description: 'Cadastro deletado com sucesso!'
                });
                navigate('/cells');
            } else {
                toastContext.addToast({
                    title: 'Erro', hideDelay: 10000,
                    description: <>{'Erro ao deletar cadastro.'}<br />{StringifyResponseErrors(result.errors)}</>
                });
            }

        }
        catch (e) {
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Erro de sistema ao deletar cadastro.'
            });
        }

        setLoading(false);
    }

    const AssignHostAddress = (option) => {
        if (option?.item?.homeAddress != null) {
            let address = option?.item?.homeAddress;
            if (address != null)
                setPlace({ ...address, id: null });
        }
    }

    useEffect(() => {
        setCell({ ...cell, place: place });
    }, [place]);

    return (
        loading ?
            (<Spinner animation="border" />) : (
                <>
                    {(params.id ? <h1>Alterar cadastro de célula</h1> : <h1>Cadastrar nova célula</h1>)}
                    <hr />
                    <Form onSubmit={SaveCell}>
                        <Container fluid>
                            <h6>Participantes</h6>
                            <Row className="mb-0">
                                <Col xs={12} md={4}>
                                    <MemberPicker label="Líder"
                                        initialValue="Selecione o líder da célula"
                                        setValue={(value) => handleSelectChangeEvent('leadId', value)}
                                        selectedLabel={FormatMemberPickerLabel(cell.lead)}
                                        selectedValue={cell.lead?.id} />
                                    <Form.Control.Feedback type="invalid">
                                        {errors.leadId}
                                    </Form.Control.Feedback>
                                </Col>

                                <Col xs={12} md={4}>
                                    <MemberPicker label="Timóteo"
                                        initialValue="Selecione o timóteo da célula"
                                        setValue={(value) => handleSelectChangeEvent('timothyId', value)}
                                        selectedLabel={FormatMemberPickerLabel(cell.timothy)}
                                        selectedValue={cell.timothy?.id} />
                                    <Form.Control.Feedback type="invalid">
                                        {errors.timothyId}
                                    </Form.Control.Feedback>
                                </Col>

                                <Col xs={12} md={4}>
                                    <MemberPicker label="Anfitrião"
                                        initialValue="Selecione o anfitrião da célula"
                                        setValue={(value) => handleSelectChangeEvent('hostId', value)}
                                        selectedLabel={FormatMemberPickerLabel(cell.host)}
                                        changeValueEvent={(option) => AssignHostAddress(option)}
                                        selectedValue={cell.host?.id} />
                                    <Form.Control.Feedback type="invalid">
                                        {errors.hostId}
                                    </Form.Control.Feedback>
                                </Col>
                            </Row>
                        </Container>

                        <Container fluid>
                            <h6>Definições</h6>
                            <Row>

                                <Col xs={12} md={4}>
                                    <Form.Group as={Col} hasValidation className="mb-3 pr-0">
                                        <Form.Label>Modalidade</Form.Label>
                                        <Form.Select name="cellType" defaultValue={cell.cellType} onChange={handleChangeEvent}>
                                            <option value="Evangelistic">Evangelística</option>
                                            <option value="Discipleship">Discipuladora</option>
                                            <option value="Macrocell">Macrocélula</option>
                                        </Form.Select>
                                    </Form.Group>
                                </Col>


                                <Col xs={12} md={4}>
                                    <Form.Group as={Col} hasValidation className="mb-3 pr-0">
                                        <Form.Label>Dia da Semana</Form.Label>
                                        <Form.Select name="weekDay" defaultValue={cell.weekDay} onChange={handleChangeEvent}>
                                            <option value="">Selecione o dia</option>
                                            <option value="Monday">Segunda-feira</option>
                                            <option value="Tuesday">Terça-feira</option>
                                            <option value="Wednesday">Quarta-feira</option>
                                            <option value="Thursday">Quinta-feira</option>
                                            <option value="Friday">Sexta-feira</option>
                                            <option value="Saturday">Sábado</option>
                                            <option value="Sunday">Domingo</option>
                                        </Form.Select>
                                        <Form.Control.Feedback type="invalid">
                                            {errors.weekDay}
                                        </Form.Control.Feedback>
                                    </Form.Group>
                                </Col>

                                <Col xs={12} md={4}>
                                    <Form.Group as={Col} className="mb-3 pr-0">
                                        <Form.Label>Horário</Form.Label>
                                        <InputMask mask="99:99" value={cell['time']} onChange={handleChangeEvent}>
                                            <Form.Control name="time" />
                                        </InputMask>
                                        <Form.Control.Feedback type="invalid">
                                            {errors.time}
                                        </Form.Control.Feedback>
                                    </Form.Group>
                                </Col>

                            </Row>
                        </Container>

                        <AddressDetails address={place} setAddress={(address) => setPlace(address)} />

                        <Container fluid>
                            <Row className="mb-0 pr-0 d-flex">

                                <Col xs={12}>
                                    {(params.id && <Button variant="danger" type="button" onClick={DeleteCellPrompt}>Deletar cadastro</Button>)}

                                    <div className="float-end d-flex">
                                        {(!params.id ?
                                            <InputGroup className="pt-1 me-3" as={Col}>
                                                <FormCheck type="checkbox" defaultChecked={registerMore} label="Cadastrar mais células"
                                                    onChange={(event) => setRegisterMore(event.target.checked)} />
                                            </InputGroup> :
                                            '')}

                                        <Button variant="primary" className="ms-3" type="submit">
                                            Salvar {(params.id ? "alterações" : "cadastro")}
                                        </Button>
                                    </div>
                                </Col>

                            </Row>
                        </Container>
                        <br />
                        {(params.id ? (
                            <>
                                <hr />
                                <br />
                                <h5>Últimos Relátorios enviados</h5>
                                <Container fluid className="no-background">
                                    <Row className="mb-3 pr-0 cell-reports-list">
                                        <Grid
                                            columns={columns}
                                            data={reports}
                                            textTotal='relatórios enviados'
                                            loading={loading}
                                            pagination={pagination}
                                            setPagination={setPagination}
                                            totalRecords={total}
                                            displayPagination={false}
                                        />
                                    </Row>
                                </Container>
                                <br />
                                {(reports.length === 0 ? <></> :
                                    <>
                                        <hr />
                                        <br />
                                        <h5>Linha de evolução</h5>
                                        <div style={{ display: 'flex', justifyContent: 'center' }}>
                                            <LineChart
                                                xAxis={[{ scaleType: 'point', data: chartXAxis }]}
                                                series={[{
                                                    dataKey: 'numMembers',
                                                    label: 'Participantes',
                                                    color: '#2CAAE1',
                                                    showMark: true
                                                },
                                                {
                                                    dataKey: 'numVisitors',
                                                    label: 'Visitantes',
                                                    color: '#00AB82',
                                                    showMark: true
                                                }]}
                                                dataset={reports.map(x => x).reverse()}
                                                height={300}
                                            />
                                        </div>
                                        <br />
                                    </>
                                )}
                                <br />
                            </>
                        ) : '')}
                    </Form>
                </>
            )
    )
}