import React from 'react';
import Header from '../components/Header';
import Footer from './Footer';
import { Component }  from 'react';
import './Layout.css';
import { checkLogIn, getCurrentUsername } from '../builders/functions';

function TimeLine() {
  if (checkLogIn()) {
    console.log(checkLogIn())
    return (
      <input type="hidden">{window.location.href = '/' + getCurrentUsername()}</input>
    )
  } else {
    return (
      <input type="hidden">{window.location.href = '/public'}</input>
    )
  }
}

export default TimeLine

