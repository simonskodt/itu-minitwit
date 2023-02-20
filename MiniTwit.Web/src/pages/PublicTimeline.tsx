import Footer from './Footer';
import Layout from './Layout';
import './Layout.css';
import { fetchPublicTimeline } from './fetch';

interface Request {
  endpoint: string;
}

function TimeLine() {
  return (
    <div className="page">
      
      <Layout />
        <h2>Public Timeline</h2>
      <button onClick={() => {
        
        var result = fetchPublicTimeline()
        //console.log(result)
        
  }     
        }>
          Fetch</button>
      <Footer />
    </div>
  );
}

export default TimeLine