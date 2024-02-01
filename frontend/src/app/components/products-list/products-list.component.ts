import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, LazyLoadEvent } from 'primeng/api';
import { Product } from 'src/app/models/product.model';
import { AuthService } from 'src/app/service/auth.service';
import { NotificationService } from 'src/app/service/notification.service';
import { ProductService } from 'src/app/service/product.service';
import { PageInput } from 'src/app/shared/pageInput';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css']
})
export class ProductsListComponent {
  product: Product = new Product();
  productList: Product[] = [];
  pageInput: PageInput = new PageInput();
  loading: boolean = true;
  totalRecords?: number = 0;
  page: number = 0;
  rows: number = 10;
  newDialog: boolean = false;
  isNew: boolean = false;
  isLoading: boolean = false;
  productForm: FormGroup;
  userId: string;
  constructor(private fb: FormBuilder,
    private productService: ProductService,
    private confirmationService: ConfirmationService,
    private ref: ChangeDetectorRef,
    private _notificationService: NotificationService,
    private authService: AuthService) { 
      this.userId = localStorage.getItem('userIdentifier');
    }

    ngOnInit(): void {
      this.createForm();
    }


  addNewProduct(){
    this.createForm();
    this.product = new Product();
    this.isNew = true
    this.newDialog = true;
  }

  loadProducts(event: LazyLoadEvent) {
    if (event !== undefined) {
      this.loading = true;
      this.pageInput.rows = event.rows;
      if(event.sortField)  this.pageInput.sortField = event.sortField;
      else this.pageInput.sortField = 'id';
      this.pageInput.sortOrder = event.sortOrder;
      this.pageInput.filters = event.filters;
      this.rows = event.rows;
      this.ref.detectChanges();
      this.loadLazyData();
    }
  }

  loadLazyData(){
      this.productService.getProductsByLazyLoad(this.pageInput, this.userId).subscribe((data:any)=>{
        if(data==null)
        {
          this.totalRecords = 0;
          this.productList = [];  
        }
        else
        {
          this.totalRecords = data.count;
          this.productList = data.items;
        }
        this.loading = false;      
      });
     
      
  }

  onEdit(product: Product): void {
    this.isNew = false;
    this.product = product;
    this.newDialog = true;
  }

  onDelete(product: Product){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this product?',
      accept: () => {
        this.isLoading = true; 
        this.productService.deleteProduct(product.id).subscribe({ next:(response) =>  {
          this.loading = false;
          this._notificationService.showMessage('success', true, `Product deleted successfully`, '');
        },
        error:(error: any) => {
          this.loading = false;
          if (error.status == 422) {
            this._notificationService.showMessage('error', true, `${error.error}`, '');
          } else {
            this._notificationService.showMessage('error', true, `${error.status}
        - ${error.statusText} - ${error.error}`, '');
          }
        }}); 
     }
  });
  
  }

  createForm() {
    this.productForm = this.fb.group({
      name: ['', [Validators.required]],
      price: ['',[Validators.required]],
      description: ['']      
    })
  }

  submitProduct(){
    this.loading = true;
    if (this.productForm.invalid) {
      this.loading = false;
      return;
    }
    this.product.createdBy = this.userId;
    this.createProduct();
  }

  hideDialog(): void {
    this.newDialog = false;
    this.createForm();
  }
  createProduct(){
    this.productService.addProduct(this.product).subscribe({ next:(response) =>  {
      this.loading = false;
      this.newDialog = false;
      this.createForm();
      if (this.isNew) {
        this.productList.push(this.product);
        this.totalRecords++;
        this._notificationService.showMessage('success', true, `New product added successfully`, '');
      }else{
        let index = this.productList.findIndex(x => x.id == this.product.id);
        if(index != -1){
          this.productList[index] = this.product;
        }
        this._notificationService.showMessage('success', true, `Product updated successfully`, '');
      }
    },
    error:(error: any) => {
      this.loading = false;
      this.newDialog = false;
      if (error.status == 422) {
        this._notificationService.showMessage('error', true, `${error.error}`, '');
      } else {
        this._notificationService.showMessage('error', true, `${error.status}
    - ${error.statusText} - ${error.error}`, '');
      }
    }});  
  }

}

