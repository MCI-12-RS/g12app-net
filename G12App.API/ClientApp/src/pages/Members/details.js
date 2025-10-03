import React, { useState, useEffect } from "react";
import { Form, Col, Row, Button, Container, Spinner, FormCheck, InputGroup } from 'react-bootstrap';
import { useParams, useNavigate, useLocation } from "react-router-dom";
import { GetMember, CreateMember, UpdateMember, DeleteMember } from '../../services/Member.service';
import MemberPicker from '../../components/MemberPicker';
import { FromBRDate, ToBRDate, StringifyResponseErrors, FormatMemberPickerLabel, ValidateCPF, FormatFirstName } from '../../shared/utils';
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
            state: 'RS'
        },
        leadershipId: null,
        leadership: {
            id: null,
            name: ''
        },
        password: '',
        confirmPassword: '',
        role: 'AppUser',
        baptized: true,
        baptismDate: '',
        idle: false,
        notes: '',
        hasNoLeadership: false,
        cpf: null
    };

    const params = useParams();
    const location = useLocation();
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [member, setMember] = useState(emptyMember);
    const [errors, setErrors] = useState({});
    const [registerMore, setRegisterMore] = useState(location.state?.registerMore ?? false);
    const [canEdit, setCanEdit] = useState(false);

    useEffect(() => {
        if (location.state?.registerMore) {
            setRegisterMore(true);
        }
    }, []);

    useEffect(() => {
        fetchUser();
        setCanEdit((authContext.auth.user.role !== 'AppUser' || IsCurrentMember()));
    }, [params]);

    useEffect(() => {
        setCanEdit((authContext.auth.user.role !== 'AppUser' || IsCurrentMember()));
    }, [authContext.auth]);

    const formatPhoneNumber = (phoneNumber) => {
        if (!phoneNumber) return '';
        phoneNumber = phoneNumber.replace(/[^0-9]/g, '');
        return `(${phoneNumber.substring(0, 2)}) ${phoneNumber.substring(2, 7)}-${phoneNumber.substring(7, 11)}`;
    }


    const fetchUser = async () => {
        setLoading(true);
        if (!params.id) {
            setMember(emptyMember);
            setMember({ ...member, leadershipId: (authContext.auth.user.memberId > 0 ? authContext.auth.user.memberId : null) });
            setLoading(false);
            return;
        }

        var result = await GetMember(params.id);
        if (result.isValid && result.data != null) {
            const member = result.data;

            if (member.birthDate) {
                member.birthDate = ToBRDate(member.birthDate);
            }

            if (member.baptismDate) {
                member.baptismDate = ToBRDate(member.baptismDate);
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
        console.log(name, value);
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

    const ValidateForm = () => {
        let errors = {};

        if ((member.password || '').length)
            if (member.password !== member.confirmPassword) {
                errors.password = 'Senhas não conferem';
            }

        if (member.name.length < 3 || member.name.split(' ').length < 2)
            errors.name = 'Insira nome e sobrenome';

        if (member.email && (!member.email.includes('@') || !member.email.includes('.')))
            errors.email = 'Email inválido';

        if (!member.email && !member.mobileNumber) {
            errors.email = 'É necessário e-mail ou celular para que o usuário faça login';
            errors.mobileNumber = 'É necessário e-mail ou celular para que o usuário faça login';
        }

        if (!params.id && !member.password)
            errors.password = 'Defina uma senha inicial para que o usuário faça login';

        if (member.cpf && !ValidateCPF(member.cpf))
            errors.cpf = 'CPF inválido - caso não tenha CPF, deixe em branco';

        if (member.hasNoLeadership === false && !member.leadershipId && authContext.auth.user.role !== 'AppSuperAdmin')
            errors.leadershipId = 'Defina a liderança ou se a pessoa não tem liderança';

        setErrors(errors);

        if (Object.keys(errors).length > 0)
            toastContext.addToast({
                title: 'Erro', hideDelay: 5000,
                description: 'Verifique os erros no formulário para prosseguir o cadastro.'
            });

        return Object.keys(errors).length === 0;
    }

    const IsCurrentMember = () => {
        return parseInt(params?.id) === authContext.auth.user.memberId;
    }

    const GetMemberPickerLabel = (pickedMember, authUser) => {
        if (!params.id && authUser && authUser.memberId > 0) {
            var leader = (authUser.g12LeadershipName ?? '');
            if (leader) leader += ' / ';
            return (leader ?? '') + authUser.name;
        }

        return FormatMemberPickerLabel(pickedMember)
    };

    const GetMemberPickerId = (pickedMember, authUser) => {
        if (!params.id && authUser && authUser.memberId > 0) {
            return authUser.memberId;
        }

        return pickedMember?.id;
    };

    const SaveMember = async (e) => {
        e.preventDefault();

        if (!ValidateForm()) {
            e.stopPropagation();
            return;
        }

        setLoading(true);

        try {
            var result;

            var payload = {
                ...member,
                birthDate: FromBRDate(member.birthDate),
                baptismDate: FromBRDate(member.baptismDate),
                password: (member.password || '').length ? md5(member.password) : null,
                mobileNumber: (member.mobileNumber || '').replace(/[^0-9]/g, ''),
                phoneNumber: (member.phoneNumber || '').replace(/[^0-9]/g, ''),
                cpf: (member.cpf || '').replace(/[^0-9]/g, ''),
            };

            delete payload.leadership;
            delete payload.spouse;

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
                    navigate('/new-member', { state: { registerMore: true }, replace: false });
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
            {loading ?
                (<Spinner animation="border" />) : (
                    <>
                        {(params.id ?
                            authContext.auth.user.role === 'AppUser' ?
                                (IsCurrentMember() ? <h1>Meus Dados</h1> :
                                    <h1>Dados de {FormatFirstName(member?.name) || 'Pessoa'}</h1>) :
                                <h1>Alterar cadastro de pessoa</h1> :
                            <h1>Cadastrar nova pessoa</h1>)}
                        <hr />
                        <Form onSubmit={SaveMember}>
                            <Container fluid>
                                <h6>Dados pessoais</h6>
                                <Row className="mb-3 pr-3 justify-content-md-center">
                                    <Col xs={12} md={6}>
                                        <Form.Group className="mb-3">
                                            <Form.Label id="basic-addon1">Nome</Form.Label>
                                            <Form.Control name="name" aria-describedby="basic-addon1" onChange={handleChangeEvent}
                                                value={member['name']} disabled={!canEdit} />
                                            <Form.Control.Feedback type="invalid">
                                                {errors.name}
                                            </Form.Control.Feedback>
                                        </Form.Group >
                                    </Col>

                                    <Col xs={12} md={6}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label>Email</Form.Label>
                                            <Form.Control name="email" aria-describedby="basic-addon2" onChange={handleChangeEvent}
                                                value={member.email} disabled={!canEdit} />
                                            <Form.Control.Feedback type="invalid">
                                                {errors.email}
                                            </Form.Control.Feedback>
                                        </Form.Group >
                                    </Col>

                                </Row>

                                <Row className="mb-3 pr-3 justify-content-md-center">

                                    <Col xs={12} md={6} lg={4}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label>CPF</Form.Label>
                                            <InputMask mask="999.999.999-99" value={member['cpf']} onChange={handleChangeEvent} disabled={!canEdit}>
                                                <Form.Control name="cpf" aria-describedby="basic-addon2" />
                                            </InputMask>
                                            <Form.Control.Feedback type="invalid">
                                                {errors.cpf}
                                            </Form.Control.Feedback>
                                        </Form.Group >
                                    </Col>

                                    <Col xs={12} md={6} lg={4}>
                                        <MemberPicker label="Cônjuge"
                                            initialValue="Não possui"
                                            disabled={authContext.auth.user.role === 'AppUser'}
                                            setValue={(value) => handleSelectChangeEvent('spouseId', value)}
                                            notInIds={[member.id]}
                                            selectedLabel={GetMemberPickerLabel(member.spouse)}
                                            selectedValue={GetMemberPickerId(member.spouse)} />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.leadershipId}
                                        </Form.Control.Feedback>
                                    </Col>

                                    <Col xs={12} md={6} lg={4}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label>Sexo</Form.Label>
                                            <Form.Select name="gender" defaultValue={member.gender} aria-describedby="basic-addon2"
                                                onChange={handleChangeEvent} disabled={!canEdit}>
                                                <option value="Male">Masculino</option>
                                                <option value="Female">Feminino</option>
                                            </Form.Select>
                                        </Form.Group >
                                    </Col>
                                </Row>

                                <Row className="mb-3 pr-3 justify-content-md-center">

                                    <Col xs={12} md={6} lg={4}>

                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label>Data de Nascimento</Form.Label>
                                            <InputGroup>
                                                <InputMask mask="99/99/9999" value={member['birthDate']} onChange={handleChangeEvent} disabled={!canEdit}>
                                                    <Form.Control type="text" name="birthDate"
                                                        aria-describedby="basic-addon2" />
                                                </InputMask>
                                                {getAge() && (
                                                    <InputGroup.Text>
                                                        {getAge() + ' anos'}
                                                    </InputGroup.Text>
                                                )}
                                            </InputGroup>
                                        </Form.Group>
                                    </Col>

                                    <Col xs={12} md={6} lg={4}>
                                        <Form.Group className="mb-3 pr-0">
                                            <Form.Label>Celular / Whatsapp</Form.Label>
                                            <InputMask mask="(99) 99999-9999" value={member['mobileNumber']} onChange={handleChangeEvent} disabled={!canEdit}>
                                                <Form.Control name="mobileNumber" aria-describedby="basic-addon2" />
                                            </InputMask>
                                            <Form.Control.Feedback type="invalid">
                                                {errors.mobileNumber}
                                            </Form.Control.Feedback>
                                        </Form.Group >
                                    </Col>

                                    <Col xs={12} md={6} lg={4}>
                                        <Form.Group className="mb-0">
                                            <Form.Label id="basic-addon1">Telefone</Form.Label>
                                            <InputMask mask="(99) 9999-9999" value={member['phoneNumber']} onChange={handleChangeEvent} disabled={!canEdit}>
                                                <Form.Control name="phoneNumber" aria-describedby="basic-addon1" />
                                            </InputMask>
                                        </Form.Group >
                                    </Col>
                                </Row>

                                <Row className="mb-0 pr-3 justify-content-md-center">
                                    <Col>
                                        <Form.Group>
                                            <Form.Label>Observações</Form.Label>
                                            <Form.Control as="textarea" aria-label="Observações" name="notes" value={member.notes} aria-describedby="basic-addon3"
                                                autoComplete='off' onChange={handleChangeEvent} disabled={!canEdit} />
                                            <Form.Control.Feedback type="invalid">
                                                {errors.notes}
                                            </Form.Control.Feedback>
                                        </Form.Group>
                                    </Col>
                                </Row>
                            </Container>

                            <Container fluid>
                                <h6>Ministerial</h6>

                                <Row className="mb-3 pr-0">
                                    <Col>
                                        <Form.Group className="mt-1">
                                            <FormCheck type="checkbox" id="hasNoLeadership" name="hasNoLeadership"
                                                defaultChecked={member.hasNoLeadership} value={member.hasNoLeadership} label="Não tem liderança"
                                                disabled={!canEdit || authContext.auth.user.role === 'AppUser'}
                                                onChange={(event) => handleChangeEvent({ target: { name: 'hasNoLeadership', value: event.target.checked } })} />
                                        </Form.Group>
                                    </Col>
                                </Row>

                                <Row className="mb-3 pr-0 justify-content-md-center">
                                    <Col>
                                        <MemberPicker label="Liderança"
                                            disabled={member.hasNoLeadership || !canEdit || authContext.auth.user.role === 'AppUser'}
                                            disableDefaultOption={authContext.auth.user.role !== 'AppSuperAdmin'}
                                            setValue={(value) => handleSelectChangeEvent('leadershipId', value)}
                                            notInIds={[member.id]}
                                            selectedLabel={GetMemberPickerLabel(member.leadership, authContext.auth.user)}
                                            selectedValue={GetMemberPickerId(member.leadership, authContext.auth.user)} />
                                        <Form.Control.Feedback type="invalid">
                                            {errors.leadershipId}
                                        </Form.Control.Feedback>
                                    </Col>
                                </Row>

                                <Row className="mb-0 pr-0 justify-content-md-center">

                                    <Col xs={3} md={2} className="mt-4">
                                        <Form.Group>
                                            <FormCheck type="checkbox" id="baptized" name="baptized"
                                                defaultChecked={member.baptized} value={member.baptized} label="Batizado" disabled={!canEdit || authContext.auth.user.role === 'AppUser'}
                                                onChange={(event) => handleChangeEvent({ target: { name: 'baptized', value: event.target.checked } })} />
                                        </Form.Group>
                                    </Col>

                                    <Col xs={9} md={10}>
                                        <Form.Group>
                                            <Form.Label>Data de Batismo:</Form.Label>
                                            <InputMask mask="99/99/9999" value={member.baptismDate} onChange={handleChangeEvent} disabled={!member.baptized || !canEdit || authContext.auth.user.role === 'AppUser'}>
                                                <Form.Control type="text" name="baptismDate" aria-describedby="basic-addon2" />
                                            </InputMask>
                                            <Form.Control.Feedback type="invalid">
                                                {errors.dateOnly}
                                            </Form.Control.Feedback>
                                        </Form.Group>
                                    </Col>
                                </Row>

                                {authContext.auth.user.role !== 'AppUser' && (
                                    <Row className="mb-0 pr-0 justify-content-md-center">
                                        <Col xs={12} md={12} className="mt-4">
                                            <Form.Group>
                                                <FormCheck type="checkbox" id="idle" name="idle"
                                                    defaultChecked={member.idle} value={member.idle} label="Inativo" disabled={!canEdit}
                                                    onChange={(event) => handleChangeEvent({ target: { name: 'idle', value: event.target.checked } })} />
                                            </Form.Group>
                                        </Col>
                                    </Row>
                                )}

                            </Container>

                            <AddressDetails address={member.homeAddress} disabled={!canEdit} setAddress={(address) => handleSelectChangeEvent('homeAddress', address)} />

                            {canEdit && (

                                <Container fluid>
                                    <h6>Acesso</h6>
                                    <Row className="mb-3 pr-0 justify-content-md-center">
                                        <Col xs={12} md={6}>
                                            <Form.Group className="mb-3">
                                                <Form.Label id="basic-addon1">Senha</Form.Label>
                                                <Form.Control type="password" autoComplete="new-password" name="password" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={member['password']} />
                                                <Form.Control.Feedback type="invalid">
                                                    {errors.password}
                                                </Form.Control.Feedback>
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
                                                <Form.Label>Função</Form.Label>
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

                            {canEdit && (
                                <Container fluid>
                                    <Row className="mb-0 pr-0 d-flex">

                                        <Col xs={12}>
                                            {(!IsCurrentMember() && authContext.auth.user.role !== 'AppUser') && (
                                                params.id && <Button variant="danger" type="button" onClick={DeleteMemberPrompt}>Deletar cadastro</Button>)}

                                            <div className="float-end d-flex">
                                                {(!params.id ?
                                                    <Form.Group className="pt-1 me-3">
                                                        <FormCheck name="register-more" id="register-more" type="checkbox" defaultChecked={registerMore} label="Cadastrar mais pessoas"
                                                            onChange={(event) => setRegisterMore(event.target.checked)} />
                                                    </Form.Group > :
                                                    '')}

                                                {(authContext.auth.user.role !== 'AppUser' || IsCurrentMember()) && (
                                                    <Button variant="primary" className="ms-3" type="submit">
                                                        Salvar {(params.id ? "alterações" : "cadastro")}
                                                    </Button>
                                                )}
                                            </div>
                                        </Col>

                                    </Row>
                                </Container>
                            )}

                        </Form>
                    </>
                )}
        </>
    );
};