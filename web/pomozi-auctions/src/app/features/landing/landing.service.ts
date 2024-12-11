import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Auction } from '../../shared/models/auction.model';

@Injectable({
    providedIn: 'root'
})
export class LandingService {

    private apiAuctionsUrl = 'auctions';

    constructor(private http: HttpClient) { }

    public createAuction(contact: Auction): Observable<Auction> {
        return this.http.post<Auction>(this.apiAuctionsUrl, contact);
    }
}
