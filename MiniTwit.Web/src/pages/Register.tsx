import { useState } from 'react';
import { Navigate } from "react-router-dom"
import { AppService } from '../services/app.service';
import Footer from './Footer';
import Header from '../components/Header';
import { useNavigate } from 'react-router-dom';
import { checkLogIn } from '../builders/functions';

function Register() {
  const [username, setUsername] = useState('')
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [passwordRepeat, setPasswordRepeat] = useState('')
  const [error, setError] = useState<string | null>(null)

  const service = new AppService()

  const navigate = useNavigate()

  const submit = (e: React.FormEvent) => {
    e.preventDefault() // Prevent page from reloading on submit

    if (username === '') {
      setError('You have to enter a username')
      return
    }

    if (!email.includes('@')) {
      setError('You have to enter a valid email address')
      return
    }

    if (password === '') {
      setError('You have to enter a password')
      return
    }

    if (password !== passwordRepeat) {
      setError('The two passwords do not match')
      return
    }

    service.registerUser(username, email, password)
      .then(() => {
        alert("You were successfully registered and can login now")
        navigate('/login')
      })
      .catch(error => setError(error))
  }

  if (checkLogIn())
    return (
      <Navigate replace to="/" />
    )
  else
    return (
      <div className="page">
        <Header isLoggedIn={false} />
        <div className="body">
          <h2>Sign Up</h2>
          {error != null &&
            <div className="error"><strong>Error:</strong> {error}</div>
          }
          <form onSubmit={submit}>
            <dl>
              <dt>Username:</dt>
              <dd>
                <input type="text" name="username" size={30} placeholder="Username" value={username} onChange={e => setUsername(e.target.value)}></input>
              </dd>
              <dt>E-Mail:</dt>
              <dd>
                <input type="text" name="email" size={30} placeholder="Email" onChange={e => setEmail(e.target.value)}></input>
              </dd>
              <dt>Password:</dt>
              <dd>
                <input type="password" name="password" size={30} placeholder="Password" onChange={e => setPassword(e.target.value)}></input>
              </dd>
              <dt>Password <small>(repeat)</small>:</dt>
              <dd>
                <input type="password" name="password2" size={30} placeholder="Password" onChange={e => setPasswordRepeat(e.target.value)}></input>
              </dd>
            </dl>
            <div className="actions"><input type="submit" value="Sign Up" /></div>
          </form>
        </div>
        <Footer />
      </div>
    )
}

export default Register
