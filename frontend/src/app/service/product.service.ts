import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../models/product.model';
import { Observable } from 'rxjs';
import { PageInput } from '../shared/pageInput';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
    constructor(private http: HttpClient) { }
  
    public addProduct(product: Product): Observable<Product> {
      return this.http.post<Product>(environment.server + 'api/product/createProduct', product);
    }
  
    public deleteProduct(id: string): Observable<Product> {
      return this.http.delete<Product>(environment.server + `api/product/${id}`);
    }

    getProductsByLazyLoad(page: PageInput, userId: string): Observable<any> {
      return this.http.post<any>(`${environment.server}api/product/getProductListByUserId/${userId}`,page);
  }

  }
