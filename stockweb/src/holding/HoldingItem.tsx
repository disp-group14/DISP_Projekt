import { FC } from "react";
import { IHolding } from "../api/api";

export interface Props {
    holding: IHolding
}

export const HoldingItem: FC<Props> = ({holding}) => {


    return <div>
        <div className = "holdingItem">
            <div>{holding.shares?.length}</div>
            <div>{holding.stock?.value}</div>
        </div>
    </div>
}



