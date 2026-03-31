import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointBase } from './endpoint-base.service';
import { ConfigurationService } from './configuration.service';

@Injectable({
  providedIn: 'root'
})
export class ProductEndpoint extends EndpointBase {
  private http = inject(HttpClient);
  private configurations = inject(ConfigurationService);

  get productsUrl() { return this.configurations.productServiceUrl + '/api/product'; }
  get categoriesUrl() { return this.configurations.productServiceUrl + '/api/product/categories'; }

  getProductsEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.productsUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getProductsEndpoint<T>());
      }));
  }

  getProductEndpoint<T>(productId: number): Observable<T> {
    const endpointUrl = `${this.productsUrl}/${productId}`;

    return this.http.get<T>(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getProductEndpoint<T>(productId));
      }));
  }

  getNewProductEndpoint<T>(product: object): Observable<T> {
    return this.http.post<T>(this.productsUrl, JSON.stringify(product), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getNewProductEndpoint<T>(product));
      }));
  }

  getUpdateProductEndpoint<T>(product: object, productId: number): Observable<T> {
    const endpointUrl = `${this.productsUrl}/${productId}`;

    return this.http.put<T>(endpointUrl, JSON.stringify(product), this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getUpdateProductEndpoint<T>(product, productId));
      }));
  }

  getDeleteProductEndpoint<T>(productId: number): Observable<T> {
    const endpointUrl = `${this.productsUrl}/${productId}`;

    return this.http.delete<T>(endpointUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getDeleteProductEndpoint<T>(productId));
      }));
  }

  getCategoriesEndpoint<T>(): Observable<T> {
    return this.http.get<T>(this.categoriesUrl, this.requestHeaders).pipe(
      catchError(error => {
        return this.handleError(error, () => this.getCategoriesEndpoint<T>());
      }));
  }
}
