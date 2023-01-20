import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { AccountService } from './../_services/account.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();

  model: any = {}

  constructor(private accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register() {
    this.accountService.register(this.model).subscribe({
      next: _ => this.router.navigateByUrl('/'),
      error: error => this.toastr.error(error.error
      )
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }
}
