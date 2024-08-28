import React, { useEffect, useState } from "react";
import { Col, Container, Row } from "react-bootstrap";
import './style.css';
import { GetCommits } from "../../services/AppChanges.service";
import md5 from "md5";
import moment from "moment";

export const AppChanges = () => {

    const [commits, setCommits] = useState([]);

    const LoadCommits = async () => {
        var commits = await GetCommits() || [];
        setCommits(commits.map((commit) => {
            return {
                sha: commit.sha,
                message: commit.commit.message,
                date: moment(commit.commit.author.date).format('DD/MM/YYYY HH:mm'),
                author: commit.commit.author.name
            }
        }).filter(x => x.message.toLowerCase().substr(0, 5) !== 'merge'));
    };

    useEffect(() => {
        LoadCommits();
    }, []);

    return (
        <Container fluid>
            <p>As alterações listadas abaixo podem levar até 10 minutos para estarem disponíveis no aplicativo.</p>
            <br />
            <br />
            {commits && commits.map((commit, index) => {
                return (
                    <>
                        <hr style={{"border":"1px dashed #777"}} />
                        <Row key={index} className="commit">
                            <Col md={11} className="commit-info">
                                <h5>{commit.message}</h5>
                                <p>{commit.date} - {commit.author}</p>
                            </Col>
                        </Row>
                    </>
                );
            })}
        </Container>
    );
}