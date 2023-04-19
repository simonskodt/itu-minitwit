import { Routes, Route } from "react-router-dom";
import './index.css';
import Login from "./pages/Login";
import PublicTimeline from "./pages/PublicTimeline";
import Register from "./pages/Register";
import PrivateTimeline from "./pages/PrivateTimeline";
import TimeLine from "./pages/TimeLine";
import { useEffect }  from 'react';

const DEVELOPMENT = "http://localhost:80/";
const PRODUCTION = "http://164.92.167.188:80/";

export const API_URL = process.env.NODE_ENV === 'development' ? DEVELOPMENT : PRODUCTION;

export default function App() {
  useEffect(() => {
    // Remove leading '/' character from pathname
    const pathname = window.location.pathname.replace('/', '');
    const withoutSpaces = pathname.replace(/%20/g, " ");
    document.title = `${withoutSpaces.charAt(0).toUpperCase()}${withoutSpaces.slice(1)} | MiniTwit`;
  }, []);

  return (
    <>
      <Routes>
        <Route path="/public" element={<PublicTimeline />} />
        <Route path="/" element={<TimeLine />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} /> 
        <Route path="/:username" element={<PrivateTimeline/>}></Route>
      </Routes>
    </>
  );
}
