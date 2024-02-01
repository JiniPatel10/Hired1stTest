import { ChangeDetectorRef, Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/service/auth.service';
import { NotificationService } from 'src/app/service/notification.service';
import { UserService } from 'src/app/service/user.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  userIsAuthenticated = false;
  constructor(private authService: AuthService, private notificationService: NotificationService, private router: Router,
    private userService: UserService, private cdr: ChangeDetectorRef) { }
// check user authentication
  ngOnInit() {
    this.userIsAuthenticated = this.authService.IsAuthenticated();
    this.authService
      .getAuthStatusListener()
      .subscribe(isAuthenticated => {
        this.userIsAuthenticated = isAuthenticated;
        this.cdr.detectChanges(); 
      });
  }

  
}
