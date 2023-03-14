import Footer from "../pages/Footer";
import Header from "./Header";
import "../pages/Layout.css";
import { FetchPublicTimeline } from "../pages/fetch";
import { useState } from "react";
import { MessageObjectWithName } from "../builders/interface";
import { useEffect } from "react";
import { Message } from "../pages/Message";
import { checkLogIn } from "../builders/functions";

interface Props {
  pageNumber: number;
}

const MessageSliceComponent: React.FC<Props> = ({ pageNumber }) => {

  const [AllMessages, setMessages] = useState<MessageObjectWithName[]>();

  useEffect(() => {
    FetchPublicTimeline(pageNumber).then((messages) => {
      setMessages(messages);
    });
  }, []);

  if (AllMessages != undefined) {
    return (
      <>
        {AllMessages?.map((mes) => (
          <view key={mes.messageId}>
            <view>
              <Message
                username={mes.userName}
                text={mes.text}
                date={mes.pubDate}
              />
            </view>
          </view>
        ))}
      </>
    );
  } else {
    return (
      <div>
        <h2>Public TimeLine</h2>
        <ul className="messages">
          <li>
            <p></p>
          </li>
        </ul>
      </div>
    );
  }
}

export default MessageSliceComponent;
