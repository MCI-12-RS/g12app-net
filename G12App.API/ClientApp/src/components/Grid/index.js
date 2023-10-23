import React from "react";
import { Pagination, Spinner } from "react-bootstrap";
import "./style.css";

const Grid = ({
    columns = [],
    data = [],
    loading = false,
    pagination = { page: 1, limit: 10 },
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

        <Spinner animation="border" /> :
        
        totalRecords == 0 ?

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
                    {data.map(member =>
                        (<tr key={member.id}>
                            {columns.map(column =>
                                <td key={column.name + '-' + member.id}>{column.render(member)}</td>
                            )}
                        </tr>)
                    )}
                </tbody>
            </table>
            
            <div className="pagination-container">
                <span className="total-records">{totalRecords} {textTotal}</span>
                <Pagination size="sm">
                    {(() => {
                        var elements = [];

                        if(pagination.page > 1)
                            elements.push(<Pagination.First onClick={() => setPage(1)} />);

                        if(pagination.page > 2)
                            elements.push(<Pagination.Prev onClick={() => setPage(pagination.page - 1)} />);

                        elements.push(<Pagination.Item active={pagination.page == 1} onClick={() => setPage(1)}>1</Pagination.Item>);
                        
                        if(pages == 1)
                            return elements;

                        var prevPage = pagination.page - 1;
                        var nextPage = pagination.page + 1;

                        if(prevPage > 2)
                            elements.push(<Pagination.Ellipsis />);
                            
                        if(prevPage > 1)
                            elements.push(<Pagination.Item onClick={() => setPage(prevPage)}>{prevPage}</Pagination.Item>);
                        
                        if(pagination.page >= 2)
                            elements.push(<Pagination.Item active onClick={() => setPage(pagination.page)}>{pagination.page}</Pagination.Item>);

                        if(nextPage <= pages)
                            elements.push(<Pagination.Item onClick={() => setPage(nextPage)}>{nextPage}</Pagination.Item>);

                        if(nextPage + 1 < pages)
                            elements.push(<Pagination.Ellipsis />);

                        if(nextPage < pages)
                            elements.push(<Pagination.Item onClick={() => setPage(pages)}>{pages}</Pagination.Item>);

                        if(pagination.page < pages){
                            elements.push(<Pagination.Next onClick={() => setPage(pagination.page + 1)} />);
                            elements.push(<Pagination.Last onClick={() => setPage(pages)} />);
                        }

                        return elements;
                    })()}
                </Pagination>
            </div>
        
        </div>
    );
};

export default Grid;