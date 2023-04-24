import './Layout.css';
import { checkLogIn, getCurrentUsername } from '../state/SessionStorage';
import { Navigate } from 'react-router-dom';

function TimeLine() {
  if (checkLogIn()) {
    return (
      <Navigate replace to={"/" + getCurrentUsername()} />
    )
  } else {
    return (
      <Navigate replace to="/public" />
    )
  }
}

export default TimeLine

