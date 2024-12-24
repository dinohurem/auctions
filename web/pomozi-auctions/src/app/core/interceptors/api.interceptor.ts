import { HttpInterceptorFn } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { inject } from '@angular/core';
import { CookieService } from '../auth/services/cookie.service';

export const apiInterceptor: HttpInterceptorFn = (req, next) => {
    var cookieService: CookieService = inject(CookieService);
    const Authorization = `Bearer`;
    // const Authorization = `Bearer ${cookieService.getCookie('accessToken')}`;
    const apiReq = req.clone({
        url: `${environment.apiUrl}/${req.url}`,
        setHeaders: {
            Authorization,
            'app-type': 'WEB',
        }
    });
    return next(apiReq);
};
