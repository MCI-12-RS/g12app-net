import React from "react";
import { Pagination } from "react-bootstrap";
import "./style.css";

const Grid = ({
    columns = [],
    data = [],
    loading = false,
    pagination = { page: 1, limit: 20 },
    setPagination = () => {},
    totalRecords = 0,
    textTotal = "registros"
}) => {

    const pages = Math.max(Math.ceil(totalRecords / pagination.limit), 1);

    const setPage = (page) => {
        setPagination({ ...pagination, page: page });
    };

    return (    
        
        loading ?

        <p class="loading"><em>Carregando...</em></p> :
        
        totalRecords == 0 ?

        <p class="no-records"><em>Nenhum registro encontrado.</em></p> :

        <div>   
            <table className="table table-striped" aria-labelledby="tableLabel">
                <thead>
                <tr key="columns">
                    {columns.map(column =>
                        <th>{column.name}</th>
                        )}
                </tr>
                </thead>
                <tbody>                
                    {data.map(member =>
                        (<tr key={member.id}>
                            {columns.map(column =>
                                <td>{column.render(member)}</td>
                            )}
                        </tr>)
                    )}
                </tbody>
            </table>
            
            <div class="pagination-container">
                <span class="total-records">{totalRecords} {textTotal}</span>
                <Pagination size="sm">
                    <Pagination.First onClick={() => setPage(1)} />
                    <Pagination.Prev onClick={() => setPage(pagination.page - 1)} />
                    <Pagination.Item active={pagination.page == 1} onClick={() => setPage(1)}>1</Pagination.Item>
                    {(() => {
                        var elements = [];
                        
                        if(pages == 1)
                            return;

                        var prevPage = pagination.page - 1;
                        var nextPage = pagination.page + 1;

                        if(prevPage > 1)
                            elements.push(<Pagination.Item onClick={() => setPage(prevPage)}>{prevPage}</Pagination.Item>);

                        if(pagination.page > 2)
                            elements.push(<Pagination.Item active onClick={() => setPage(pagination.page)}>{pagination.page}</Pagination.Item>);

                        if(nextPage < pages)
                            elements.push(<Pagination.Item onClick={() => setPage(nextPage)}>{nextPage}</Pagination.Item>);

                        if(nextPage + 1 < pages)
                            elements.push(<Pagination.Ellipsis />);

                        if(nextPage < pages)
                            elements.push(<Pagination.Item onClick={() => setPage(pages)}>{pages}</Pagination.Item>);

                        return elements;
                    })()}
                    <Pagination.Next onClick={() => setPage(pagination.page + 1)} />
                    <Pagination.Last onClick={() => setPage(pages)} />
                </Pagination>
            </div>
        
        </div>
    );
};

export default Grid;