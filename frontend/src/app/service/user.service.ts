import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { PageInput } from '../shared/pageInput';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }

  public addUser(user: User): Observable<User> {
    return this.http.post<User>(environment.server + 'api/user/createUser', user);
  }

  
  public addNewUser(user: User): Observable<User> {
    return this.http.post<User>(environment.server + 'api/user/addNewUser', user);
  }

  public deleteUser(id: string): Observable<User> {
    return this.http.delete<User>(environment.server + `api/user/${id}`);
  }
  public sendForgotPasswordMail(email: string): Observable<boolean> {
    return this.http.get<boolean>(environment.server + `api/user/sendForgotPasswordMail/${email}`);
  }
  getUsersByLazyLoad(page: PageInput): Observable<any> {
    return this.http.post<any>(`${environment.server}api/user/getUsersList`,page);
}
public checkUserExists(email: string): Observable<boolean> {
  return this.http.get<boolean>(environment.server + `api/user/checkUserExists/${email}`);
}
public changePassword(userIdentifier: string,user: User): Observable<User> {
  return this.http.post<User>(environment.server + `api/user/changePassword/${userIdentifier}`, user);
}
public getUserById(userIdentifier: string): Observable<User> {
  return this.http.get<User>(environment.server + `api/user/getUserById/${userIdentifier}`);
}
public updateUserEmail(user: User): Observable<User> {
  return this.http.post<User>(environment.server + `api/user/updateUserEmail`, user);
}
public updateUser(user: User): Observable<User> {
  return this.http.post<User>(environment.server + `api/user/updateUser`, user);
}
}
