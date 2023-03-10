import ReactDOM from "react-dom";
import { BrowserRouter, Routes, Route } from "react-router-dom";
import './index.css';
import Login from "./pages/Login";
import TimeLine from "./pages/TimeLine";
import PublicTimeline from "./pages/PublicTimeline";
import SignUp from "./pages/SignUp";
import PrivateTimeline from "./pages/PrivateTimeline";

const DEVELOPMENT = "https://localhost:7111/";
const PRODUCTION = "http://164.92.167.188:80/";

export const API_URL = process.env.NODE_ENV === 'development' ? DEVELOPMENT : PRODUCTION;

export default function App() {
  return (
    <>
      <Routes>
        <Route path="/" element={<TimeLine />} />
        <Route path="/public" element={<PublicTimeline />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<SignUp />} /> 
        <Route path="/:userName" element={<PrivateTimeline/>}></Route>
      </Routes>
    </>
  );
}
