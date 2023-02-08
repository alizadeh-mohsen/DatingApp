import { MemberService } from 'src/app/_services/member.service';
import { Member } from './../_models/member';
import { Component, OnInit } from '@angular/core';
import { Pagination } from '../_models/paginations';
import { userParams } from '../_models/userParams';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent implements OnInit {
  members: Member[] | undefined;
  predicate = 'liked';
  pagination: Pagination | undefined;
  userParams: userParams | undefined;

  constructor(private memberService: MemberService) {
    this.userParams = this.memberService.getuserParams();
  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    if (this.userParams) {
      this.memberService.getLikes(this.predicate,
        this.userParams.pageNumber, this.userParams.pageSize).subscribe({
          next: response => {
            this.members = response.result;
            this.pagination = response.pagination;
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
}
