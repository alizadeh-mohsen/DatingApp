import { userParams } from './../../_models/userParams';
import { Pagination } from './../../_models/paginations';
import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { MemberService } from 'src/app/_services/member.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  members: Member[] = [];
  pagination: Pagination | undefined;
  userParams: userParams | undefined;
  genderList = [{ value: 'male', display: 'Males' }, { value: 'female', display: 'Females' }]
  orderByList = [{ value: 'created', display: 'Newest' }, { value: 'lastActive', display: 'Females' }]

  constructor(private memberService: MemberService) {
    this.userParams = this.memberService.getuserParams();
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    if (this.userParams) {
      this.memberService.setuserParams(this.userParams);
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
  }

  pageChanged(event: PageChangedEvent) {
    if (this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.memberService.setuserParams(this.userParams);
      this.loadMembers();
    }
  }

  resetFilters() {
    
      this.memberService.resetuserParams();
      this.loadMembers();
    
  }

}
