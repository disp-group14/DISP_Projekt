import React, { FC } from "react";
import { Stock } from "../Models/Stock";
import { StockItem } from "./StockItem";
import "./StockList.scss";

const stocks: Stock[] = [
  {
    name: "Doge Coin",
    value: 420,
  },
  {
    name: "Tesla Coin",
    value: 123,
  },
  {
    name: "Vestas",
    value: 10031,
  },
];

export const StockList: FC = () => {
  return (
    <div className ="container">
        <h1>Stocks</h1>
        <div className= "header">
            <p>Name</p>
            <p>Value</p>
        </div>
        <hr></hr>
        <div className="stockList">
            {stocks.map((item) => {
            return <StockItem stock={item}></StockItem>;
            })}
        </div>
    </div>
  );
};
