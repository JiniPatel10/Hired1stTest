import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Router } from '@angular/router';
import { NotificationService } from './notification.service';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {


  constructor(private http: HttpClient,  
    private router: Router,
    private notificationService :NotificationService,
    ) {

  }
  private isAuthenticated = false;
  private tokenTimer: any; // variable of TimeOut timer
  private authStatusListener = new Subject<boolean>(); // user authentication status listener


  isLoggedIn(): boolean {
    return this.isAuthenticated;
  }

  public loginUser(user: User) {
     this.http.post<any>(environment.server + 'api/user/login', user)
    .subscribe({
      next:(response) => {
      const token = response.token; 
      if (token) { 
        const expirationDate = response.expiresIn;
        const userIdentifier = response.id;
        this.saveAuthData(token, expirationDate, userIdentifier);
        this.setAuthTimer(3600); // 1 hr
        this.isAuthenticated = true; 
        this.authStatusListener.next(true);
        this.router.navigate(['/users']);
      }
    },
    error:(error) => {
      if (error.status == 422) {
        this.notificationService.showMessage('error', true, `${error.error}`, '');
      } else {
        this.notificationService.showMessage('error', true, `${error.status}
    - ${error.statusText} - ${error.error}`, '');
      }
    }
  });
  }

    /**
   * Private method used to store token in browsers local stroge.
   */
    private saveAuthData(token: string, expirationDate: Date, userIdentifier: string) {
      localStorage.setItem('token', token); 
      localStorage.setItem('expiration', expirationDate.toString()); 
      localStorage.setItem('userIdentifier', userIdentifier); 
    }
    /**
     * Method to clear local storage.
     */
    private clearAuthData() {
      localStorage.removeItem('token');
      localStorage.removeItem('expiration');
      localStorage.removeItem('userIdentifier');
    }

  /**
   * Timer function to call logout method after 1h expires
   */
  private setAuthTimer(duration: number) {
    this.tokenTimer = setTimeout(() => { 
      this.logout();
    }, duration * 1000);
  }

 /**
   * Logout method. Before user is logged out
   * it first deletes token, then emits that
   * information through authStatusListener
   * and clears timeout timer
   */
 logout() {
  this.clearAuthData(); 
  this.isAuthenticated = false;
  clearTimeout(this.tokenTimer);
  if (!this.isAuthenticated) {
    this.authStatusListener.next(false);
    this.router.navigate(['/login']); 
  }
}
 /*
  * Method which returns user's token. 
  * used in posts.service.ts
  * */
 getToken() {
  let token = localStorage.getItem('token'); 
  return token;
}
getIsAuth() {
  return this.isAuthenticated;
}
getUserIdentifier() {
  let userIdentifier = localStorage.getItem('userIdentifier'); 
  return userIdentifier;
}

getAuthStatusListener() {
  return this.authStatusListener.asObservable(); 
}

public IsAuthenticated() : boolean {
  let userIdentifier = localStorage.getItem('userIdentifier');
  return (userIdentifier != "" && userIdentifier != undefined && userIdentifier != null)  ? true : false;
}

public login(redirect?: string): void { 
    localStorage.setItem('authRedirect', redirect ? redirect : ''); 
}


}

