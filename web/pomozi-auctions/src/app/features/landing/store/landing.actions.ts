import { createAction, props } from '@ngrx/store';
import { Auction } from '../../../shared/models/auction.model';

// Action for adding an Auction
export const createAuction = createAction(
    '[Auction] Create Auction',
    props<{ auction: Auction }>()
);

export const createAuctionSuccess = createAction(
    '[Auction] Create Auction Success',
    props<{ auction: Auction }>()
);

export const createAuctionFailure = createAction(
    '[Auction] Create Auction Failure',
    props<{ error: any }>()
);