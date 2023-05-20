import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserModel } from 'src/app/models/user.model';
import { HttpClient } from '@angular/common/http';
import { api } from 'src/app/app.config';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  user: UserModel = new UserModel();
  users: UserModel[] = [];
  userId = localStorage.getItem("userId");
  userName = localStorage.getItem("userName");

  http = inject(HttpClient);
  router = inject(Router);

  ngOnInit(): void {
    this.getUsers();
  }

  selectUser(id: number){
    this.router.navigateByUrl("/" + id);
  }

  getUsers(){
    this.http.get<UserModel[]>(api + "GetUsers/" + this.userId)
      .subscribe((res)=> {
        this.users = res;
      });
      //www.taner.com/1
  }
}
