import Header from './Header';
import Footer from './Footer';
import './Layout.css';

interface Request {
  endpoint: string;
}

function TimeLine() {
  return (
    <div className="page">
      <Header isLoggedIn={false} />
        <div className="body">
          <h2>Timeline</h2>
        </div>
      <Footer />
    </div>
  );
}

export default TimeLine