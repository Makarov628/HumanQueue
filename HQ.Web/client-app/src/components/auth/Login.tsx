import { Typography } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import React, { useState } from 'react';
import TextField from '@mui/material/TextField';
import Stack from '@mui/material/Stack';
import Button from '@mui/material/Button';
import useToken from '../common/Token';


const Login = () => {
    const navigate = useNavigate();

    const { token, removeToken, setToken } = useToken();
    const [userName, setUserName] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [error, setError] = useState<string | null>(null);

    const handleKeyDown = (key: string) => {
        if (key === 'Enter') {
            sign();
        }
    }

    const sign = () => {
        if ((userName == "") && (password == "")) {
            setError('Неверная пара логин/пароль')
            return;
        }
        else {
            let data = new FormData();
            data.append('username', userName);
            data.append('password', password);
            
            setToken(userName);
            
            window.location.reload();
        }
    }

    return (
        <div id="fullPage">
            <div id="brandingWrapper" className="float">
                <div id="branding" style={{ backgroundImage: "url('/images/illustration.png')" }}></div>
            </div>
            <div id="contentWrapper" className='float'>
                <div id="content">
                    <div id="header">
                        <Typography variant='h4'>HQ</Typography>
                    </div>
                    <div id="workArea">
                        <div style={{ marginBottom: '30px' }}>
                            <div>
                                <div style={{ marginBottom: '30px' }}>
                                    <Typography variant='body1'>
                                        Выполнить вход, используя учетную запись организации.
                                    </Typography>
                                </div>
                                <div id="error" className="fieldMargin error smallText" style={{ display: error == null ? 'none' : 'block' }}>
                                    <Typography variant='caption' id="errorText">{error}</Typography>
                                </div>
                                <div>
                                    <Stack spacing={1}>
                                        <TextField
                                            required
                                            id="outlined-required"
                                            label="Имя пользователя"
                                            onChange={(e) => setUserName(e.target.value)}
                                            onKeyDown={(e) => handleKeyDown(e.key)}
                                        />
                                        <TextField
                                            required
                                            id="outlined-password-input"
                                            label="Пароль"
                                            type="password"
                                            autoComplete="current-password"
                                            onKeyDown={(e) => handleKeyDown(e.key)}
                                            onChange={(e) => setPassword(e.target.value)}
                                        />
                                    </Stack>
                                    <div className="submitMargin">
                                        <Button
                                            onClick={() => sign()}
                                            variant="contained">
                                            Вход
                                        </Button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div id="footer">
                    <div id="footerLinks">
                        <Typography variant='caption'>ВКТУ им. Д.Серикбаева</Typography>
                    </div>
                </div>
            </div>
        </div>
    )
}

export default Login