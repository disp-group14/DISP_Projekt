import { Share } from "./Share";

export interface Stock {
    name: string,
    value: number,
    shares?: Share[]
}