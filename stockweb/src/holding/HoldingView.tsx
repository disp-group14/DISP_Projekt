import { FC } from "react";
import { IHolding } from "../api/api";
import { HoldingItem } from "./HoldingItem";

const holdings: IHolding[] = [];
export const HoldingView: FC = () => {
  return (
    <div>
      <div>hello</div>
      {holdings.map((item) => {
        return (
          <div className = "holdingItem">
            <HoldingItem holding={item}></HoldingItem>;
            <button>Sell</button>
          </div>
        );
      })}
    </div>
  );
};
