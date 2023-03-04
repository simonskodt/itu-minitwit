import './Login.css';
import React, { useState } from 'react';
import { AppService } from '../services/app.service';
import { useNavigate } from 'react-router-dom';
import Header from './Header';
import Footer from './Footer';

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const appService = new AppService();

    const navigate = useNavigate();
    
    const goToHome= () => {
      navigate('/'); 
    };

    const submit = (e: React.FormEvent) => {
        let promise = appService.Login(username, password);
        promise.catch( () => alert("Wrong credentials"))
        promise.then(goToHome)
    };

    return (
      <div className="page">
        <Header isLoggedIn = {false}/>
          <div className="body">
            <div className='login-form'>
            <h2>Sign In</h2>
              <label htmlFor="username">Username</label>
              <input
                type="text"
                placeholder="Username"
                name="username"
                required
                onChange={e => setUsername(e.target.value)}
              />
              <label htmlFor="password">Password</label>
              <input
                type="password"
                placeholder="Password"
                name="password"
                required
                onChange={e => setPassword(e.target.value)}
              />
              <button onClick={submit}>Login</button>
            </div>
          </div>
        <Footer />
     </div>
    );
  }

  export default Login;