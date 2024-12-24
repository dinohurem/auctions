import { Auction } from "../../../shared/models/auction.model";

export interface LandingState {
    auctions: Auction[];
    loading: boolean;
    error: any;
    currentPage: number;
    pageSize: number;
    totalResults: number;
}