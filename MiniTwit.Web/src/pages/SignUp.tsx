import React, { useState } from 'react';
import { AppService } from '../services/app.service';
import Footer from './Footer';
import Layout from './Layout';
import { useNavigate } from 'react-router-dom';

function SignUp() {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [passwordRepeat, setPasswordRepeat] = useState('');


  const appService = new AppService();

  const navigate = useNavigate();
  
  const goToHome= () => {
    navigate('/'); 
  };

  const submit = (e: React.FormEvent) => {
    console.log("USERNAME:"+username)
    if (password != passwordRepeat){
      alert("Passwords doesnt match")
      return
    }

    if(!email.includes('@')){
      alert("Wrong Email Format")
      return
    }

    let promise = appService.registerUser(username, email, password);
    promise.catch( () => alert("An error occured, all input fields must be filled"))
    promise.then(goToHome)
};

return (
  <div className="page">
    <Layout />
      <div className='login-form'>
      <h2>Sign Up</h2>
        <label htmlFor="username">Username</label>
        <input 
          type="text"
          placeholder="Username"
          name="username"
          required
          onChange={e => setUsername(e.target.value)}
        />
        <label htmlFor="email">Email</label>
        <input 
          type="text"
          placeholder="Email"
          name="email"
          required
          onChange={e => setEmail(e.target.value)}
        />
        <label htmlFor="password">Password</label>
        <input
          type="password"
          placeholder="Password"
          name="password"
          required
          onChange={e => setPassword(e.target.value)}
        />
        <label htmlFor="password(repeat)">Password(Repeat)</label>
        <input
          type="password(repeat)"
          placeholder="Password(Repeart)"
          name="password(repeat)"
          required
          onChange={e => setPasswordRepeat(e.target.value)}
        />
        <button onClick={submit}>Sign Up</button>
      </div>
    <Footer />
 </div>
);
}
export default SignUp;
