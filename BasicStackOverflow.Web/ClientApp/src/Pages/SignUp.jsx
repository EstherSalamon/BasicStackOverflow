import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const SignUp = () => {

    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');
    const navigate = useNavigate();
    const [message, setMessage] = useState('');

    const onButtonClick = async () => {
        const { data } = await axios.get(`/api/users/emailexists?email=${email}`);
        if (data === true) {
            setMessage('Email already taken. Please sign up with a different one.')
        } else {
            await axios.post('/api/users/signup', { User: { name, email }, password });
            navigate('/login');
        }
    };

    return (
        <div className='container' style={{ marginTop: 80 }}>
            <div className='col-md-6 offset-3'>
                <h2>Sign up for a new account</h2>
                <hr />
                <h6 className='text-danger'>{message}</h6>
                <input type='text' name='name' placeholder='Name' value={name} className='form-control mt-2' onChange={e => setName(e.target.value)} />
                <input type='email' name='email' placeholder='Email' value={email} className='form-control mt-2' onChange={e => setEmail(e.target.value)} />
                <input type='password' name='password' placeholder='Password' value={password} className='form-control mt-2' onChange={e => setPassword(e.target.value)} />
                <button className='btn btn-outline-danger mt-2 w-100' disabled={!(name && email && password)} onClick={onButtonClick}>Submit</button>
            </div>
        </div>
    )

};

export default SignUp;