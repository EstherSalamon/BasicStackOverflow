import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Home = () => {

    const [questions, setQuestions] = useState([]);
    const [searchText, setSearchText] = useState('');
    const [filteredQuestions, setFilteredQuestions] = useState([]);
    const [tags, setTags] = useState([]);

    useEffect(() => {

        const loadData = async () => {
            const { data } = await axios.get('/api/questions/getall');
            setQuestions(data);
            setFilteredQuestions(data);
            loadTags();
        };

        const loadTags = async () => {
            const { data } = await axios.get('/api/questions/gettags');
            setTags(data);
        };

        loadData();

    }, []);

    const onSearchChange = async (e) => {
        setSearchText(e.target.value);
        const { data } = await axios.get(`/api/questions/searchbytag?tagId=${e.target.value}`);
        setFilteredQuestions(data);
    };

    const onClearClick = () => {
        setFilteredQuestions(questions);
        setSearchText('');
    };

    function getGuid() {
        return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
            (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
        );
    }

    return (
        <div className='container' style={{ marginTop: 80 }}>
            <div className='col-md-10 offset-1'>
                <h1>These are the most awesome questions</h1>
                <hr />
                <div className='col-md-4 offset-8'>
                    <div className='input-group'>
                        <select className='form-control' value={searchText} onChange={e => onSearchChange(e)}>
                            <option value='-1'>Search</option>
                            {tags && tags.map(t =>
                                <option key={t.id} value={t.id}>{t.name}</option>)}
                        </select>
                        <button className='btn btn-primary' onClick={onClearClick}>Clear</button>
                    </div>
                    <br/>
                </div>
                <div className='row'>
                {filteredQuestions && filteredQuestions.map(q =>
                    <div key={getGuid()} className='card col-6'>
                        <div className='card-body'>
                            <a href={`/view/${q.Id}`}>
                                <h3 className='card-title'>{q.Title}</h3>
                            </a>    
                            <h6>Tags: {q.JoinTags && q.JoinTags.map(jt =>
                                <span key={getGuid()} className='badge text-bg-primary' style={{marginRight: 2}}>{jt.Tag.Name}</span>)}</h6>
                            <p className='card-text'>{q.QuestionText}</p>
                            <h6>{q.Answers ? q.Answers.length : 0} answer(s)</h6>
                        </div>
                    </div>)}

                </div>
            </div>
        </div>
    )

};

export default Home;