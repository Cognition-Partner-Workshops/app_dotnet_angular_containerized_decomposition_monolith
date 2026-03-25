// ---------------------------------------
// Product Catalog API Endpoint Service
// Calls the standalone Product Catalog microservice
// ---------------------------------------

import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { environment } from '../../environments/environment';
import { AuthService } from './auth.service';

@Injectable({ providedIn: 'root' })
export class ProductEndpointService {
  private authService = inject(AuthService);
  private http = inject(HttpClient);

  private get productCatalogUrl(): string {
    return environment.productCatalogUrl || '';
  }

  private get productsUrl() {
    return `${this.productCatalogUrl}/api/products`;
  }

  private get categoriesUrl() {
    return `${this.productCatalogUrl}/api/products/categories`;
  }

  private get requestHeaders(): { headers: HttpHeaders } {
    const headers = new HttpHeaders({
      Authorization: `Bearer ${this.authService.accessToken}`,
      'Content-Type': 'application/json',
      Accept: 'application/json, text/plain, */*'
    });
    return { headers };
  }

  getProductsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.productsUrl, this.requestHeaders);
  }

  getProductEndpoint<T>(productId: number): Observable<T> {
    return this.http.get<T>(`${this.productsUrl}/${productId}`, this.requestHeaders);
  }

  createProductEndpoint<T>(product: object): Observable<T> {
    return this.http.post<T>(this.productsUrl, JSON.stringify(product), this.requestHeaders);
  }

  updateProductEndpoint<T>(productId: number, product: object): Observable<T> {
    return this.http.put<T>(`${this.productsUrl}/${productId}`, JSON.stringify(product), this.requestHeaders);
  }

  deleteProductEndpoint<T>(productId: number): Observable<T> {
    return this.http.delete<T>(`${this.productsUrl}/${productId}`, this.requestHeaders);
  }

  getCategoriesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.categoriesUrl, this.requestHeaders);
  }
}
