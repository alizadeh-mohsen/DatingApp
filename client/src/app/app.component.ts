import { AccountService } from './_services/account.service';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  title = 'Dating App';
  users: any;

  constructor(private http: HttpClient, private accountService: AccountService) { }

  ngOnInit() {
    this.getUsers();
    this.setUser();

  }

  getUsers() {
    this.http.get('http://localhost:5001/api/Users').subscribe({
      next: response => {
        this.users = response;
        console.log(response);
      },
      error: error => console.log(error.error),
      complete: () => console.log('request completed')
    });
  }

  setUser() {
    const user = localStorage.getItem('user');
    if (user)
      this.accountService.setCurrentUser(JSON.parse(user));
    else
      return;

  }
}
