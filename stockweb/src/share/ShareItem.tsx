import { FC } from "react";
import { Share } from "../Models/Share";

export interface Props {
    share: Share
}

export const ShareItem: FC<Props> = ({share}) => {


    return <div>
        <div className = "shareItem">
            <div>{share.stockId}</div>
            <div>{share.owner}</div>
        </div>
    </div>
}



