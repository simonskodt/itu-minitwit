import ReactDOM from "react-dom";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import './index.css';
import Login from "./pages/Login";
import TimeLine from "./pages/TimeLine";
import PublicTimeline from "./pages/PublicTimeline";
import SignUp from "./pages/SignUp";

export const LOCALHOST = "https://localhost:7111/";
export const PRODUCTION = "http://164.92.167.188:80/";

export default function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<TimeLine />} />
        <Route path="/public" element={<PublicTimeline />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<SignUp />} />
      </Routes>
    </>
  );
}
