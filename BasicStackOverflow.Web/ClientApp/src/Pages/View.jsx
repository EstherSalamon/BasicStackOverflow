import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { useAuthentication } from '../AuthContext';

const View = () => {

    const { id } = useParams();
    const [question, setQuestion] = useState({});
    const navigate = useNavigate();
    const { user } = useAuthentication();
    const [answerText, setAnswerText] = useState('');
    const [processing, setProcessing] = useState(false);

    const loadData = async () => {
        const { data } = await axios.get(`/api/questions/byid?id=${id}`);
        if (!data) {
            navigate('/');
        }
        setQuestion(data);
        console.log(data);
    }

    useEffect(() => {

        loadData();

    }, []);

    function formatDate(string) {
        var options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(string).toLocaleDateString([], options);
    };

    const onAnswerClick = async () => {
        setProcessing(true);
        const answer = {
            questionId: question.Id,
            userId: user.id,
            answerText
        };
        await axios.post('/api/questions/answer', { answer });
        setAnswerText('');
        setProcessing(false);
        loadData();
    };

    return (
        <div className='container' style={{ marginTop: 80 }}>
            <div className='col-md-8 offset-2'>
                <div className='card'>
                    <div className='card-body'>
                        <h2>{question.Title}</h2>
                        <hr/>
                        <h6>Asked by {question.User ? question.User.Name : ''} on {formatDate(question.DatePosted)}</h6>
                        <hr />
                        <p>{question.QuestionText}</p>
                        <hr />
                        <h5>Tags: {question.JoinTags && question.JoinTags.map(jt =>
                            <span key={jt.Tag.id} className='badge text-bg-primary' style={{ marginRight: 2 }}>{jt.Tag.Name}</span>)}</h5>
                    </div>
                </div>
                <br />
                <h2>Answers:</h2>
                {question.Answers ? question.Answers.map(a =>
                    <div className='card' key={a.id}>
                        <div className='card-body'>
                            <h4>{a.AnswerText}</h4>
                            <p>Answered by {a.User.Name}</p>
                            <p>On {formatDate(a.DatePosted)}</p>
                    </div>
                </div>) : <h6>No answers yet, but feel free to share your thoughts!</h6> }
                <br />
                {!user ? <a href='/login'><h4>Log in to share your answer</h4></a> : <div className='card'>
                    <div className='card-body bg-light'>
                        <h3>Submit your answer here</h3>
                        <br />
                        <textarea name='answerText' value={answerText} onChange={e => setAnswerText(e.target.value)} rows='10' className='form-control'></textarea>
                        <button className='btn btn-primary w-100 mt-2' onClick={onAnswerClick}>{processing ? 'Processing...' : 'Submit'}</button>
                    </div>
                </div>}
            </div>
        </div>
    )
};

export default View;