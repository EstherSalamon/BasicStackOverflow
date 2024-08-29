import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useAuthentication } from '../AuthContext';
import '../Loader.css';

const LogIn = () => {

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const { setUser } = useAuthentication();
    const [validLogin, setValidLogin] = useState(true);
    const [processing, setProcessing] = useState(false);

    const onButtonClick = async () => {
        setProcessing(true);
        const { data } = await axios.post('/api/users/login', { email, password });
        const validLogin = data;
        setValidLogin(validLogin);
        if (validLogin) {
            setUser(data);
            navigate('/');
        }
        setProcessing(false);
    };

    return (
        <div className='container' style={{ marginTop: 80 }}>
            <div className='col-md-6 offset-3'>
                <h2>Log in to your account</h2>
                <hr />
                {!validLogin && <h4 style={{textDecorationColor: 'red'}}>Invalid login. Please try again.</h4>}
                <input type='email' name='email' placeholder='Email' value={email} className='form-control mt-2' onChange={e => setEmail(e.target.value)} />
                <input type='password' name='password' placeholder='Password' value={password} className='form-control mt-2' onChange={e => setPassword(e.target.value)} />
                <a href='/signup'>Don't have an account? Click here to sign up</a>
                <button className='btn btn-outline-dark mt-2 w-100' onClick={onButtonClick}>{processing ? <span className='loader'></span> : 'Submit'}</button>
            </div>
        </div>
    )
};

export default LogIn;