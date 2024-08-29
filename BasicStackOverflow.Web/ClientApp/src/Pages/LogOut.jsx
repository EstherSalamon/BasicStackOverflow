import { useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useAuthentication } from '../AuthContext';
import axios from 'axios';

const LogOut = () => {

    const navigate = useNavigate();
    const { setUser } = useAuthentication();

    useEffect(() => {

        const logOut = async () => {
            await axios.post('/api/users/logout');
            setUser(null);
            navigate('/');
        };

        logOut();

    }, []);

    return (<></>);
};

export default LogOut;