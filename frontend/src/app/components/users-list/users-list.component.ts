import { ChangeDetectorRef, Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmationService, LazyLoadEvent } from 'primeng/api';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/service/auth.service';
import { NotificationService } from 'src/app/service/notification.service';
import { UserService } from 'src/app/service/user.service';
import { PageInput } from 'src/app/shared/pageInput';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.css']
})
export class UsersListComponent {
  user: User = new User();
  usersList: User[] = [];
  pageInput: PageInput = new PageInput();
  loading: boolean = true;
  totalRecords?: number = 0;
  page: number = 0;
  rows: number = 10;
  newDialog: boolean = false;
  isNew: boolean = false;
  isLoading: boolean = false;
  userForm: FormGroup;
  userId: string;
  constructor(private fb: FormBuilder,
    private userService: UserService,
    private confirmationService: ConfirmationService,
    private ref: ChangeDetectorRef,
    private _notificationService: NotificationService,
    private authService: AuthService) { 
      this.userId = localStorage.getItem('userIdentifier');
    }

    ngOnInit(): void {
      this.createForm();
    }


  addNewUser(){
    this.createForm();
    this.isNew = true
    this.newDialog = true;
    this.user = new User();
  }

  loadUsers(event: LazyLoadEvent) {
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
      this.userService.getUsersByLazyLoad(this.pageInput).subscribe((data:any)=>{
        if(data==null)
        {
          this.totalRecords = 0;
          this.usersList = [];  
        }
        else
        {
          this.totalRecords = data.count;
          this.usersList = data.items;
        }
        this.loading = false;      
      });
     
      
  }

  onEdit(user: User): void {
    this.isNew = false;
    this.user = user;
    this.newDialog = true;
  }

  onDelete(user: User){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this user?',
      accept: () => {
        this.isLoading = true; 
        this.userService.deleteUser(user.id).subscribe({ next:(response) =>  {
          this.loading = false;
          this._notificationService.showMessage('success', true, `User deleted successfully`, '');
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
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: [''],
      email: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}')]]      
    })
  }

  submitUser(){
    this.loading = true;
    if (this.userForm.invalid) {
      this.loading = false;
      return;
    }
    this.user.createdBy = this.userId;
    this.createUser();
  }
  onInputEmail() {
    // Convert the email to lowercase
    this.user.email = this.user.email.toLowerCase();
  }
  hideDialog(): void {
    this.newDialog = false;
    this.createForm();
  }
  createUser(){
    this.userService.addNewUser(this.user).subscribe({ next:(response) =>  {
      this.loading = false;
      this.newDialog = false;
      this.createForm();
      if (this.isNew) {
        this.usersList.push(this.user);
        this.totalRecords++;
        this._notificationService.showMessage('success', true, `New user added successfully`, '');
      }else{
        let index = this.usersList.findIndex(x => x.id == this.userId);
        if(index != -1){
          this.usersList[index] = this.user;
        }
        this._notificationService.showMessage('success', true, `User updated successfully`, '');
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
