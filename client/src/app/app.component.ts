import { AccountService } from './_services/account.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  constructor(private accountService: AccountService) { }

  ngOnInit() {
    this.setUser();
  }

  setUser() {
    const user = localStorage.getItem('user');
    if (user)
      this.accountService.setCurrentUser(JSON.parse(user));
    else
      return;
  }
}
