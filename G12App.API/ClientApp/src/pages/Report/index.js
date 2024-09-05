import React, { useEffect } from 'react';
import { Form, Col, Row, Accordion, Container } from 'react-bootstrap';
import { debounce } from 'lodash';
import Grid from '../../components/Grid';
import { isMobile } from 'react-device-detect';
import { DownloadExportReports, GetReports } from '../../services/Report.service.js';
import moment from 'moment';
import { DecimalToMoney, FromBRDate, ToBRDate, CleanSearchForm, FormatFirstLastName } from '../../shared/utils.js';
import { faHouse, faPerson } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Link, useParams } from 'react-router-dom';
import ReactInputMask from 'react-input-mask';

export const Reports = () => {

    const firstCurrentWeekDay = ToBRDate(moment().startOf('week'));
    const endCurrentWeekDay = ToBRDate(moment().endOf('week'));

    const [cells, setCells] =
        React.useState([]);
    const [pagination, setPagination] =
        React.useState({ page: 1, limit: 20 });
    const [searchData, setSearchData] =
        React.useState({
            leadership: null,
            g12Leadership: null,
            startDate: firstCurrentWeekDay,
            endDate: endCurrentWeekDay
        });
    const [loading, setLoading] = React.useState(false);
    const [total, setTotal] = React.useState(0);
    const [sort] = React.useState({ sortBy: 'date', order: true });
    const params = useParams();

    const IsCellSpecific = () => {
        return params.cellId;
    };

    const columns = [
        {
            name: '#',
            properties: { md: 1 },
            className: "text-left d-none d-md-block",
            render: (member) => (member.id.toString() ?? '0').padStart(5, '0')
        },
        {
            name: 'Dados da célula',
            render: (report) => <span>
                <span><b>Líder:</b> {FormatFirstLastName(report.lead?.name)} </span>
                {report.lead?.computedG12Leader && (<><br /><span><b>G12:</b> {FormatFirstLastName(report.lead?.computedG12Leader)} </span></>)}
                {report.cell?.place?.street && (<><br /><span><b> Endereço:</b> {report.cell?.place?.street}, {report.cell?.place?.number} {report.cell?.place?.neighborhood && (" - " + report.cell?.place?.neighborhood)}</span></>)}
            </span>
        },
        {
            name: 'Detalhes',
            render: (report) => {
                return (
                    <span>
                        <span>{moment(report.date).format('DD/MM/yyyy')} - {moment(report.date).format('HH:mm')}</span>
                        <br />
                        <span>
                            {report.weekCellSkipped ?
                                <>
                                    <span><b>Não houve célula.</b></span>
                                    <br />
                                    <span><b>Observações:</b> {report.notes}</span>
                                </>
                                :
                                <>
                                    <span><b>Tema:</b> {report.subject}</span>
                                    {report.notes && <><br /><span><b>Observações:</b> {report.notes}</span></>}
                                </>}
                        </span>
                    </span>
                )
            }
        },
        {
            name: 'Oferta',
            properties: { md: 1 },
            render: (report) => DecimalToMoney(report.contribution)
        },
        {
            name: 'Participantes',
            className: 'text-center',
            properties: { md: 1 },
            render: (report, index) => <>{report.numMembers}</>
        },
        {
            name: 'Visitantes',
            className: 'text-center',
            properties: { md: 1 },
            render: (report, index) => <>{report.numVisitors}</>
        },
        {
            name: '',
            className: 'text-right actions',
            properties: { md: 1 },
            render: (report) => <span className='text-center'>
                <Link to={"/members/" + report.leadershipId} title="Dados do líder"><FontAwesomeIcon icon={faPerson} /></Link>
                <Link to={"/cells/" + report.cellId} title="Dados da célula"><FontAwesomeIcon icon={faHouse} /></Link>
            </span>
        }
    ];

    const search = async () => {
        setLoading(true);

        let payload = GetPayload();
        const result = await GetReports(payload, pagination, sort);
        if (result.isValid) {
            setCells(result.data);
            setTotal(result.totalRecords ?? result.data.length);
        }

        setLoading(false);
    };

    const GetPayload = () => {
        return {
            ...searchData,
            startDate: searchData.startDate ? FromBRDate(searchData.startDate) : null,
            endDate: searchData.endDate ? FromBRDate(searchData.endDate) : null
        };
    }

    const debounceEventTrigger = debounce(field => {
        var currentValue = searchData[field.name];
        var newValue = field.value;
        if (newValue === '__/__/____' || !newValue) newValue = null;
        if (newValue !== currentValue)
            setSearchData({ ...searchData, [field.name]: newValue });
    }, 500);

    const handleSearchEvent = (event) => {
        var field = event.target;
        debounceEventTrigger(field);
    };

    const DownloadReportsResult = async () => {
        DownloadExportReports(GetPayload(), { page: 1, limit: 9999 }, {});
    };

    useEffect(() => {
        setPagination({ ...pagination, page: 1 });
    }, [searchData]);

    useEffect(() => {
        search();
    }, [pagination]);

    useEffect(() => {
        if (params.cellId) {
            setSearchData({ ...searchData, cellId: params.cellId, startDate: null, endDate: null });
        }
    }, [params.cellId]);

    return (
        <>
            <div>
                {!IsCellSpecific() ? <h1>Relatórios</h1> : <h1>Relatórios da célula</h1>}
                <hr />

                {!IsCellSpecific() && (
                    <Container fluid className="filter-box">

                        <Accordion className="mb-3" {... !isMobile && ({ defaultActiveKey: ['0'], alwaysOpen: true })}>
                            <Accordion.Item eventKey="0">
                                <Accordion.Header>
                                    Filtros
                                </Accordion.Header>

                                <Accordion.Body>

                                    <Row className="clean-form">
                                        <Link onClick={() => CleanSearchForm(setSearchData)}>Limpar filtros</Link>
                                    </Row>

                                    <Row className="pr-3">
                                        <Form.Group className="mb-3" as={Col}>
                                            <Form.Label>Líder de Célula</Form.Label>
                                            <Form.Control id="basic-url" name="leadership" autoComplete='off' onKeyUp={handleSearchEvent} />
                                        </Form.Group>

                                        <Form.Group className="mb-3 pr-3" as={Col}>
                                            <Form.Label>G12</Form.Label>
                                            <Form.Control id="basic-url" name="g12Leadership" autoComplete='off' onKeyUp={handleSearchEvent} />
                                        </Form.Group>
                                    </Row>

                                    <Row className="pr-3">
                                        <Form.Group className="mb-3" as={Col}>
                                            <Form.Label>Desde</Form.Label>
                                            <ReactInputMask mask="99/99/9999" onChange={handleSearchEvent} defaultValue={firstCurrentWeekDay}>
                                                <Form.Control id="basic-url" name="startDate" autoComplete='off' />
                                            </ReactInputMask>
                                        </Form.Group>

                                        <Form.Group className="mb-3 pr-3" as={Col}>
                                            <Form.Label>Até</Form.Label>
                                            <ReactInputMask mask="99/99/9999" onChange={handleSearchEvent} defaultValue={endCurrentWeekDay}>
                                                <Form.Control id="basic-url" name="endDate" autoComplete='off' />
                                            </ReactInputMask>
                                        </Form.Group>
                                    </Row>

                                </Accordion.Body>
                            </Accordion.Item>
                        </Accordion>

                    </Container>
                )}

                <div className="row">
                    <Grid
                        downloadMethod={DownloadReportsResult}
                        columns={columns}
                        data={cells}
                        textTotal='relatórios'
                        loading={loading}
                        pagination={pagination}
                        setPagination={setPagination}
                        totalRecords={total} />
                </div>
            </div>
        </>
    )
}