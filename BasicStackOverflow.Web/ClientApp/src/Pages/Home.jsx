import React, { useState, useEffect } from 'react';
import axios from 'axios';

const Home = () => {

    const [questions, setQuestions] = useState([]);

    useEffect(() => {

        const loadData = async () => {
            const { data } = await axios.get('/api/questions/getall');
            console.log(data);
            setQuestions(data);
        };

        loadData();

    }, []);

    return (
        <div className='container' style={{ marginTop: 80 }}>
            <div className='col-md-8 offset-2'>
                <h1>These are the most awesome questions</h1>
                <hr />
                {questions && questions.map(q =>
                    <div className='card' key={q.Id}>
                        <div className='card-body'>
                            <a href={`/view/${q.Id}`}>
                                <h3 className='card-title'>{q.Title}</h3>
                            </a>    
                            <h6>Tags: {q.JoinTags && q.JoinTags.map(jt =>
                                <span key={jt.Tag.id} className='badge text-bg-primary' style={{marginRight: 2} }>{jt.Tag.Name}</span>)}</h6>
                            <p className='card-text'>{q.QuestionText}</p>
                            <h6>{q.Answers ? q.Answers.length : 0} answer(s)</h6>
                        </div>
                    </div>)}
            </div>
        </div>
    )

};

export default Home;

/*
<div className='container' style={{ marginTop: 80 }}>
<div className='col-md-8 offset-2'>
<h1>These are all available questions</h1>
<hr />
<table className='table table-striped table-hover'>
<thead>
<tr>
<th>Id</th>
<th>Title</th>
<th>Tags</th>
<th>Answers</th>
<th>Name</th>
</tr>
</thead>
<tbody>
{questions && questions.map(q => {
<tr key={q.id}>
    <td>{q.id}</td>
    <td>{q.title}</td>
    <td>{q.JoinTags.length}</td>
    <td>{q.Answers.length}</td>
    <td>{q.User.Name}</td>
</tr>
})}
</tbody>
</table>
</div>
</div>*/