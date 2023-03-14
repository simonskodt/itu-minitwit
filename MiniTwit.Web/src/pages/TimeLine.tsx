import Header from '../components/Header';
import Footer from './Footer';
import './Layout.css';
import { checkLogIn } from '../builders/functions';

interface Request {
  endpoint: string;
}

function TimeLine() {

  return (
    <div className="page">
      <Header isLoggedIn={checkLogIn()} />
        <div className="body">
          <h2>Timeline</h2>
        </div>
      <Footer />
    </div>
  );
}

export default TimeLine