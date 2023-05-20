import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { UserModel } from 'src/app/models/user.model';
import { api } from 'src/app/app.config';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  userName: string = "";

  http = inject(HttpClient);
  router = inject(Router);

  login() {
    this.http.post<UserModel>(api + "Login", { userName: this.userName }).subscribe({
      next: (res) => {
        localStorage.setItem("userId", res.id.toString());
        localStorage.setItem("userName", res.userName.toString());
        this.router.navigateByUrl("/");
      },
      error: (err: HttpErrorResponse) => {
        alert(err.error.message);
      }
    });
  }
}
