import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { MemberService } from './../../_services/member.service';
import { AccountService } from './../../_services/account.service';
import { Member } from './../../_models/member';
import { User } from './../../_models/User';
import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  user: User | null = null;
  member: Member | undefined;

  constructor(private accountService: AccountService, private memberService: MemberService, private Toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (!user) return;
        this.user = user;
      }
    })

  }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    })
  }

  updateMember() {
    
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        this.Toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member);
      }
    })

  }

}
