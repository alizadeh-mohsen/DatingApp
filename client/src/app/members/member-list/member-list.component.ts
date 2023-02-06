import { userParams } from './../../_models/userParams';
import { take } from 'rxjs';
import { AccountService } from './../../_services/account.service';
import { Pagination } from './../../_models/paginations';
import { MemberService } from './../../_services/member.service';
import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  pagination: Pagination | undefined;
  user: User | undefined;
  userParams: userParams | undefined;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]
  orderByList = [{ value: 'created', display: 'Newest' }, { value: 'lastActive', display: 'Females' }]

  constructor(private memberService: MemberService, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.userParams = new userParams(user);
          this.user = user;
        }
      }
    })

  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {

    if (!this.userParams) return;
    this.memberService.getMembers(this.userParams).subscribe({
      next: response => {
        console.log(response);

        if (response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      }
    })
  }

  pageChanged(event: PageChangedEvent) {
    if (this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.loadMembers();
    }
  }

  resetFilters() {
    if (this.user) {
      this.userParams = new userParams(this.user);
      this.loadMembers();
    }
  }

}
