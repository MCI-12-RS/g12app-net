import React from 'react';
import { Tree, TreeNode } from 'react-organizational-chart';
import { GetMinistryChildren } from '../../services/Ministry.service';
import styled from 'styled-components';
import { Badges, Baptized, CellHost, CellLeader, CellTimothy } from '../../components/Badges';
import { FormatFirstLastName } from '../../shared/utils';

import './style.css';
import { Spinner } from 'react-bootstrap';
import { useAuthenticationContext } from '../../contexts/AuthenticationContext';

const StyledNode = styled.div`
    padding: 8px 15px;
    border-radius: 8px;
    display: inline-block;
    text-align: left;
    border: 1px solid #000;
`;

const NameNode = styled.div`
    font-size: 1.1em;
    margin-bottom: 8px;
    text-align: center;
`;

const InfoNode = styled.div`
    font-size: 0.9em;
`;

const TreeStyle = {
    lineWidth: '2px',
    lineColor: '#0099dbff',
    lineBorderRadius: '10px',
};

export const Ministries = () => {

    const [ministries, setMinistries] = React.useState([]);
    const [loading, setLoading] = React.useState(true);

    const authContext = useAuthenticationContext();

    let renderTree = (item) => (
        <Tree
            key={item.husband?.id || item.wife?.id}
            label={getTreeNodeBody(item)}
            {...TreeStyle}>
            {renderTreeChildren(item)}
        </Tree>);

    let renderTreeChildren = (parent) => {

        if(parent.loading) {
            return (<Spinner animation="border" />);
        }

        if(!Array.isArray(parent.children) || parent.children.length == 0)
            return null;

        return parent.children.map((item) => (
            <TreeNode key={item.husband?.id || item.wife?.id} label={getTreeNodeBody(item)}>
                {(item.children && item.children.length > 0) && renderTreeChildren(item.children)}
            </TreeNode>
        ));
    };

    const toggleChildren = (item) => {
        if (!item.children || item.children.length === 0) {
            let id = item.husband?.id || item.wife?.id;
            if (!id) return;
            item.loading = true;
            setMinistries([...ministries]);
            GetMinistryChildren(id).then(childrenResponse => {
                if (childrenResponse.data) {
                    item.children = childrenResponse.data;
                    if(item.children.length === 0) {
                        item.children = null;
                    }
                    
                    item.loading = false;
                    setMinistries([...ministries]);
                }
            });
        } else {
            item.children = null;
            setMinistries([...ministries]);
        }
    };

    // Initial load (Parent 0 and then their children)
    React.useEffect(() => {
        GetMinistryChildren(0).then(response => {
            let data = response.data;

            Promise.all(data.map(item => {
                return GetMinistryChildren(item.husband?.id || item.wife?.id).then(childrenResponse => {
                    if (childrenResponse.data)
                        item.children = childrenResponse.data;
                });
            })).then(() => {
                setMinistries(data);
                setLoading(false);
            });

        });
    }, []);

    let getTreeNodeBody = (item) => {
        let totalDisciples = (item.husband?.totalDisciples || 0) + (item.wife?.totalDisciples || 0);
        let totalCells = (item.husband?.totalCells || 0) + (item.wife?.totalCells || 0);
        return (
            <StyledNode className="tree-node" onClick={() => toggleChildren(item)} {...{ style: { cursor: (totalDisciples ? 'pointer' : 'default') } }}>
                <NameNode>
                    <strong>
                        {item.husband &&
                            <span>{FormatFirstLastName(item.husband.name)}</span>
                        }
                        {item.husband && item.wife && " e "}
                        {item.wife &&
                            <span>{FormatFirstLastName(item.wife.name)}</span>
                        }
                    </strong>
                </NameNode>
                <InfoNode>
                    <strong>Discípulos:</strong>&nbsp;
                    {item.husband && item.husband.totalDisciples}
                    {item.husband && item.husband.totalDisciples && (item.husband && item.husband.totalDisciples == 1 ? " homem" : " homens")}
                    {item.husband && item.husband.totalDisciples && item.wife && item.wife.totalDisciples && " e "}
                    {item.wife && item.wife.totalDisciples}
                    {item.wife && item.wife.totalDisciples && (item.wife && item.wife.totalDisciples == 1 ? " mulher" : " mulheres")}

                    {((!item.husband || !item.husband?.totalDisciples) && (!item.wife || !item.wife?.totalDisciples)) && "0 discípulos"}
                </InfoNode>
                <InfoNode>
                    <strong>Células:</strong>&nbsp;
                    {(totalCells)} {totalCells == 1 ? "célula" : "células"}

                </InfoNode>
                <Badges>
                    {((item.husband && item.husband.baptized) || (item.wife && item.wife.baptized)) && <Baptized />}
                    {((item.husband && item.husband.cellsLeadingCount > 0) || (item.wife && item.wife.cellsLeadingCount > 0)) && <CellLeader />}
                    {((item.husband && item.husband.cellsServingCount > 0) || (item.wife && item.wife.cellsServingCount > 0)) && <CellTimothy />}
                    {((item.husband && item.husband.cellsHostingCount > 0) || (item.wife && item.wife.cellsHostingCount > 0)) && <CellHost />}
                </Badges>
            </StyledNode>);
    };

    return (
        <>
            <div>
                {authContext.auth.user.role === 'AppSuperAdmin' && (<h1>Ministérios</h1>)}
                {authContext.auth.user.role !== 'AppSuperAdmin' && (<h1>Meu Ministério</h1>)}
                <hr />
                <div style={{ overflowX: 'auto', overflowY: 'none' }}>
                    {loading && (<Spinner animation="border" />)}
                    {!loading && ministries.length > 0 && ministries.map(m => renderTree(m))}
                </div>
            </div>
        </>
    );

};
