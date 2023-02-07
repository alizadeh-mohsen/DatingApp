import { AccountService } from './account.service';
import { userParams } from './../_models/userParams';
import { PaginatedResult } from './../_models/paginations';
import { environment } from './../../environments/environment';

import { Member } from './../_models/member';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, of, take } from 'rxjs';
import { User } from '../_models/User';

@Injectable({
  providedIn: 'root'
})
export class MemberService {
  members: Member[] = [];
  baseUrl = environment.baseUrl;
  memberCache = new Map();
  user: User | undefined;
  userParams: userParams | undefined;


  constructor(private http: HttpClient, private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) {
          this.userParams = new userParams(user);
          this.user = user;
        }
      }
    })

  }
  setuserParams(params: userParams) {
    this.userParams = params;
  }

  getuserParams() {
    return this.userParams;
  }
  resetuserParams() {
    if (this.user) {
      this.userParams = new userParams(this.user);
      return this.userParams;
    }
    return;
  }

  getMembers(userParams: userParams) {
    const cachedResponse = this.memberCache.get(Object.values(userParams).join('-'));
    if (cachedResponse) return of(cachedResponse);

    let params = this.getPaginationHeaders(userParams.pageNumber, userParams.pageSize);

    params = params.append('gender', userParams.gender);
    params = params.append('minAge', userParams.minAge);
    params = params.append('maxAge', userParams.maxAge);
    params = params.append('orderby', userParams.orderBy);

    return this.getPaginatedResult<Member[]>(this.baseUrl + 'users', params).pipe(
      map(response => {
        this.memberCache.set(Object.values(userParams).join('-'), response);
        return response;
      })
    );
  }

  getMember(username: string) {

    const member = [...this.memberCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((member: Member) => member.userName === username);

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
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photoId, {});
  }

  deletePhoto(PhotoId: number) {
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + PhotoId);
  }

  private getPaginatedResult<T>(url: string, params: HttpParams) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>;
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body;
        }
        const pagination = response.headers.get('Pagination');
        if (pagination) { paginatedResult.pagination = JSON.parse(pagination); }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber);
    params = params.append('pageSize', pageSize);
    return params;
  }
}
