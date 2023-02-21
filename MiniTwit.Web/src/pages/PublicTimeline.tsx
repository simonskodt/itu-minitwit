import Footer from './Footer';
import Header from './Header';
import './Layout.css';
import { useContext } from 'react';
import { getMessageArray } from '../builders/functions';
import { FetchPublicTimeline } from './fetch';


function TimeLine() {
  return (
    <div className="page">
      <Header
        isLoggedIn={false}
      />
      <h2>Public Timeline</h2>
      <button onClick={() => {
        FetchPublicTimeline().then(
          (messages) => {
            let buildingData = getMessageArray(messages);
            console.log(buildingData);
            //setMessages(buildingData);
            //console.log(messages);
          },
        );
      }
      }>
        Fetch</button>

      <Footer />
    </div>
  );
}

export default TimeLine