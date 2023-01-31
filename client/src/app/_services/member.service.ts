import { Photo } from './../_models/photo';
import { environment } from './../../environments/environment';

import { Member } from './../_models/member';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  members: Member[] = [];

  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }

  getMembers() {
    if (this.members.length > 0)
      return of(this.members);

    return this.http.get<Member[]>(this.baseUrl + 'users').pipe(
      map(response => {
        this.members = response;
        return response;
      })
    );
  }

  getMember(username: string) {
    const member = this.members.find(m => m.userName === username);
    if (member) return of(member);

    return this.http.get<Member>(this.baseUrl + 'users/' + username);
  }

  updateMember(member: Member) {
    return this.http.put(this.baseUrl + 'users', member).pipe(
      map(() => {
        const index = this.members.indexOf(member);
        this.members[index] = {
          ...this.members[index],
          ...member
        }
      })
    );
  }

  setMainPhoto(photoId: number) {
    return this.http.put(this.baseUrl + 'users/set-main-photo/'+ photoId,{});
  }

  deletePhoto(PhotoId:number)
  {
    return this.http.delete(this.baseUrl+'users/delete-photo/'+PhotoId);
  }

}
