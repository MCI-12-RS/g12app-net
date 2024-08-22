import React, { useState, useEffect } from "react";
import { Form, Col, Row, Button, Container, Spinner, FormCheck } from 'react-bootstrap';
import { useParams, useNavigate, useLocation } from "react-router-dom";
import { GetMember, CreateMember, UpdateMember, DeleteMember } from '../../services/Member.service';
import MemberPicker from '../../components/MemberPicker';
import { FromBRDate, ToBRDate, StringifyResponseErrors, FormatMemberPickerLabel } from '../../shared/utils';
import { useToastContext } from '../../contexts/ToastContext';
import moment from "moment";
import { AddressDetails } from "../../components/Address";
import InputMask from "react-input-mask";
import md5 from 'md5';

import './style.css';
import { useAuthenticationContext } from "../../contexts/AuthenticationContext";

export const MemberDetails = () => {

    const toastContext = useToastContext();
    const authContext = useAuthenticationContext();

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
        },
        password: '',
        confirmPassword: '',
        role: 'AppUser'
    };

    const params = useParams();
    const location = useLocation();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
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

    const formatPhoneNumber = (phoneNumber) => {
        if (!phoneNumber) return '';
        phoneNumber = phoneNumber.replace(/[^0-9]/g, '');
        return `(${phoneNumber.substring(0, 2)}) ${phoneNumber.substring(2, 7)}-${phoneNumber.substring(7, 11)}`;
    }


    const fetchUser = async () => {
        setLoading(true);
        if (!params.id) {
            setMember(emptyMember);
            setMember({ ...member, leadershipId: authContext.auth.user.memberId });
            setLoading(false);
            return;
        }

        var result = await GetMember(params.id);
        if (result.isValid && result.data != null) {
            const member = result.data;

            if (member.birthDate) {
                member.birthDate = ToBRDate(member.birthDate);
            }

            if (member.mobileNumber) {
                member.mobileNumber = formatPhoneNumber(member.mobileNumber);
            }

            if (member.phoneNumber) {
                member.phoneNumber = formatPhoneNumber(member.phoneNumber);
            }

            setMember(member);
        } else {
            navigate('/members');
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
                return y > 100 ? '' : y;
            }
        }

        return ''
    }

    const ValidateMember = () => {
        var valid = true;

        if ((member.password || '').length)
            if (member.password !== member.confirmPassword) {
                toastContext.addToast({
                    title: 'Erro', hideDelay: 5000,
                    description: 'Senhas não conferem.'
                });
                valid = false;
            }

        return valid;
    }

    const IsCurrentMember = () => {
        return parseInt(params?.id) === authContext.auth.user.memberId;
    }

    const GetMemberPickerLabel = () => {
        if (!params.id) {
            var leader = (authContext.auth.user.g12LeadershipName ?? '');
            if (leader) leader += ' / ';
            return (leader ?? '') + authContext.auth.user.name;
        }

        return FormatMemberPickerLabel(member.leadership)
    };

    const GetMemberPickerId = () => {
        if (!params.id) {
            return authContext.auth.user.memberId;
        }

        return member.leadership?.id
    };

    const SaveMember = async (e) => {
        e.preventDefault();
        setLoading(true);

        var valid = ValidateMember();
        if (!valid) {
            setLoading(false);
            return false;
        }

        try {
            var result;

            var payload = {
                ...member,
                birthDate: FromBRDate(member.birthDate),
                password: (member.password || '').length ? md5(member.password) : null,
                mobileNumber: (member.mobileNumber || '').replace(/[^0-9]/g, ''),
                phoneNumber: (member.phoneNumber || '').replace(/[^0-9]/g, ''),
            };

            if (params.id) {
                result = await UpdateMember(payload);
            } else {
                result = await CreateMember(payload);
            }

            if (result.isValid) {
                toastContext.addToast({
                    title: 'Sucesso', hideDelay: 5000,
                    description: 'Cadastro salvo com sucesso!'
                });

                if (registerMore)
                    navigate('/new-member', { state: { registerMore: true } });
                else
                    navigate('/members/' + result.data.id);
            } else {
                toastContext.addToast({
                    title: 'Erro', hideDelay: 10000,
                    description: <>{'Erro ao salvar cadastro.'}<br />{StringifyResponseErrors(result.errors)}</>
                });
            }

        }
        catch (e) {
            console.error(e);
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Erro de sistema ao salvar cadastro.'
            });
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
                toastContext.addToast({
                    title: 'Sucesso', hideDelay: 5000,
                    description: 'Cadastro deletado com sucesso!'
                });
                navigate('/members');
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

    return (
        <>
            {(params.id ?
                authContext.auth.user.role !== 'AppUser' ? <h1>Alterar cadastro de pessoa</h1> : <h1>Dados de Pessoa</h1> :
                <h1>Cadastrar nova pessoa</h1>)}
            <hr />
            {loading ?
                (<Spinner animation="border" />) : (
                    <>
                        <Form>
                            <Container fluid>
                                <h6>Dados pessoais</h6>
                                <Row className="mb-3 pr-0 justify-content-md-center">
                                    <Col xs={12} md={4}> 
                                        <Form.Group className="mb-3">
                                            <Form.Label id="basic-addon1">Nome</Form.Label>
                                            <Form.Control name="name" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={member['name']} />
                                        </Form.Group >
                                    </Col>

                                    <Col xs={12} md={4}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label id="basic-addon2">Email</Form.Label>
                                            <Form.Control name="email" aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                                value={member.email} />
                                        </Form.Group >
                                    </Col>

                                    <Col xs={12} md={4}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label id="basic-addon2">Sexo</Form.Label>
                                            <Form.Select name="gender" defaultValue={member.gender} aria-describedby="basic-addon2" onChange={handleChangeEvent}>
                                                <option value="Male">Masculino</option>
                                                <option value="Female">Feminino</option>
                                            </Form.Select>
                                        </Form.Group >
                                    </Col>
                                </Row>

                                <Row className="mb-3 pr-0 justify-content-md-center">
                                    <Col xs={12} md={4}>

                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label id="basic-addon2">Data de Nascimento</Form.Label>
                                            <InputMask mask="99/99/9999" value={member['birthDate']} onChange={handleChangeEvent}>
                                                <Form.Control type="text" name="birthDate"
                                                    aria-describedby="basic-addon2" />
                                            </InputMask>
                                            {getAge() && (<Form.Label id="basic-addon2">{getAge() + ' anos'}</Form.Label>)}
                                        </Form.Group >
                                    </Col>

                                    <Col xs={12} md={4}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label id="basic-addon2">Celular / Whatsapp</Form.Label>
                                            <InputMask mask="(99) 99999-9999" value={member['mobileNumber']} onChange={handleChangeEvent}>
                                                <Form.Control name="mobileNumber" aria-describedby="basic-addon2" />
                                            </InputMask>
                                        </Form.Group >
                                    </Col>

                                    <Col xs={12} md={4}>
                                        <Form.Group className="mb-0">
                                            <Form.Label id="basic-addon1">Telefone</Form.Label>
                                            <InputMask mask="(99) 9999-9999" value={member['phoneNumber']} onChange={handleChangeEvent}>
                                                <Form.Control name="phoneNumber" aria-describedby="basic-addon1" />
                                            </InputMask>
                                        </Form.Group >
                                    </Col>
                                </Row>
                            </Container>

                            <Container fluid>
                                <h6>Ministerial</h6>
                                <Row className="mb-0 pr-0 justify-content-md-center">
                                    <Col>
                                        <MemberPicker label="Liderança"
                                            setValue={(value) => handleSelectChangeEvent('leadershipId', value)}
                                            notInIds={[member.id]}
                                            selectedLabel={GetMemberPickerLabel()}
                                            selectedValue={GetMemberPickerId()} />
                                    </Col>
                                </Row>
                            </Container>

                            <AddressDetails address={member.homeAddress} setAddress={(address) => handleSelectChangeEvent('homeAddress', address)} />

                            {(IsCurrentMember() || authContext.auth.user.role !== 'AppUser') && (

                                <Container fluid>
                                    <h6>Acesso</h6>
                                    <Row className="mb-3 pr-0 justify-content-md-center">
                                        <Col xs={12} md={6}>
                                            <Form.Group className="mb-3">
                                                <Form.Label id="basic-addon1">Senha</Form.Label>
                                                <Form.Control type="password" autoComplete="new-password" name="password" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={member['password']} />
                                            </Form.Group >
                                        </Col>
                                        <Col xs={12} md={6}>
                                            <Form.Group className="mb-0">
                                                <Form.Label id="basic-addon1">Confirmar Senha</Form.Label>
                                                <Form.Control type="password" autoComplete="new-password" name="confirmPassword" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={member['confirmPassword']} />
                                            </Form.Group >
                                        </Col>
                                    </Row>
                                </Container>

                            )}

                            {authContext.auth.user.role !== 'AppUser' && (

                                <Container fluid>
                                    <h6>Permissões</h6>

                                    <Row className="mb-3 pr-0 justify-content-md-center">
                                        <Col>
                                            <Form.Group className="mb-3 pr-0">
                                                <Form.Label id="basic-addon2">Função</Form.Label>
                                                <Form.Select name="role" defaultValue={member.role} aria-describedby="basic-addon2"
                                                    onChange={handleChangeEvent} disabled={IsCurrentMember() ? "disabled" : false}>
                                                    <option value="AppUser">Usuário</option>
                                                    <option value="AppAdmin">Administrador</option>
                                                    {authContext.auth.user.role === 'AppSuperAdmin' &&
                                                        (<option value="AppSuperAdmin">Super Administrador</option>)}
                                                </Form.Select>
                                            </Form.Group >
                                        </Col>
                                    </Row>
                                    <Row className="mb-0 pr-0 justify-content-md-center">
                                        <Col>
                                            <ul>
                                                <li><strong>Usuário:</strong> Pode visualizar seu ministério, cadastrar relatórios de células.</li>
                                                <li><strong>Administrator:</strong> Pode visualizar seu ministério, gerenciar membros, células e cadastrar relatórios.</li>
                                                <li><strong>Super Administrador:</strong> Pode gerenciar todos membros, células e relatórios, independente de ministério.</li>
                                            </ul>
                                        </Col>
                                    </Row>

                                </Container>

                            )}

                            <Container fluid>
                                <Row className="mb-0 pr-0 d-flex">

                                    <Col xs={12}>
                                        {!IsCurrentMember() && (
                                            params.id && <Button variant="danger" type="button" onClick={DeleteMemberPrompt}>Deletar cadastro</Button>)}

                                        <div className="float-end d-flex">
                                            {(!params.id ?
                                                <Form.Group className="pt-1 me-3">
                                                    <FormCheck name="register-more" id="register-more" type="checkbox" defaultChecked={registerMore} label="Cadastrar mais pessoas"
                                                        onChange={(event) => setRegisterMore(event.target.checked)} />
                                                </Form.Group > :
                                                '')}

                                            {(authContext.auth.user.role !== 'AppUser' || IsCurrentMember()) && (
                                                <Button variant="primary" className="ms-3" type="submit" onClick={SaveMember}>
                                                    Salvar {(params.id ? "alterações" : "cadastro")}
                                                </Button>
                                            )}
                                        </div>
                                    </Col>

                                </Row>
                            </Container>

                        </Form>
                    </>
                )}
        </>
    );
};