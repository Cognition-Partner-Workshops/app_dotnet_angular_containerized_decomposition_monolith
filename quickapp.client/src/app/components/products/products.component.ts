// ---------------------------------------
// Email: quickapp@ebenmonney.com
// Templates: www.ebenmonney.com/templates
// (c) 2024 www.ebenmonney.com/mit-license
// ---------------------------------------

import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TranslateModule } from '@ngx-translate/core';

import { fadeInOut } from '../../services/animations';
import { ProductEndpointService } from '../../services/product-endpoint.service';
import { AlertService, MessageSeverity } from '../../services/alert.service';

@Component({
    selector: 'app-products',
    templateUrl: './products.component.html',
    styleUrl: './products.component.scss',
    animations: [fadeInOut],
    imports: [CommonModule, TranslateModule]
})
export class ProductsComponent implements OnInit {
  private productEndpoint = inject(ProductEndpointService);
  private alertService = inject(AlertService);

  products: any[] = [];
  loadingIndicator = false;

  ngOnInit() {
    this.loadProducts();
  }

  loadProducts() {
    this.loadingIndicator = true;
    this.alertService.startLoadingMessage('Loading products...');

    this.productEndpoint.getProductsEndpoint<any[]>()
      .subscribe({
        next: data => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.products = data;
        },
        error: error => {
          this.alertService.stopLoadingMessage();
          this.loadingIndicator = false;
          this.alertService.showStickyMessage('Load Error',
            'Unable to retrieve products from the Product Catalog service.\n' +
            'Ensure the Product Catalog API is running.',
            MessageSeverity.error, error);
        }
      });
  }
}
