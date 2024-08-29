import React from 'react';
import { Route, Routes } from 'react-router-dom';
import Layout from './components/Layout';
import Home from './Pages/Home';
import { AuthComponent } from './AuthContext';
import LogIn from './Pages/LogIn';
import LogOut from './Pages/LogOut';
import PrivateRoute from './components/PrivateRoute';
import SignUp from './Pages/SignUp';
import View from './Pages/View';
import AskQuestion from './Pages/AskQuestion';
const App = () => {
    return (
        <AuthComponent>
            <Layout>
                <Routes>
                    <Route path='/' element={<Home />} />
                    <Route path='/signup' element={<SignUp />} />
                    <Route path='/login' element={<LogIn />} />
                    <Route path='/askquestion' element={<PrivateRoute><AskQuestion /></PrivateRoute>} />
                    <Route path='/logout' element={<PrivateRoute><LogOut /></PrivateRoute>} />
                    <Route path='/view/:id' element={<View />} />
                </Routes>
            </Layout>
        </AuthComponent>
    );
}

export default App;