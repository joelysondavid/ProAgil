import { OnInit, Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { fromEvent } from 'rxjs';
import { User } from 'src/app/_models/User';
import { AuthService } from 'src/app/_services/auth.service';
import { Router } from '@angular/router';



@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registerForm: FormGroup;
  user: any = User;

  constructor(private authService: AuthService
    , private router: Router
    , public fb: FormBuilder
    , private toastr: ToastrService) {

  }

  ngOnInit() {
    this.validation();
  }

  validation() {
    this.registerForm = this.fb.group({
      fullName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      userName: ['', Validators.required],
      passwords: this.fb.group({
        password: ['', [Validators.required, Validators.minLength(4)]],
        confirmPassword: ['', [Validators.required, Validators.minLength(4)]]
      }, { validator: this.compararSenhas })
    });
  }

  compararSenhas(fb: FormGroup) {
    const confirmSenhaCtrl = fb.get('confirmPassword');
    const senhaCtrl = fb.get('password');
    // tslint:disable-next-line:max-line-length
    if ((senhaCtrl.errors == null || 'mismatch' in senhaCtrl.errors)) {
      if (fb.get('confirmPassword').value !== senhaCtrl.value) {
        fb.get('confirmPassword').setErrors({ mismatch: true });
      } else {
        confirmSenhaCtrl.setErrors(null);
      }
    }
  }

  cadastrarUsuario() {
    if (this.registerForm.valid) {
      this.user = Object.assign({
        password: this.registerForm.get('passwords.password').value
      },
        this.registerForm.value);
      this.authService.register(this.user).subscribe(
        () => {
          this.router.navigate(['/user/login']);
          this.toastr.success('Cadastro realizado com sucesso!');
        },
        error => {
          const erro = error.error;
          erro.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                this.toastr.error('Este usuário já está cadastrado!');
                break;
              default:
                this.toastr.error(`Erro no cadastro! Code: ${element.code}`);
            }
          });
        }
      )
    }
  }

}
