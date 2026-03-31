// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../services/animations';
import { ProductEndpoint } from '../../services/product-endpoint.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

interface Product {
  id: number;
  name: string;
  description: string;
  icon: string;
  buyingPrice: number;
  sellingPrice: number;
  unitsInStock: number;
  isActive: boolean;
  isDiscontinued: boolean;
  productCategoryName: string;
}

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrl: './products.component.scss',
    animations: [fadeInOut],
    imports: [CommonModule, TranslateModule]
})
export class ProductsComponent implements OnInit {
  private productEndpoint = inject(ProductEndpoint);
  private alertService = inject(AlertService);

  products: Product[] = [];
  loadingIndicator = false;

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading products...');

    this.productEndpoint.getProductsEndpoint<Product[]>().subscribe({
      next: products => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.products = products;
      },
      error: error => {
        this.alertService.stopLoadingMessage();
        this.loadingIndicator = false;
        this.alertService.showStickyMessage('Load Error',
          'Unable to retrieve products from the Product Service.',
          MessageSeverity.error, error);
      }
    });
  }
}
