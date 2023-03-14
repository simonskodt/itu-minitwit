import React, { useState } from 'react';
import { AppService } from '../services/app.service';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import Footer from './Footer';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');

  const appService = new AppService();

  const navigate = useNavigate();

  const goToHome = () => {
    navigate('/');
  };

  const submit = (e: React.FormEvent) => {
    let promise = appService.Login(username, password);
    promise.catch(() => {
      alert("Wrong credentials")
      sessionStorage.setItem('isLoggedIn', 'false')}
    )
    sessionStorage.setItem('isLoggedIn', 'true')
    promise.then((result) => {
      console.log(result)
      sessionStorage.setItem('username',result.data.username)
      goToHome()
    })
  };

  return (
    <div className="page">
      <Header isLoggedIn={false} />
      <div className="body">
        <div className='login-form'>
          <h2>Sign In</h2>
          <label htmlFor="username">Username </label><br />
          <input
            className="text-field"
            type="text"
            placeholder="Username"
            name="username"
            required
            onChange={e => setUsername(e.target.value)}
          /><br />
          <label htmlFor="password">Password </label><br />
          <input
            className="text-field"
            type="password"
            placeholder="Password"
            name="password"
            required
            onChange={e => setPassword(e.target.value)}
          /><br />
          <button
            onClick={submit}
            type="submit"
          >Login</button>
        </div>
      </div>
      <Footer />
    </div>
  );
}

export default Login;