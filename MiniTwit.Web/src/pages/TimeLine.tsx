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
      <input type="hidden">{window.location.href = '/' + getCurrentUsername()}</input>
    )
  }else{
    return(
      <input type="hidden">{window.location.href = '/public'}</input>

    )
  }
}

export default TimeLine

