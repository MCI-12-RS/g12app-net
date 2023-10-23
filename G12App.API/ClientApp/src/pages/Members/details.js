import React, { useState, useEffect } from "react";
import { InputGroup, Form, Col, Row, Button, Container, Spinner } from 'react-bootstrap';
import { useParams, useNavigate } from "react-router-dom";
import { GetMember, CreateMember, UpdateMember } from '../../services/Member.service';
import MemberPicker from '../../components/MemberPicker';
import { HandleDateMask, FromBRDate, ToBRDate } from '../../shared/utils';

import './style.css';
import moment from "moment";
import { AddressDetails } from "../../components/Address";

export const MemberDetails = () => {

    const emptyMember = {
        id: 0,
        name: '',
        birthDate: '',
        phoneNumber: '',
        mobileNumber: '',
        email: '',
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
    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [member, setMember] = useState(emptyMember);

    useEffect(() => {
        fetchUser();
    }, [params]);

    const fetchUser = async () => {
        setLoading(true);
        if (!params.id) {
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
        if (member.birthDate && member.birthDate.length == 10) {
            var date = moment(FromBRDate(member.birthDate));
            return moment().diff(date, 'years') ?? '-';
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
                            </Row>

                            <Row className="mb-3 pr-0 justify-content-md-center">
                                <Col>

                                    <InputGroup className="mb-3 pr-0" as={Col}>
                                        <InputGroup.Text id="basic-addon2">Data de Nascimento</InputGroup.Text>
                                        <Form.Control type="text" pattern="\d{2}\/\d{2}\/\d{2}" name="birthDate" onKeyPress={HandleDateMask}
                                            aria-describedby="basic-addon2" maxLength={10} value={member['birthDate']} onChange={handleChangeEvent} />
                                        <InputGroup.Text id="basic-addon2">{getAge() + ' anos'}</InputGroup.Text>
                                    </InputGroup>
                                </Col>

                                <Col>
                                    <InputGroup className="mb-3 pr-0" as={Col}>
                                        <InputGroup.Text id="basic-addon2">Celular</InputGroup.Text>
                                        <Form.Control name="mobileNumber" aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                            value={member.mobileNumber} />
                                    </InputGroup>
                                </Col>

                                <Col>
                                    <InputGroup className="mb-3" as={Col}>
                                        <InputGroup.Text id="basic-addon1">Telefone / Whatsapp</InputGroup.Text>
                                        <Form.Control name="phoneNumber" aria-describedby="basic-addon1" onChange={handleChangeEvent}
                                            value={member.phoneNumber} />
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
                                    {(params.id && <Button variant="danger" type="submit" className="">Deletar cadastro</Button>)}
                                    <Button variant="primary" type="submit" onClick={SaveMember} className="float-end">
                                        Salvar {(params.id ? "alteracoes" : "cadastro")}
                                    </Button>
                                </Col>

                            </Row>
                        </Container>
                    </>
                )}
        </>
    );
};