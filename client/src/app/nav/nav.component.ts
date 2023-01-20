import { HttpClient } from '@angular/common/http';
import { AccountService } from './../_services/account.service';
import { Component, OnInit } from '@angular/core';
import { User } from '../_models/User';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  model: any = {};
  user: any = {
    username: '',
    token: ''
  };

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
  }

  login() {

    this.accountService.login(this.model).subscribe(
     res => this.user = res
    )
  }

  logout() {
    this.accountService.logout();
  }
}
