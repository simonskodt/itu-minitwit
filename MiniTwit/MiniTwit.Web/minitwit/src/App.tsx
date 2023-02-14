import React from 'react';
import logo from './logo.svg';
import './App.css';
import { Button } from '@mui/material';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <Button
          onClick={() => {
            alert('clicked');
            
          }}
        >
        Clich here to insert data
    </Button>
      </header>
    </div>
  );
}

export default App;
