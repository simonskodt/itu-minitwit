import './Login.css';
import React, { useState } from 'react';
import { AppService } from '../services/app.service';

const Login = () => {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');

    const appService = new AppService();


  const submit = (e: React.FormEvent) => {
      appService.Login(username, password);
      e.preventDefault();
  };

    return (
      <form onSubmit={submit} className="login-form">
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

      <button type="submit">Login</button>
    </form>
    );
  }

  export default Login;