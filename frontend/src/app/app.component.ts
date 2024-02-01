import { ChangeDetectorRef, Component } from '@angular/core';
import { AuthService } from './service/auth.service';
import { UserService } from './service/user.service';
import { NotificationService } from './service/notification.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Hired 1st';
  isLoading = false;
  userIsAuthenticated = false;
  user = {
    name: "",
    surname: "",
  };
  userIdentifier: string;

  constructor(private authService: AuthService, private notificationService: NotificationService,    private router: Router,
    private userService:UserService,private cdr: ChangeDetectorRef) { 
    }

  ngOnInit() {
    this.userIsAuthenticated = this.authService.IsAuthenticated();
    if(this.userIsAuthenticated) this.setAuthenticatedUser();
    this.authService
      .getAuthStatusListener()
      .subscribe(isAuthenticated => {
        if(isAuthenticated) this.setAuthenticatedUser();
        this.userIsAuthenticated = isAuthenticated;
        this.cdr.detectChanges(); 
      });
  }

  // check if the user is valid and authenticate user
  setAuthenticatedUser(){
    this.userIdentifier = this.authService.getUserIdentifier();
  }

//perform action on logout  
  onLogout() {
    this.authService.logout();
  }

}
