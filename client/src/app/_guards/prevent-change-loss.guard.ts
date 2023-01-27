
import { Injectable } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable({
  providedIn: 'root'
})
export class PreventChangeLossGuard implements CanDeactivate<MemberEditComponent> {

  canDeactivate(component: MemberEditComponent): boolean {
    if (component.editForm?.dirty)
      return confirm('If you leave all your changes will be lost. Continue?');
    return true;

  }

}
