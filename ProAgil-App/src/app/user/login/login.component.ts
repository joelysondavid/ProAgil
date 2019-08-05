import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/_services/auth.service';
import { FormBuilder } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/_models/User';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  titulo = 'Login';
  model: any = {};
  constructor(
      private authService: AuthService
    , private router: Router
    , public fb: FormBuilder
    , private toastr: ToastrService) { }

  ngOnInit() {
    if(localStorage.getItem('token') !== null) {
      this.router.navigate(['/dashboard']);
    }
  }


  login() {
    this.authService.login(this.model).subscribe(
      () =>{
        this.router.navigate(['/dashboard']);
        this.toastr.success('Login realizado com sucesso!');
      },
      error => {
        this.toastr.error('Falha ao tentar logar');
      }
    )
  }

}
