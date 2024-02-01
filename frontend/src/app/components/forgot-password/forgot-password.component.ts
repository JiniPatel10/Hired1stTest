import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { NotificationService } from 'src/app/service/notification.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
  isLoading = false;
  isUserExists:boolean = true;
  emailSent: boolean = false;
  email: string;
  constructor(
    private router: Router,
    private userService: UserService,
    private notificationService:NotificationService
    ) {} 

  onContinue() {
    if (!this.email) {
      return;
    }
    const emailPattern: RegExp = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    const isValidEmail: boolean = emailPattern.test(this.email);

    if (isValidEmail) {
      this.sendForgotPasswordMail(this.email); 
    }else{
      this.notificationService.showMessage('error', true, `Enter valid email`, '');
    }

  }

  //navigate to login
  moveToLogin(){
    this.router.navigate(['/login']);
  }

  // send forgot password mail
  sendForgotPasswordMail(email: string) {
    this.isLoading = true;
    this.userService.sendForgotPasswordMail(email)
      .subscribe({
        next:(response) => {
          this.isLoading = false;
          if(response){
            this.emailSent = true;
            this.email = email;
            this.notificationService.showMessage('success', true, `Email send successfully`, '');
          }else {
            this.emailSent = false;
            this.email = null;
          }
      },
      error:(error) =>{
        this.isLoading = false;
        this.email = null;
        if (error.status == 422) {
          this.notificationService.showMessage('error', true, `${error.error}`, '');
        } else {
          this.notificationService.showMessage('error', true, `${error.status}
      - ${error.statusText} - ${error.error}`, '');
        }
      }});
  }
  onInputEmail() {
    // Convert the email to lowercase
    this.email = this.email.toLowerCase();
  }
}