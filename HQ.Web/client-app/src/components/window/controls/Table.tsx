import { State } from "../../enums/State"
import React from 'react';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import { Toolbar, Typography } from "@mui/material";
import { useTheme } from '@mui/material/styles';
import { useSnackbar } from 'notistack';
import * as api from '../../../api';

export interface ITabloTable {
    waitingRequests?: Array<api.RequestWaitingResponse>,
    calledRequests?: Array<api.RequestCalledResponse>,
    title: string
}

interface IRequest {
    id: number
    name: string
    state: number,
    window: string
}

const scale = {
    '@keyframes scaleAnimation': {
        '0%': {
            transform: 'scale(1)',
            opacity: '1'
        },
        '25%': {
            transform: 'scale(0.5)',
            opacity: '0.5'
        },
        '50%': {
            transform: 'scale(1)',
            opacity: '1'
        },
        '75%': {
            transform: 'scale(1)',
            opacity: '1'
        },
        '100%': {
            transform: 'scale(1)',
            opacity: '1'
        }
    },
}

export default function TabloTable({ ...props }: ITabloTable) {
    const theme = useTheme();
    const [requests, setRequests] = React.useState<IRequest[]>([]);

    return (
        <TableContainer >
            <Toolbar
                sx={{
                    pl: { sm: 2 },
                    pr: { xs: 1, sm: 1 },
                    textAlign: 'center'
                }}
            >
                <Typography
                    sx={{ flex: '1 1 100%', fontSize: '32px !important', fontWeight: '200' }}
                    variant="h6"
                    id="tableTitle"
                    component="div"
                >

                    {props.title}
                </Typography>
            </Toolbar>
            <Table sx={{ minWidth: 650 }} aria-label="simple table">
                <TableHead>
                    <TableRow>
                        {
                            props.waitingRequests ? 
                            <>
                                {/* <TableCell align="center"></TableCell> */}
                                {/* <TableCell  align="center" sx={{ fontSize: '25px' }}>Номер</TableCell> */}
                                {/* <TableCell align="center"></TableCell> */}
                            </>
                            : null
                        }
                        {
                            props.calledRequests ? 
                            <>
                                <TableCell align="center" sx={{ fontSize: '25px' }}>Номер</TableCell>
                                <TableCell align="center"></TableCell>
                                <TableCell align="center" sx={{ fontSize: '25px' }}>Окно</TableCell>

                            </>
                            : null
                        }
                    </TableRow>
                </TableHead>
                <TableBody>
                    {props.waitingRequests?.reduce((resultArray: api.WaitingRequestResponse[][], item, index) => {
                        const chunkIndex = Math.floor(index / 2)

                        if (!resultArray[chunkIndex]) {
                            resultArray[chunkIndex] = [] // start a new chunk
                        }

                        resultArray[chunkIndex].push(item)

                        return resultArray
                    }, []).map((cells: api.WaitingRequestResponse[], index: number) => (
                        <TableRow
                            key={`request_${index}`}
                            sx={{
                                '&:last-child td, &:last-child th': { border: 0 }
                            }}
                        >
                            {
                                cells.map(cell => (
                                    <TableCell align="center"
                                        sx={{
                                            padding: '8px 16px',
                                            color: theme.palette.primary.main,
                                            fontSize: '50px',
                                            scale
                                        }}>{cell.number}</TableCell>
                                ))
                            }

                        </TableRow>
                    ))}

                    {props.calledRequests?.map((request: api.RequestCalledResponse, index: number) => (
                        <TableRow
                            key={`request_${index}_${request.id}`}
                            sx={{
                                '&:last-child td, &:last-child th': { border: 0 }
                            }}
                        >
                            <TableCell align="center"
                                sx={{
                                    padding: '8px 16px',
                                    color: theme.palette.primary.main,
                                    fontSize: '50px',
                                    scale,
                                    animation: `scaleAnimation 1.5s 0.${index + 1}s infinite`,
                                }}>{request.number}</TableCell>
                            <TableCell align="center" sx={{ padding: '8px 16px' }}><ArrowForwardIcon
                                sx={{
                                    fontSize: '32px'
                                }} /></TableCell>
                            <TableCell align="center"
                                sx={{
                                    fontSize: '50px',
                                    padding: '8px 16px'
                                }}>{request.window?.number}</TableCell>
                        </TableRow>
                    ))}
                </TableBody>
            </Table>
        </TableContainer>
    )
}