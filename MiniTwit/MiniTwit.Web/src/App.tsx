import React from 'react';
import logo from './logo.svg';
import './App.css';
import { Button } from '@mui/material';
import { AppService } from './services/app.service';

function App() {

  const appService = new AppService();

  return (
    <div className="App">
      <header className="App-header">
        <Button
          onClick={() => {
            alert('clicked');
            const response = appService.insertUser();
          }}
        >
        Clich here to insert data
    </Button>
      </header>
    </div>
  );
}

export default App;
