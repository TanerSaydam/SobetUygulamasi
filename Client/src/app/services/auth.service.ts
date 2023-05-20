import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  router = inject(Router);

  checkLogin(){
    let userId = localStorage.getItem("userId");
    if(userId != undefined){
      return true
    }

    this.router.navigateByUrl("/login");
    return false;
  }
}
