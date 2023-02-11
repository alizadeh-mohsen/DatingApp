import { Member } from 'src/app/_models/member';
import { MemberService } from 'src/app/_services/member.service';
import { Injectable } from '@angular/core';
import * as router from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemberDetailedResolver implements router.Resolve<Member> {
  constructor(private memberService: MemberService) { }

  resolve(route: router.ActivatedRouteSnapshot): Observable<Member> {
    return this.memberService.getMember(route.paramMap.get('username')!)
  }
}
