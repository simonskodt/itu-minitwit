import ReactDOM from "react-dom";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import './index.css';
import Login from "./pages/Login";
import TimeLine from "./pages/TimeLine";
import PublicTimeline from "./pages/PublicTimeline";
import SignUp from "./pages/SignUp";

export default function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<TimeLine />} />
        <Route path="/public" element={<PublicTimeline />} />
        <Route path="/login" element={<Login />} />
        <Route path="/signup" element={<SignUp />} />
      </Routes>
    </>
  );
}
