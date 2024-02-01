import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmationService } from 'primeng/api';
import { User } from 'src/app/models/user.model';
import { AuthService } from 'src/app/service/auth.service';
import { NotificationService } from 'src/app/service/notification.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent {
  isLoading = false;
  userIdentifier: string;
  user : User;
  editEmailDialog:boolean = false;
  editNameDialog:boolean = false;
  editSurnameDialog:boolean = false;
  editPhoneDialog:boolean = false;
  changePasswordDialog:boolean = false;
  emailForm: FormGroup;
  nameForm: FormGroup;
  surnameForm: FormGroup;
  phoneForm: FormGroup;
  changePasswordForm: FormGroup;
  editedEmail: string;
  editedName: string;
  editedSurname: string;
  editedPassword: string;
  editedConfirmPassword: string;
  enteredCurrentPassword: string;
  isSameEmail: boolean = false;
  hidePassword: boolean = true;
  hideCurrentPassword: boolean = true;
  hideNewPassword: boolean = true;
  hideConfirmPassword: boolean = true;
  message: string = null;
  error: boolean = false;
  constructor( private notificationService: NotificationService,private router: Router,
    private userService:UserService, private fb: FormBuilder,private confirmationService: ConfirmationService,) { 
      this.userIdentifier = localStorage.getItem('userIdentifier');

    }

    ngOnInit(){
  this.getUser();
  this.createForm();
}
// get user details by userid
  getUser(){
    this.isLoading = true; 
    this.userService.getUserById(this.userIdentifier).subscribe({
      next:(response) => {
        this.isLoading = false;
        this.user = response;
        this.setEditValues();
      }, error:(error) =>{
        this.isLoading = false;
        if (error.status == 422) {
          this.notificationService.showMessage('error', true, `${error.error}`, '');
        } else {
          this.notificationService.showMessage('error', true, `${error.status}
      - ${error.statusText} - ${error.error}`, '');
        }      
      }
    })
  
  }

  createForm() {
    this.emailForm = this.fb.group({
      email: ['', [Validators.required, Validators.pattern('^([A-Z|a-z|0-9](\.|_){0,1})+[A-Z|a-z|0-9]\@([A-Z|a-z|0-9])+((\.){0,1}[A-Z|a-z|0-9]){2}\.[a-z]{2,3}$')]]
    });
    this.nameForm = this.fb.group({
      name: ['', [Validators.required]]
    });
    this.surnameForm = this.fb.group({
      surname: ['']
    });
    this.changePasswordForm = this.fb.group({
      password: ['', [Validators.required,Validators.maxLength(20), Validators.minLength(6) ]],
      confirmPassword: ['', [Validators.required,Validators.maxLength(20), Validators.minLength(6) ]]
    });

  }

  closeDialog(){
    this.editEmailDialog = false;
    this.editNameDialog = false;
    this.editSurnameDialog = false;
    this.changePasswordDialog = false;
    this.isSameEmail = false;
    this.editedEmail = null;
    this.editedSurname = null;
    this.editedName = null;
    this.editedPassword = null;
    this.enteredCurrentPassword = null;
    this.error = false;
    this.createForm();
  }

  
  // change password
  changePassword(){
    if(!this.editedPassword || !this.editedConfirmPassword){
      this.error = true;
      this.message = 'Please enter values';
      return;
    }
    if(this.editedPassword.toLowerCase() != this.editedConfirmPassword.toLowerCase()){
      this.error = true;
      this.message = 'Password and Confirm Password must match';
      return;
    }
    this.isLoading = true; 
    this.user.password = this.editedPassword;
    this.userService.changePassword(this.userIdentifier, this.user).subscribe({
      next:(response) => {
        this.user.password = this.editedPassword;
        this.isLoading = false;
        this.closeDialog();
        this.notificationService.showSuccess(`Password updated successfully`) 
      }, error:(error) =>{
        this.isLoading = false; 
        if (error.status == 422) {
          this.notificationService.showMessage('error', true, `${error.error}`, '');
        } else {
          this.notificationService.showMessage('error', true, `${error.status}
      - ${error.statusText} - ${error.error}`, '');
        }      
      }
    })
  }
  togglePasswordVisibility() {
    this.hidePassword = !this.hidePassword;
  }

  setEditValues(){
    this.editedName = this.user.firstName;
    this.editedSurname = this.user.lastName;
  }

      // update email
      updateEmail(){
        if(!this.editedEmail){
          this.notificationService.showMessage('error', true, `Please enter valid email`, '');
          return;
        }
        this.user.email = this.editedEmail;
        this.confirmationService.confirm({
          message: 'By updating email your login email will be changed. Are you sure that you want to update email?',
          accept: () => {
            this.isLoading = true; 
            this.userService.updateUserEmail(this.user).subscribe({
              next:(response) => {
                this.isLoading = false;
                this.closeDialog();
                this.setEditValues();         
                this.notificationService.showSuccess(`Email updated successfully`);
                this.user = response;
              }, error:(error) =>{
                this.isLoading = false; 
                if (error.status == 422) {
                  this.notificationService.showMessage('error', true, `${error.error}`, '');
                } else {
                  this.notificationService.showMessage('error', true, `${error.status}
              - ${error.statusText} - ${error.error}`, '');
                }      
              }
            })
         }
      });
    
      }
    // update name
      updateName(){
        if(!this.editedName){
          this.notificationService.showMessage('error', true, `Please enter valid name`, '');
          return;
        }
        this.user.firstName = this.editedName;
        this.updateUser('Name');
      }
    // update surname
      updateSurname(){
        if(!this.editedSurname){
          this.notificationService.showMessage('error', true, `Please enter valid surname`, '');
          return;
        }
        this.user.lastName = this.editedSurname;
        this.updateUser('Surname');
      }

      // update user details 
    updateUser(type){
      this.isLoading = true; 
      this.userService.updateUser(this.user).subscribe({
        next:(response) => {
          this.user = response;
          this.setEditValues();
          this.isLoading = false;
          this.closeDialog();
          this.notificationService.showSuccess(`${type} updated successfully`) 
        }, error:(error) =>{
          this.isLoading = false; 
          if (error.status == 422) {
            this.notificationService.showMessage('error', true, `${error.error}`, '');
          } else {
            this.notificationService.showMessage('error', true, `${error.status}
        - ${error.statusText} - ${error.error}`, '');
          }      
        }
      })
    }

    onEditFirstName(){
      this.editNameDialog = true;
      this.editedName= this.user.firstName;
    }
    onEditSurname(){
      this.editSurnameDialog = true;
      this.editedSurname= this.user.lastName;
    }
}
