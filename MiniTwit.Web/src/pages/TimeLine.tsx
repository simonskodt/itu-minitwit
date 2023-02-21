

import Header from './Header';
import Footer from './Footer';
import './Layout.css';

interface Request {
  endpoint: string;
}

function TimeLine() {
  return (
    <div className="page">
      <Header 
        isLoggedIn={false}
      />
        <h2>Public Timeline</h2>
      <Footer />
    </div>
  );
}

export default TimeLine