import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/service/auth.service';
import { NotificationService } from 'src/app/service/notification.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  userForm: FormGroup;
  loading: boolean = false;
  email: string;
  password: string;
  user: User = new User();
  isLoading: boolean = false;

  constructor(private fb: FormBuilder,
    private userService: UserService,
    private authService: AuthService,
    private _notificationService: NotificationService) {

  }
  ngOnInit(): void {
    this.createForm();
  }

  createForm() {
    this.userForm = this.fb.group({
      email: [''],
      password: [''],
      
    })
  }

  onLogin() {
    if (!this.email || !this.password) {
      return;
    }
    this.isLoading = true;
    this.user.email = this.email;
    this.user.password = this.password;
    this.authService.loginUser(this.user);
    this.isLoading = false;
  }
}
