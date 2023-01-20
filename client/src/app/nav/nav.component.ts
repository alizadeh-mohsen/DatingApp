import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  user: any;

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  login() {

    this.accountService.login(this.model).subscribe(
      {
        next: user => {
          if (user)
            this.user = user
            else
            this.user="-"
        }
      }
    );


  }
  logout() {
    this.accountService.logout();
  }
}
