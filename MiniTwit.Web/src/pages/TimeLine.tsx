import Footer from './Footer';
import Layout from './Layout';
import './Layout.css';

interface Request {
  endpoint: string;
}

function TimeLine() {

  return (
    <div className="page">
      
      <Layout/>
        <h2>Public Timeline</h2>
      <Footer />
    </div>
  );
}

export default TimeLine