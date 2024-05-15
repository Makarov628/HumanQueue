import { useState } from 'react';

const useToken = () => {

    const getToken = () => {
        const userToken = sessionStorage.getItem('token');
        return userToken && userToken
    }

    const [token, setToken] = useState(getToken());


    const saveToken = (userToken: string | null) => {
        sessionStorage.setItem('token', userToken || '');
        setToken(userToken);
    };

    const parseJwt = (value: string | null) => {
        if (value === null)
            return null

        try {
            var base64Url = value.split('.')[1];
            var base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
            var jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
                return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
            }).join(''));
            return JSON.parse(jsonPayload);
        } catch (e) {
            return null;
        }
    };

    const verify = () => {
        let accessToken = getToken()

        if (accessToken === null) {
            return false;
        }

        const decodedJwt = parseJwt(accessToken);
        
        if (decodedJwt === null) {
            removeToken();
            return false;
        }

        if (decodedJwt.exp * 1000 < Date.now()) {
            removeToken();
            return false;
        }

        return true;
    }

    const isAdmin = () => {
        let accessToken = getToken()

        if (accessToken == 'admin') {
            return true
        }

        return false;
    }

    const removeToken = () => {
        sessionStorage.removeItem("token");
        setToken(null);
    }

    return {
        setToken: saveToken,
        token,
        removeToken,
        verify,
        parseJwt,
        isAdmin
    }
}

export default useToken;