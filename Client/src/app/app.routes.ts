import { inject } from '@angular/core';
import { Routes } from '@angular/router';
import { AuthService } from './services/auth.service';

export const routes: Routes = [
    {
        path: "login",
        loadComponent: ()=> import("./components/login/login.component").then(c=> c.LoginComponent)
    },
    {
        path: "",
        loadComponent: ()=> import("./components/home/home.component").then(c=> c.HomeComponent),
        canActivate: [()=> inject(AuthService).checkLogin()]
    },
    {
        path: ":userId",
        loadComponent: ()=> import("./components/home/home.component").then(c=> c.HomeComponent),
        canActivate: [()=> inject(AuthService).checkLogin()]
    },
];
