import { DOCUMENT } from '@angular/common';
import { Inject, Injectable } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class CookieService {
    constructor(@Inject(DOCUMENT) private document: Document) { }

    getCookie(cookieName: string): string {
        return '';
    }

    hasCookie(cookieName: string): any { }

    setCookie(cookieName: string, value: string, expiresInMinutes: number | null = null): void {
    }

    clear(cookieName: string): void {
    }
}