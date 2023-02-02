import { Router } from '@angular/router';
import { AccountService } from './../_services/account.service';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, ValidatorFn, Validators } from '@angular/forms';


@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  @Output() cancelRegister = new EventEmitter();
  registerForm: FormGroup = new FormGroup({});
  validationErrors: string[] | undefined;

  constructor(private accountService: AccountService,
    private router: Router,
    private fb: FormBuilder) { }
      date: any;

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm() {
    this.registerForm = this.fb.group({
      gender:['male'],
      username: ['', Validators.required],
      knownAs: ['', Validators.required],
      dateOfBirth: ['', Validators.required],
      city: ['', Validators.required],
      country: ['', Validators.required],
      password: ['', Validators.required],
      confirmPassword: ['', [Validators.required, this.compareValueWith('password')]]
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })
  }

  compareValueWith(controlToCompare: string): ValidatorFn {
    return (control: AbstractControl) => {
      return control.value === control.parent?.get(controlToCompare)?.value ? null : { notMatching: true }
    }
  }

  register() {
    
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const formValues = { ...this.registerForm.value, dateOfBirth: dob };

    this.accountService.register(formValues).subscribe({
      next: _ => { this.router.navigateByUrl('/members') },
      error: error => {
        this.validationErrors = error
      }
    });
  }
  cancel() {
    this.cancelRegister.emit(false);
  }

  getDateOnly(dob: string | undefined) {
    if (!dob) return
    let date = new Date(dob);
    return new Date(date.setMinutes(date.getMinutes() - date.getTimezoneOffset())).toISOString().slice(0, 10);
  }
}
