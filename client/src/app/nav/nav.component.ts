import { Router } from '@angular/router';
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

  constructor(public accountService: AccountService, private router: Router) { }

  ngOnInit(): void {
  }

  login() {

    this.accountService.login(this.model).subscribe(
      {
        next: user => {
          if (user) {
            this.user = user;
            this.router.navigateByUrl('/members');
          }
          else
            this.user = "-"
        }
      }
    );


  }
  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }
}
