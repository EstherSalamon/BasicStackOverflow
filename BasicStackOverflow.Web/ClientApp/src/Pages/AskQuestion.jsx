import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import { useAuthentication } from '../AuthContext';

const AskQuestion = () => {

    const [question, setQuestion] = useState({
        title: '',
        questionText: '',
        tags: []
    });
    const [tag, setTag] = useState([]);
    const { user } = useAuthentication();
    const navigate = useNavigate();

    const onTextChange = e => {
        const copy = { ...question };
        copy[e.target.name] = e.target.value;
        setQuestion(copy);
    };

    const onButtonClick = async () => {
        const copy = { ...question };
        copy.userId = user.id;
        await axios.post('/api/questions/add', copy);
        navigate('/');
    };

    const onTagsChange = e => {
        setTag(e.target.value);
    };

    const onTabKeyDown = e => {
        if (e.key === "Tab" || e.key === 'Enter') {
            e.preventDefault();
            const tagsCopy = [...question.tags];
            tagsCopy.push(tag);
            const copy = { ...question };
            copy.tags = tagsCopy;
            setQuestion(copy);
            setTag('');
        }
    };

    const onTagDeleteClick = index => {
        const copy = [...question.tags];
        copy.splice(index, 1);
        const copyQ = { ...question };
        copyQ.tags = copy;
        setQuestion(copyQ);
    };

    return (
        <div className='container' style={{ marginTop: 80 }}>
            <div className='col-md-6 offset-3'>
                <h2>What pressing matter concerns you at the moment?</h2>
                <hr />
                <input type='text' name='title' placeholder='Title' value={question.title} onChange={e => onTextChange(e)} className='form-control' />
                <textarea className='form-control mt-2' name='questionText' value={question.questionText} onChange={e => onTextChange(e)} rows='6' placeholder="So... Let's hear the details"></textarea>
                <input type='text' name='tag' placeholder='Enter tag name here. Click tab or enter to continue.' value={tag} onChange={e => onTagsChange(e)} className='form-control mt-2' onKeyDown={onTabKeyDown} />
                {question.tags && question.tags.map((t, index) =>
                    <span className="badge text-bg-primary mt-2 h-75" style={{ marginRight: 2 }} key={index}>
                        {t} 
                        <button onClick={_ => onTagDeleteClick(index)} className='btn btn-primary' style={{paddingTop: 2}}>X</button>
                    </span>)}
                <button className='btn btn-secondary mt-2 w-100' onClick={onButtonClick}>Submit</button>
            </div>
        </div>
    )

};

export default AskQuestion;