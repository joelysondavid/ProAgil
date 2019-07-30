import { Component, OnInit, TemplateRef } from '@angular/core';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale, BsLocaleService, ptBrLocale } from 'ngx-bootstrap';
import { TouchSequence } from 'selenium-webdriver';
import { templateJitUrl } from '@angular/compiler';
import { ToastrService } from 'ngx-toastr';
import { DateTimeFormatPipePipe } from '../_helps/DateTimeFormatPipe.pipe';
defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  titulo = 'Eventos';

  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  imagemLargura = 75;
  imagemMargem = 0;
  mostrarImagem = false;
  modalRef: BsModalRef;
  modoSalvar = 'post';
  // tslint:disable-next-line: variable-name
  _filtroLista = '';
  registerForm: FormGroup;
  bodyDeletarEvento = '';

  constructor(
      private eventoService: EventoService
    , private modalService: BsModalService
    , private fb: FormBuilder
    , private localeService: BsLocaleService
    , private toastr: ToastrService
    ) {
      this.localeService.use('pt-br');
      }

  get filtroLista(): string {
    return this._filtroLista;
  }
  set filtroLista(value: string) {
    this._filtroLista = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  // editar evento
  editarEvento(evento: Evento, template: any) {
    this.modoSalvar = 'put'; // flag para editar
    this.openModal(template); // abri o modal
    this.evento = evento; // passa os valores do evento clicado para o objeto local
    console.log(evento.dataEvento);
    this.registerForm.patchValue(evento); // carrega os dados do evento para o modal
    console.log(this.modoSalvar);
  }
  // novo evento
  novoEvento(template: any) {
    this.modoSalvar = 'post';
    this.openModal(template);
    console.log(this.modoSalvar);
  }


excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
}

confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Evento deletado com sucesso!');
        }, error => {
          this.toastr.error(`Erro ao tentar deletar evento: ${this.evento.tema}, Código: ${this.evento.id}!`);
          console.log(error);
        }
    );
}

  // metodo para mostrar o templete do modal
  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  // função para mostrar ou nao a imagem
  alternarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }

  validation() {
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      imagemURL: ['', Validators.required],
      qtdPessoas: ['',
      [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['',
      [Validators.required, Validators.email]]
    });
  }
  salvarAlteracao(template: any) {
    if (this.registerForm.valid){
      if(this.modoSalvar === 'post') {
        this.evento = Object.assign({}, this.registerForm.value);
        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            template.hide();
            this.getEventos();
            this.toastr.success('Evento Inserido com sucesso!');
          }, error => {
            this.toastr.error(`Erro ao inserir evento!`);
            console.log(error);
          }
        );
      } else {
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
        this.eventoService.putEvento(this.evento).subscribe(
          () => {
            template.hide();
            this.getEventos();
            this.toastr.success('Evento atualizado com sucesso!');
          }, error => {
            this.toastr.error(`Erro ao tentar atualizar evento: ${this.evento.tema}, Código: ${this.evento.id}!`);
            console.log(error);
          }
        );
      }
      }
  }

  getEventos() {
    this.eventoService.getAllEventos().subscribe(
      (_eventos: Evento[]) => {
      this.eventos = _eventos;
      this.eventosFiltrados = this.eventos;
    }, error => {
      this.toastr.error(`Erro ao tentar carregar eventos: ${error}`);
    }
    );
  }
}
