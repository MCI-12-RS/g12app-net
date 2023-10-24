import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Button, Container, FormCheck } from 'react-bootstrap';
import { useParams, useNavigate, useLocation } from "react-router-dom";
import { CreateCell, DeleteCell, GetCell, UpdateCell } from '../../services/Cell.service';
import MemberPicker from '../../components/MemberPicker';




import './style.css';
import moment from "moment";
import { AddressDetails } from "../../components/Address";
import InputMask from "react-input-mask";

export const CellDetails = () => {

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
    const location = useLocation();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [cell, setCell] = useState(emptyCell);
    const [registerMore, setRegisterMore] = useState(location.state?.registerMore ?? false);

    useEffect(() => {
        if (location.state?.registerMore) {
            setRegisterMore(true);
        }
    }, []);

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

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setCell({ ...cell, [name]: value });
    };

    const handleSelectChangeEvent = (name, value) => {
        setCell({ ...cell, [name]: value });
    };

    const handleSearchEvent = (event) => {
        var field = event.target;
        debounceEventTrigger(field);
    };

    const SaveCell = async () => {
        setLoading(true);

        try {
            var result;
            if (params.id) {
                result = await UpdateCell(cell);
            } else {
                result = await CreateCell(cell);
            }

            if (result.isValid) {
                alert('Cadastro salvo com sucesso!');
                if (registerMore)
                    navigate('/new-cell', { state: { registerMore: true } });
                else
                    navigate('/cells/' + result.data.id);
            } else {
                alert('Erro ao salvar cadastro.');
            }

        }
        catch (e) {
            alert('Erro ao salvar cadastro.');
        }

        setLoading(false);
    }

    const DeleteCellPrompt = async (e) => {
        e.preventDefault();
        window.confirm('Tem certeza que deseja deletar o cadastro?') && await ConfirmDeleteMember();
        return false;
    }

    const ConfirmDeleteMember = async () => {
        setLoading(true);

        try {
            var result = await DeleteCell(cell.id);

            if (result.isValid) {
                alert('Cadastro deletado com sucesso!');
                navigate('/cells');
            } else {
                alert('Erro ao deletar cadastro.');
            }

        }
        catch (e) {
            alert('Erro ao deletar cadastro.');
        }

        setLoading(false);
    }

    return (
        loading ?
            (<div>Carregando...</div>) : (
                <>
                    {(params.id ? <h1>Alterar cadastro de célula</h1> : <h1>Cadastrar nova célula</h1>)}
                    <hr />
                    <Container fluid>
                        <h6>Participantes</h6>
                        <Row className="mb-3" as={Col}>
                            <Col>
                                <MemberPicker label="Líder"
                                    setValue={(value) => handleSelectChangeEvent('leadId', value)}
                                    selectedLabel={cell.lead?.name}
                                    selectedValue={cell.lead?.id} />
                            </Col>

                            <Col>
                                <MemberPicker label="Timóteo"
                                    setValue={(value) => handleSelectChangeEvent('timothyId', value)}
                                    selectedLabel={cell.timothy?.name}
                                    selectedValue={cell.timothy?.id} />
                            </Col>

                            <Col>
                                <MemberPicker label="Anfitrião"
                                    setValue={(value) => handleSelectChangeEvent('hostId', value)}
                                    selectedLabel={cell.host?.name}
                                    selectedValue={cell.host?.id} />
                            </Col>
                        </Row>
                    </Container>

                    <Container fluid>
                        <h6>Data/Horário</h6>
                        <Row ClassName="mb-3 pr-0">

                            <InputGroup className="mb-3 pr-0" as={Col}>
                                <InputGroup.Text id="basic-addon2">Dia da Semana</InputGroup.Text>
                                <Form.Select name="weekDay" defaultValue={cell.weekDay} aria-describedby="basic-addon2" onChange={handleChangeEvent}>
                                    <option value="">Selecione o dia</option>
                                    <option value="Sunday">Segunda-feira</option>
                                    <option value="Tuesday">Terça-feira</option>
                                    <option value="Wednesday">Quarta-feira</option>
                                    <option value="Thursday">Quinta-feira</option>
                                    <option value="Friday">Sexta-feira</option>
                                    <option value="Saturday">Sábado</option>
                                    <option value="Monday">Domingo</option>
                                </Form.Select>
                            </InputGroup>


                            <Col>
                                <InputGroup className="mb-3 pr-0" as={Col}>
                                    <InputGroup.Text id="basic-addon2">Horário</InputGroup.Text>
                                    <InputMask mask="99:99:99" value={cell['time']} onChange={handleChangeEvent}>
                                        <Form.Control name="time" aria-describedby="basic-addon2" />
                                    </InputMask>
                                </InputGroup>
                            </Col>
                        </Row>
                    </Container>

                    <AddressDetails address={cell.place} setAddress={(address) => handleSelectChangeEvent('place', address)} />

                    <Container fluid>
                        <Row className="mb-3 pr-0 d-flex">

                            <Col xs={12}>
                                {(params.id && <Button variant="danger" type="submit" onClick={DeleteCellPrompt}>Deletar cadastro</Button>)}

                                <div className="float-end d-flex">
                                    {(!params.id ?
                                        <InputGroup className="pt-1 me-3" as={Col}>
                                            <FormCheck type="checkbox" defaultChecked={registerMore} label="Cadastrar mais células"
                                                onChange={(event) => setRegisterMore(event.target.checked)} />
                                        </InputGroup> :
                                        '')}

                                    <Button variant="primary" className="ms-3" type="submit" onClick={SaveCell}>
                                        Salvar {(params.id ? "alteracoes" : "cadastro")}
                                    </Button>
                                </div>
                            </Col>

                        </Row>
                    </Container>
                </>
            )
    )
}