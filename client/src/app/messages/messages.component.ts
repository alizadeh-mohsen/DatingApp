import { MessageService } from './../_services/message.service';
import { Component, OnInit } from '@angular/core';
import { Message } from '../_models/message';
import { Pagination } from '../_models/paginations';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages?: Message[];
  pagination?: Pagination;
  container = 'Unread';
  pageNumber = 1;
  pageSize = 5;
  loading = true;

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {
    this.getMessages();
  }

  getMessages() {

    console.log('inside get');
    this.loading = true;
    this.messageService.getMessages(this.pageNumber, this.pageSize, this.container).subscribe({
      next: response => {
        this.messages = response.result;
        this.pagination = response.pagination;
        this.loading = false;
      }
    })
  }

  
  deleteMessage(id:number){
    this.messageService.deleteMessage(id).subscribe({
      next: _=>{
        this.messages?.splice(this.messages.findIndex(x=>x.id===id),1)
      }
    })
  }

  pageChanged(event: PageChangedEvent) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.getMessages();
    }
  }


}
