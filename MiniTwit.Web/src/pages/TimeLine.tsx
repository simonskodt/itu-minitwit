import Footer from './Footer';
import Layout from './Layout';
import './Layout.css';

interface Request {
  endpoint: string;
}


// const TimelineTitle = ({r: Request, profile_user }) => {
//   const endpoint = r.endpoint;
//   if (endpoint === 'public_timeline') {
//     return <h1>Public Timeline</h1>;
//   } else if (endpoint === 'user_timeline') {
//     return <h1>{profile_user.username}'s Timeline</h1>;
//   }
// };

function TimeLine() {
  return (
    <div className="page">
      
      <Layout />
        <h2>Public Timeline</h2>
      <Footer />
    </div>
  );
}

export default TimeLine