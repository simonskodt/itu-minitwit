import { useState } from 'react';
import { Navigate } from "react-router-dom"
import { useNavigate } from 'react-router-dom';
import Header from '../components/Header';
import Footer from './Footer';
import { checkLogIn } from '../state/SessionStorage';
import { AuthenticationService } from '../services/AuthenticationService';
import { APIError } from '../models/APIError';

const Login = () => {
  const [username, setUsername] = useState('')
  const [password, setPassword] = useState('')
  const [error, setError] = useState<string | null>(null)

  const authService = new AuthenticationService()

  const navigate = useNavigate()

  const submit = (e: React.FormEvent) => {
    e.preventDefault() // Prevent page from reloading on submit

    if (username === '') {
      setError('You have to enter a username')
      return
    }

    if (password === '') {
      setError('You have to enter a password')
      return
    }

    authService.login(username, password)
      .then(user => {
        sessionStorage.setItem('username', user.username)
        sessionStorage.setItem('isLoggedIn', 'true')
        navigate('/')
      })
      .catch((error: APIError) => {
        setError(error.error_msg)
        sessionStorage.setItem('isLoggedIn', 'false')
      })
  }

  if (checkLogIn()) {
    return (
      <Navigate replace to="/" />
    )
  }
  else
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
                  <input type="text" name="username" size={30} placeholder="Username" onChange={e => setUsername(e.target.value)} />
                </dd>
                <dt>Password:</dt>
                <dd>
                  <input type="password" name="password" size={30} placeholder="Password" onChange={e => setPassword(e.target.value)} />
                </dd>
              </dl>
              <div className="actions"><input type="submit" value="Sign In" /></div>
            </form>
          </div>
        </div>
        <Footer />
      </div>
    )
}

export default Login
