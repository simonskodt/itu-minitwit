import Header from '../components/Header';
import Footer from './Footer';
import './Layout.css';
import { checkLogIn, getCurrentUsername } from '../builders/functions';

interface Request {
  endpoint: string;
}

function TimeLine() {

  if(checkLogIn()){
    console.log(checkLogIn())
    return(
      <div>{window.location.href = '/' + getCurrentUsername()}</div>
    )
  }else{
    return(
      <div>{window.location.href = '/public'}</div>

    )
  }
}

export default TimeLine

