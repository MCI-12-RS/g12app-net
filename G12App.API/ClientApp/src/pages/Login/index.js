import React, { useState } from "react";
import { Button, Col, Container, InputGroup } from "react-bootstrap";
import { Input } from "reactstrap";

const [login, setLogin] = useState(emptyLogin);


export const LogPage = () => {

    const emptyLogin = {
        email: '',
        password: ''
    };

    const handleChangeEvent = (event) => {
        const { name, value } = event.target;
        setLogin ({... login, [name]: value });
    }

    <Container fluid>
        <h6>Sign In</h6>
        <Row className="mb-3 pr-0 justify-content-md-center">
            <Col>
            <InputGroup className="mb-3">
                <InputGroup.Text id="basic-addon1">E-mail</InputGroup.Text>
                <Form.Control name="email" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={login['email']} />
            </InputGroup>
            </Col>
        </Row>

        <Row className="mb-3 pr-0 justify-content-md-center">
            <Col>
            <InputGroup className="mb-3">
                <InputGroup.Text id="basic-addon1">Senha</InputGroup.Text>
                <Form.Control name="password" aria-describedby="basic-addon1" onChange={handleChangeEvent} value={login['password']} />
            </InputGroup>
            </Col>
        </Row>

        <Row className="mb-3 pr-0 d-flex">
            <div className="float-end d-flex">
                <Button variant="primary" className="ms-3" type="submit">
                    Login
                </Button>
            </div>
        </Row>
    </Container>
}