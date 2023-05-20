import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserModel } from 'src/app/models/user.model';
import { HttpClient } from '@angular/common/http';
import { api } from 'src/app/app.config';
import { ActivatedRoute, Router } from '@angular/router';
import { MessageModel } from 'src/app/models/message.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  user: UserModel = new UserModel();
  users: UserModel[] = [];
  userId = localStorage.getItem("userId");
  userName = localStorage.getItem("userName");
  messages: MessageModel[]  = [];
  chatId: number = 0;
  text: string = "";

  http = inject(HttpClient);
  router = inject(Router);
  activatedRoute = inject(ActivatedRoute);

  constructor(){
    this.activatedRoute.params.subscribe(res=> {
      if(res["userId"] != undefined){
        this.getUser(res["userId"]);
      }
    })
  }

  ngOnInit(): void {
    this.getUsers();
  }

  selectUser(id: number){
    this.router.navigateByUrl("/" + id);
  }

  getUser(id: number){
    this.http.get<UserModel>(api + "GetUser/" + id).subscribe(res=> {
      this.user = res
      this.getChats();
    })
  }

  getUsers(){
    this.http.get<UserModel[]>(api + "GetUsers/" + this.userId)
      .subscribe((res)=> {
        this.users = res;
      });
      //www.taner.com/1
  }

  getChats(){
    this.http.post<any>(api + "GetChatMessages", {userId: this.userId, toUserId: this.user.id}).subscribe(res=> {
      console.log(res);
      this.messages = res.messages;
      this.chatId = res.chatId;
    });
  }

  sendMessage(){
    if(this.text.length == 0) return;

    this.http.post<any>(api + "PostMessage", {userId: this.userId, chatId: this.chatId, text: this.text}).subscribe(res=> {
      this.getChats();
    })
  }
}
