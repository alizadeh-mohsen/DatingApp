import { Pagination } from './../../_models/paginations';
import { MemberService } from './../../_services/member.service';
import { Member } from './../../_models/member';
import { Component, OnInit } from '@angular/core';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  pageNumber = 1;
  pageSize = 5;
  members: Member[] = [];
  pagination: Pagination | undefined;

  constructor(private memberService: MemberService) { }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.memberService.getMembers(this.pageNumber, this.pageSize).subscribe({
      next: response => {
        if (response.result && response.pagination) {
          this.members = response.result;
          this.pagination = response.pagination;
        }
      }
    })
  }

  pageChanged(event: PageChangedEvent) {
    this.pageNumber = event.page;
    this.loadMembers();
  }

}
