import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { User } from 'src/app/models/user.model';
import { NotificationService } from 'src/app/service/notification.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent {
  user: User = new User();
  userForm: FormGroup;
  loading: boolean = false;

  constructor(private fb: FormBuilder,
    private router: Router,
    private userService: UserService,
    private _notificationService: NotificationService) {

  }
  ngOnInit(): void {
    this.createForm();
  }

  createForm() {
    this.userForm = this.fb.group({
      firstName: ['', [Validators.required]],
      lastName: [''],
      email: ['', [Validators.required, Validators.pattern('[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}')]],
      password: ['',[Validators.maxLength(20), Validators.minLength(6), Validators.required]],
      
    })
  }

  submitUser(){
    this.loading = true;
    if (this.userForm.invalid) {
      this.loading = false;
      return;
    }
    this.user.createdBy = "";
    this.userService.addUser(this.user).subscribe({ next:(response) =>  {
      this.loading = false;
      this._notificationService.showMessage('success', true, `Thank you for registrating with us.`, '');
      this.router.navigate(['/login']);
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

  onInputEmail() {
    // Convert the email to lowercase
    this.user.email = this.user.email.toLowerCase();
  }

}
