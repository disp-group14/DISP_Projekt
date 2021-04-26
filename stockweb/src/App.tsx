import React, { useState } from "react";
import { BrowserRouter, Link, Route, Switch } from "react-router-dom";
import "./App.scss";
import { HoldingView } from "./holding/HoldingView";
import { StockList } from "./stock/StockList";

const App: React.FC = () => {
  const [Seleceted, setSeleceted] = useState("Stocks");
  return (
    <BrowserRouter>
      <div className="app">
        <div className="navigation">
          <Link to="/" className= {Seleceted === "Stocks"?"selected":"none"}  onClick ={e => setSeleceted("Stocks")}  ><h1>Stocks</h1></Link>
          <div></div>
          <Link to="/holdings" className= {Seleceted === "Portfolio"?"selected":"none"} onClick ={e => setSeleceted("Portfolio")}><h1>Portfolio</h1></Link>
        </div>

        <Switch>
          <Route path="/holdings">
            <HoldingView></HoldingView>
          </Route>
          <Route path="/">
            <StockList></StockList>
          </Route>
        </Switch>
      </div>
    </BrowserRouter>
  );
};

export default App;
