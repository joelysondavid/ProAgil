// modulos
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter'; // Importação para utilizar filtro em todos os camppos da tabela
import { AppRoutingModule } from './app-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TooltipModule, TabsModule } from 'ngx-bootstrap';
import { NgxMaskModule } from 'ngx-mask';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { NgxCurrencyModule } from 'ngx-currency';

// componentes
import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { EventosComponent } from './eventos/eventos.component';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { ContatosComponent } from './contatos/contatos.component';
import { TituloComponent } from './_shared/titulo/titulo.component';
import { EventoEditComponent } from './eventos/eventoEdit/eventoEdit.component';
// pipe
import { DateTimeFormatPipePipe } from './_helps/DateTimeFormatPipe.pipe';
import { DatepickerModule, BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { ToastrModule } from 'ngx-toastr';
import { EventoService } from './_services/evento.service';

import { UserComponent } from './User/User.component';
// componentes internos de user
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthInterceptor } from './auth/auth.inserceptor';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      EventosComponent,
      EventoEditComponent,
      PalestrantesComponent,
      DashboardComponent,
      ContatosComponent,
      TituloComponent,
      UserComponent,
      LoginComponent,
      RegistrationComponent,
      DateTimeFormatPipePipe
   ],
   imports: [
      BrowserModule,
      //moduloparafiltro
      Ng2SearchPipeModule,
      BsDropdownModule.forRoot(),
      BsDatepickerModule.forRoot(),
      NgxCurrencyModule,
      TooltipModule.forRoot(),
      //ToastrModuleadded\\\\\\\\\\\\\\\\nTooltipModule.forRoot(),
      ModalModule.forRoot(),
      TabsModule.forRoot(),
      NgxMaskModule.forRoot(),
      BrowserAnimationsModule,
      ToastrModule.forRoot({
         timeOut: 2000,
         preventDuplicates: true,
         progressBar: true
      }),
      AppRoutingModule,
      HttpClientModule,
      FormsModule,
      DatepickerModule.forRoot(),
      ReactiveFormsModule
   ],
   providers: [
      EventoService,
      {
         provide: HTTP_INTERCEPTORS,
         useClass: AuthInterceptor,
         multi: true
      }
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
