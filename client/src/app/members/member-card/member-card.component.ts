import { ToastrService } from 'ngx-toastr';
import { MemberService } from 'src/app/_services/member.service';
import { Member } from './../../_models/member';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {

  @Input() member: Member | undefined;
  constructor(private memberService: MemberService, private toastrService: ToastrService) { }

  ngOnInit(): void {
  }

  addLike(member: Member) {
    this.memberService.addLikes(member?.userName).subscribe({
      next: () => this.toastrService.success('You have liked ' + member.knownAs)
    });
  }
}
