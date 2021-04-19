import React from 'react';
import { Holding } from './api/api';
import './App.css';
import { HoldingItem } from './holding/HoldingItem';
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
