import { HoldingClient, ShareClient, ShareHolderClient, StockClient } from "../api/api";

// TODO: Get from env vars
export const stockClient = new StockClient('-');

export const shareClient = new ShareClient('http://localhost:5000');

export const holdingClient = new HoldingClient('http://localhost:5000');

export const shareHolderClient = new ShareHolderClient('http://localhost:5000');