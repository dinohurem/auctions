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

  registerCandidate(registrationRequest: FormData): Observable<any> {
    // Get the fullName value from FormData
    const firstName = registrationRequest.get('firstName') as string | null;
    const lastName = registrationRequest.get('lastName') as string | null;

    // Append firstName and lastName to FormData
    if (firstName) {
      registrationRequest.append('firstName', firstName);
    }
    if (lastName) {
      registrationRequest.append('lastName', lastName);
    }

    // Send the request to the backend
    return this.http.post<any>('auth/register', registrationRequest);
  }
}