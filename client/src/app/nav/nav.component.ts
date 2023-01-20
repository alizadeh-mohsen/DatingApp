import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  loggedIn = false;
  user: any = {};

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
  }

  login() {
    console.log(this.user);
    this.accountService.login(this.user).subscribe({
      next: response => {
        this.loggedIn = true; 
      },
      error: error => console.log(error.error),
      complete: () => console.log('login completed')
    })
  }

  logout() { }
}
