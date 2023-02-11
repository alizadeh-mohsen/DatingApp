import { MessageService } from './../../_services/message.service';
import { NgForm } from '@angular/forms';
import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Message } from 'src/app/_models/message';

@Component({
  selector: 'app-member-messages',
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css']
})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @Input() username?: string;
  @Input() messages: Message[] = [];
  messageContent = '';

  constructor(private messageService: MessageService) { }

  ngOnInit(): void {

  }

  sendMessage() {
    console.log(this.username);
    
    if (!this.username) return;

    this.messageService.sendMessage(this.username, this.messageContent).subscribe({
      next: message => {
        this.messages.push(message);
        this.messageForm?.reset();
      }
    });
  }

}
