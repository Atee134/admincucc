import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpHandler, HttpRequest, HttpErrorResponse, HttpEvent, HTTP_INTERCEPTORS } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ErrorDetails } from '../_models/generatedDtos';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
    intercept (req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(req).pipe(
            catchError(error => {
                if (error instanceof HttpErrorResponse) {
                    if (error.status === 401) {
                        return throwError(error.statusText);
                    }
                    const serverError = error.error;
                    let errorMessages = '';
                    const applicationError = error.headers.get('Application-Error');
                    if (applicationError) {
                        const errorsSplit = applicationError.split(';');
                        for (let i = 0; i < errorsSplit.length; i++) {
                            errorMessages += errorsSplit[i] + '\n';
                        }
                        return throwError(errorMessages);
                    }
                    let modelStateErrors = '';
                    if (serverError && typeof serverError === 'object') {
                        for (const key in serverError) {
                            if (serverError[key]) {
                                modelStateErrors += serverError[key] + '\n';
                            }
                        }
                        return throwError(modelStateErrors);
                    }
                return throwError(serverError || 'Server Error');
                }
            })
        );
    }
}

export const ErrorInterceptorProvider = {
    provide: HTTP_INTERCEPTORS,
    useClass: ErrorInterceptor,
    multi: true
}
