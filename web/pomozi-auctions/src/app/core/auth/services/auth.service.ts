// core/auth/services/auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest, ResetPassword, TokenResponse } from '../../../shared/models/login.model';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login(loginRequest: LoginRequest): Observable<TokenResponse> {
    const email = loginRequest.email;
    const password = loginRequest.password;
    return this.http.post<TokenResponse>('auth/login', { email, password });
  }
}