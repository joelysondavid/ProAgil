import { Component, OnInit } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(
    private authService: AuthService
    , public router: Router
    , private toastr: ToastrService
  ) { }

  ngOnInit() {
  }

  loggedIn(){
    if(localStorage.getItem('token') !== null) {
      return true;
    }
    return false;
  }

  logout() {
    localStorage.removeItem('token');
    this.toastr.show('Log Out');
    this.router.navigate(['/user/login']);
  }

  enter() {
    this.router.navigate(['/user/login/']);
  }

}
