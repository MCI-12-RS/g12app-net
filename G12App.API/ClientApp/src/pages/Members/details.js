import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Button, Container, Spinner, FormCheck } from 'react-bootstrap';
import { useParams, useNavigate, useLocation } from "react-router-dom";
import { GetMember, CreateMember, UpdateMember, DeleteMember } from '../../services/Member.service';
import MemberPicker from '../../components/MemberPicker';
import { HandleDateMask, FromBRDate, ToBRDate } from '../../shared/utils';

import './style.css';
import moment from "moment";
import { AddressDetails } from "../../components/Address";
import InputMask from "react-input-mask";

export const MemberDetails = () => {

    const emptyMember = {
        id: 0,
        name: '',
        birthDate: '',
        phoneNumber: '',
        mobileNumber: '',
        email: '',
        gender: 'Male',
        homeAddress: {
            street: '',
            number: '',
            neighborhood: '',
            city: '',
            state: ''
        },
        leadershipId: null,
        leadership: {
            id: null,
            name: ''
        }
    };

    const params = useParams();
    const location = useLocation();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [member, setMember] = useState(emptyMember);
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
            setMember(emptyMember);
            setLoading(false);
            return;
        }

        var result = await GetMember(params.id);
        if (result.isValid) {
            const member = result.data;
            if (member.birthDate) {
                member.birthDate = ToBRDate(member.birthDate);
            }
            setMember(result.data);
        }
        setLoading(false);
    };

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setMember({ ...member, [name]: value });
    };

    const handleSelectChangeEvent = (name, value) => {
        setMember({ ...member, [name]: value });
    };

    const getAge = () => {
        if (member.birthDate && member.birthDate.length > 0 && member.birthDate.indexOf('_') < 0) {
            var date = moment(FromBRDate(member.birthDate));
            if (date.isValid()) {
                var y = moment().diff(date, 'years');
                return y > 100 ? '-' : y;
            }
        }

        return '-'
    }

    const SaveMember = async () => {
        setLoading(true);

        try {
            var result;
            if (params.id) {
                result = await UpdateMember({ ...member, birthDate: FromBRDate(member.birthDate) });
            } else {
                result = await CreateMember({ ...member, birthDate: FromBRDate(member.birthDate) });
            }

            if (result.isValid) {
                alert('Cadastro salvo com sucesso!');
                if(registerMore)
                    navigate('/new-member', {state: { registerMore: true }});
                else 
                    navigate('/members/' + result.data.id);
            } else {
                alert('Erro ao salvar cadastro.');
            }

        }
        catch (e) {
            alert('Erro ao salvar cadastro.');
        }

        setLoading(false);
    }

    const DeleteMemberPrompt = async (e) => {
        e.preventDefault();
        window.confirm('Tem certeza que deseja deletar o cadastro?') && await ConfirmDeleteMember();
        return false;
    }

    const ConfirmDeleteMember = async () => {
        setLoading(true);

        try {
            var result = await DeleteMember(member.id);

            if (result.isValid) {
                alert('Cadastro deletado com sucesso!');
                navigate('/members');
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
        <>
            {(params.id ? <h1>Alterar cadastro de pessoa</h1> : <h1>Cadastrar nova pessoa</h1>)}
            <hr />
            {loading ?
                (<Spinner animation="border" />) : (
                    <>
                        <Container fluid>
                            <h6>Dados pessoais</h6>
                            <Row className="mb-3 pr-0 justify-content-md-center">
                                <Col>
                                    <InputGroup className="mb-3" as={Col}>
                                        <InputGroup.Text id="basic-addon1">Nome</InputGroup.Text>
                                        <Form.Control name="name" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={member['name']} />
                                    </InputGroup>
                                </Col>

                                <Col>
                                    <InputGroup className="mb-3 pr-0" as={Col}>
                                        <InputGroup.Text id="basic-addon2">Email</InputGroup.Text>
                                        <Form.Control name="email" aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                            value={member.email} />
                                    </InputGroup>
                                </Col>

                                <Col>
                                    <InputGroup className="mb-3 pr-0" as={Col}>
                                        <InputGroup.Text id="basic-addon2">Sexo</InputGroup.Text>
                                        <Form.Select name="gender" defaultValue={member.gender} aria-describedby="basic-addon2" onChange={handleChangeEvent}>
                                            <option value="Male">Masculino</option>
                                            <option value="Female">Feminino</option>
                                        </Form.Select>
                                    </InputGroup>
                                </Col>
                            </Row>

                            <Row className="mb-3 pr-0 justify-content-md-center">
                                <Col>

                                    <InputGroup className="mb-3 pr-0" as={Col}>
                                        <InputGroup.Text id="basic-addon2">Data de Nascimento</InputGroup.Text>
                                        <InputMask mask="99/99/9999" value={member['birthDate']} onChange={handleChangeEvent}>
                                            <Form.Control type="text" name="birthDate"
                                                aria-describedby="basic-addon2" />
                                        </InputMask>
                                        <InputGroup.Text id="basic-addon2">{getAge() + ' anos'}</InputGroup.Text>
                                    </InputGroup>
                                </Col>

                                <Col>
                                    <InputGroup className="mb-3 pr-0" as={Col}>
                                        <InputGroup.Text id="basic-addon2">Celular / Whatsapp</InputGroup.Text>
                                        <InputMask mask="(99) 99999-9999" value={member['mobileNumber']} onChange={handleChangeEvent}>
                                            <Form.Control name="mobileNumber" aria-describedby="basic-addon2" />
                                        </InputMask>
                                    </InputGroup>
                                </Col>

                                <Col>
                                    <InputGroup className="mb-3" as={Col}>
                                        <InputGroup.Text id="basic-addon1">Telefone</InputGroup.Text>
                                        <InputMask mask="(99) 9999-9999" value={member['phoneNumber']} onChange={handleChangeEvent}>
                                            <Form.Control name="phoneNumber" aria-describedby="basic-addon1" />
                                        </InputMask>
                                    </InputGroup>
                                </Col>
                            </Row>
                        </Container>

                        <Container fluid>
                            <h6>Ministerial</h6>
                            <Row className="mb-3 pr-0 justify-content-md-center">
                                <Col>
                                    <MemberPicker label="Lideranca"
                                        setValue={(value) => handleSelectChangeEvent('leadershipId', value)}
                                        notInIds={[member.id]}
                                        selectedLabel={member.leadership?.name}
                                        selectedValue={member.leadership?.id} />
                                </Col>
                            </Row>
                        </Container>

                        <AddressDetails address={member.homeAddress} setAddress={(address) => handleSelectChangeEvent('homeAddress', address)} />

                        <Container fluid>
                            <Row className="mb-3 pr-0 d-flex">

                                <Col xs={12}>
                                    {(params.id && <Button variant="danger" type="submit" onClick={DeleteMemberPrompt}>Deletar cadastro</Button>)}

                                    <div className="float-end d-flex">
                                        {(!params.id ?                                     
                                        <InputGroup className="pt-1 me-3" as={Col}>
                                            <FormCheck type="checkbox" defaultChecked={registerMore} label="Cadastrar mais pessoas" 
                                                onChange={(event) => setRegisterMore(event.target.checked)} /> 
                                        </InputGroup>: 
                                        '')}

                                        <Button variant="primary" className="ms-3" type="submit" onClick={SaveMember}>
                                            Salvar {(params.id ? "alteracoes" : "cadastro")}
                                        </Button>
                                    </div>
                                </Col>

                            </Row>
                        </Container>
                    </>
                )}
        </>
    );
};