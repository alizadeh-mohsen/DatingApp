import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  templateUrl: './server-error.component.html',
  styleUrls: ['./server-error.component.css']
})
export class ServerErrorComponent  {

  erroDetail: any;

  constructor(public route: ActivatedRoute,private router: Router) {
    this.erroDetail = this.router.getCurrentNavigation()?.extras.state;
      console.log(this.erroDetail);
      
   }

}
