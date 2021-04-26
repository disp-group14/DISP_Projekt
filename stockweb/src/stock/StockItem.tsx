import { FC } from "react";

import { Stock } from "../Models/Stock";
import "./stockItem.scss"

export interface Props {
    stock: Stock
}

export const StockItem: FC<Props> = ({stock: stock}) => {
    return (
        <div className = "stockItem">
            <div>{stock.name}</div>
            <div>{stock.value} $</div>
        </div>
    )

}

