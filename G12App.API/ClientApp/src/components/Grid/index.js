import React from "react";
import { Col, Container, Pagination, Row, Spinner } from 'react-bootstrap'
import "./style.css";

const Grid = ({
    columns = [],
    data = [],
    loading = false,
    pagination = { page: 1, limit: 10 },
    setPagination = () => { },
    totalRecords = 0,
    textTotal = "registros",
    displayPagination = true
}) => {

    const pages = Math.max(Math.ceil(totalRecords / pagination.limit), 1);

    const setPage = (page) => {
        setPagination({ ...pagination, page: page });
    };

    return (

        loading ?

            <Spinner animation="border" /> :

            totalRecords === 0 ?

                <p className="no-records"><em>Nenhum registro encontrado.</em></p> :

                <div>
                    <Container className="table grid">
                        <Row className="grid-header d-none d-md-flex">
                            {columns.map(column =>
                                <Col key={column.name} className={"col " + (column?.className ?? '')} {...column.properties}>{column.name}</Col>
                            )}
                        </Row>
                        {data.map((item, index) =>
                        (<Row key={item.id} className={"grid-item d-block d-md-flex " + ((index % 2) === 0 ? 'even' : 'odd')} >
                            {columns.map((column) =>
                                <>
                                    <div key={column.name + '-' + item.id} 
                                        className={"d-block d-md-none " + (column?.className ?? '') + (column.classFunction ? column.classFunction(item) : '')} 
                                        {...column.properties}>
                                        <div className="inside-column-name d-md-none">{column.name}</div>
                                        <div className="inside-column-value d-md-none">{column.render(item, index)}</div>
                                    </div>

                                    <Col key={column.name + '-' + item.id} 
                                        className={"align-middle col d-none d-md-block " + (column?.className ?? '') + (column.classFunction ? column.classFunction(item) : '')} 
                                        {...column.properties}>
                                        <div className="grid-cell-content d-none d-md-inline-flex">{column.render(item, index)}</div>
                                    </Col>
                                </>
                            )}
                        </Row>)
                        )}
                    </Container>

                    {(displayPagination &&
                        <div className="pagination-container">
                            <span className="total-records">{totalRecords} {textTotal}</span>
                            <Pagination size="sm">
                                {(() => {
                                    var elements = [];

                                    if (pagination.page > 1)
                                        elements.push(<Pagination.First key="first" onClick={() => setPage(1)} />);

                                    if (pagination.page > 2)
                                        elements.push(<Pagination.Prev key="prev" onClick={() => setPage(pagination.page - 1)} />);

                                    elements.push(<Pagination.Item key="item-1" active={pagination.page === 1} onClick={() => setPage(1)}>1</Pagination.Item>);

                                    if (pages === 1)
                                        return elements;

                                    var prevPage = pagination.page - 1;
                                    var nextPage = pagination.page + 1;

                                    if (prevPage > 2)
                                        elements.push(<Pagination.Ellipsis key="ellipsis-1" />);

                                    if (prevPage > 1)
                                        elements.push(<Pagination.Item key={"item-" + prevPage} onClick={() => setPage(prevPage)}>{prevPage}</Pagination.Item>);

                                    if (pagination.page >= 2)
                                        elements.push(<Pagination.Item key="item-active" active onClick={() => setPage(pagination.page)}>{pagination.page}</Pagination.Item>);

                                    if (nextPage <= pages)
                                        elements.push(<Pagination.Item key={"item-" + nextPage} onClick={() => setPage(nextPage)}>{nextPage}</Pagination.Item>);

                                    if (nextPage + 1 < pages)
                                        elements.push(<Pagination.Ellipsis key="ellipsis-2" />);

                                    if (nextPage < pages)
                                        elements.push(<Pagination.Item key={"item-" + pages} onClick={() => setPage(pages)}>{pages}</Pagination.Item>);

                                    if (pagination.page < pages) {
                                        elements.push(<Pagination.Next key="item-next" onClick={() => setPage(pagination.page + 1)} />);
                                        elements.push(<Pagination.Last key="item-last" onClick={() => setPage(pages)} />);
                                    }

                                    return elements;
                                })()}
                            </Pagination>
                        </div>
                    )}

                </div>
    );
};

export default Grid;