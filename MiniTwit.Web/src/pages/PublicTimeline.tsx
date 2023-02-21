import Footer from './Footer';
import Layout from './Layout';
import './Layout.css';
import { useContext } from 'react';
import { MessagesContext } from '../contexts/messageContext';
import { getMessageArray } from '../builders/functions';
import { FetchPublicTimeline } from './fetch';


function TimeLine() {
  const {messages} = useContext(MessagesContext) 
  const { setMessages } = useContext(MessagesContext);
  

  return (
    <div className="page">
      
      <Layout />
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

          <button onClick={()=> console.log(messages)}>
            test

          </button>
      <Footer />
    </div>
  );
}

export default TimeLine