// Model for auction.

export interface Auction {
    id: number;
    title: string;
    description: string;
    startPrice: number;
    currentPrice: number;
    startDate: Date;
    endDate: Date;
    image: string;
    userId: number;
    user: any;
    categoryId: number;
    category: any;
}