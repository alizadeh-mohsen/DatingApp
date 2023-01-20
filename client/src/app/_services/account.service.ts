import { User } from './../_models/User';
import { HttpClient, JsonpInterceptor } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private currentUsersource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUsersource.asObservable();

  baseUrl = 'http://localhost:5001/api/';

  constructor(private http: HttpClient) { }

  login(model: any) {
    return this.http.post<User>(this.baseUrl + 'account/login', model)
      .pipe(
        map((response: User) => {
          const user = response;
          if (user)
            localStorage.setItem('user', JSON.stringify(user));
          this.currentUsersource.next(user);
        })
      );
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUsersource.next(null);
  }

  setCurrentUser(user: User) {
    this.currentUsersource.next(user);
  }
}
