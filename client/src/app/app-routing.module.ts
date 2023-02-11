import { MemberDetailedResolver } from './_resolver/member-detailed.resolver';
import { Member } from 'src/app/_models/member';
import { PreventChangeLossGuard } from './_guards/prevent-change-loss.guard';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberEditComponent } from './members/member-edit/member-edit.component';

const routes: Routes = [
  { path: "", component: HomeComponent },
  {
    path: "",
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: "members", component: MemberListComponent, canActivate: [AuthGuard] },
      { path: "members/:username", component: MemberDetailComponent, canActivate: [AuthGuard], resolve: { member: MemberDetailedResolver } },
      { path: "member/edit", component: MemberEditComponent, canActivate: [AuthGuard], canDeactivate: [PreventChangeLossGuard] },
      { path: "lists", component: ListsComponent, canActivate: [AuthGuard] },
      { path: "messages", component: MessagesComponent, canActivate: [AuthGuard] },]
  },
  { path: "not-found", component: NotFoundComponent },
  { path: "server-error", component: ServerErrorComponent },
  { path: "**", component: NotFoundComponent, pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
