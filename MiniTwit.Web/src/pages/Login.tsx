import React, { useState } from 'react';
import { AppService } from '../services/app.service';
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import Footer from './Footer';

const Login = () => {
  const [username, setUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState(null);

  const appService = new AppService();

  const navigate = useNavigate();

  const submit = (e: React.FormEvent) => {
    e.preventDefault()
    appService.Login(username, password)
      .then(user => {
        setError(null)
        sessionStorage.setItem('username', user.username)
        sessionStorage.setItem('isLoggedIn', 'true')
        navigate('/')
      })
      .catch(err => {
        setError(err)
        sessionStorage.setItem('isLoggedIn', 'false')
      });
  };

  return (
    <div className="page">
      <Header isLoggedIn={false} />
      <div className="body">
        <div className='login-form'>
          <h2>Sign In</h2>
          {error != null &&
            <div className="error"><strong>Error:</strong> {error}</div>
          }
          <form onSubmit={submit}>
            <dl>
              <dt>Username:</dt>
              <dd>
                <input type={"text"} name={"username"} size={30} placeholder={"Username"} required onChange={e => setUsername(e.target.value)} />
              </dd>
              <dt>Password:</dt>
              <dd>
                <input type={"password"} name={"password"} size={30} placeholder={"Password"} required onChange={e => setPassword(e.target.value)} />
              </dd>
            </dl>
            <div className='actions'><input type={"submit"} value={"Sign In"}/></div>
          </form>
        </div>
      </div>
      <Footer />
    </div>
  );
}

export default Login;