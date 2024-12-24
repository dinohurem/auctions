import { DOCUMENT } from '@angular/common';
import { Inject, Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class CookieService {
    constructor(@Inject(DOCUMENT) private document: Document) { }

}