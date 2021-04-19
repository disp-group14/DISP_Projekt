import React from 'react';
import './App.css';
import { StockList } from './stock/StockList';

const App: React.FC = () => {
  return (
    <div className="App">
      <header className="App-header">
        <StockList></StockList>
      </header>
    </div>
  );
}

export default App;
