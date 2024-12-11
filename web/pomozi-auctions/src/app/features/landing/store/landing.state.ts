export interface LandingState {
    auctions: any[];
    loading: boolean;
    error: any;
    currentPage: number;
    pageSize: number;
    totalResults: number;
}