import React from 'react';
import logo from './logo.svg';
import { Button } from '@mui/material';
import { AppService } from '../services/app.service';

function SignUp() {

  const appService = new AppService();

  return (
    <div className="App">
      <header className="App-header">
        <div>THIS IS SIGNUP</div>
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

export default SignUp;
