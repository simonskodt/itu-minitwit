import Footer from "./Footer";
import Header from "../components/Header";
import "./Layout.css";
import { useState } from "react";
import { checkLogIn } from "../state/SessionStorage";
import MessageSliceComponent from "../components/MessageSliceComponent";

function PublicTimeline() {
  const [pageNumber, setPageNumber] = useState(1);

  const handlePageChangeHigher = () => {
    setPageNumber(pageNumber + 1);
  };

  const handlePageChangeLower = () => {
    if (pageNumber <= 1) {
      alert("Press next page to see more messages")
    } else {
      setPageNumber(pageNumber + -1);
    }
  };

  return (
    <div className="page">
      <Header isLoggedIn={checkLogIn()} />
      <div className="body">
        <div className="headers">
          <div className="left">
            <h2>Public Timeline</h2>
          </div>
          <div className="right page-number">
          Page: {pageNumber}
          </div>
        </div>
        <MessageSliceComponent key={pageNumber} pageNumber={pageNumber} />
        <div className="page-number">Page: {pageNumber}</div>
        <div className="page-number">
          <button onClick={handlePageChangeLower}>Prev. Page</button>
          <button onClick={handlePageChangeHigher}>Next Page</button>
        </div>
      </div>
      <Footer />
    </div>
  );
}

export default PublicTimeline;
