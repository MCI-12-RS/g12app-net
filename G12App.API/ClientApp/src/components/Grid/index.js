import React from "react";
import { Pagination, Spinner } from "react-bootstrap";
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
                    <table className="table table-striped" aria-labelledby="tableLabel">
                        <thead>
                            <tr key="columns">
                                {columns.map(column =>
                                    <th key={column.name}>{column.name}</th>
                                )}
                            </tr>
                        </thead>
                        <tbody>
                            {data.map((item, index) =>
                            (<tr key={item.id}>
                                {columns.map(column =>
                                    <td key={column.name + '-' + item.id}>{column.render(item, index)}</td>
                                )}
                            </tr>)
                            )}
                        </tbody>
                    </table>

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