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
import { datepickerAnimation } from 'ngx-bootstrap/datepicker/datepicker-animations';
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
  evento = new Evento();
  imagemLargura = 75;
  imagemMargem = 0;
  mostrarImagem = false;
  modalRef: BsModalRef;
  modoSalvar = 'post';
  // tslint:disable-next-line: variable-name
  _filtroLista = '';
  registerForm: FormGroup;
  bodyDeletarEvento = '';
  edit: string;

  dataAtual: string;

  file: File;
  fileNameToUpdate: any;

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
    this.mostrar();
    this.evento = Object.assign({}, evento); // passa os valores do evento clicado para o objeto local
    this.fileNameToUpdate = this.evento.imagemURL.toString();
    this.evento.imagemURL = ''; // seta url da imagem para não dar problema ao carregar as informações
    this.registerForm.patchValue(this.evento); // passa os dados do objeto e coloca no modal
    this.edit = `Evento: ${this.evento.tema}, Cód.: ${this.evento.id}`;
    // console.log(this.modoSalvar);
  }
  // novo evento
  novoEvento(template: any) {
    this.modoSalvar = 'post';
    this.edit = 'Novo Evento';
    this.openModal(template);
    // console.log(this.modoSalvar);
  }


  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }
  mostrar() {

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
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  onFileChange(event) {
    // letor de arquivo
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      this.file = event.target.files;
      console.log(this.file);
    }
  }

  uploadImagem() {
    if (this.modoSalvar === 'post') {// chamando upload
      const nomeArquivo = this.evento.imagemURL.split('\\', 3);

      this.evento.imagemURL = nomeArquivo[2];
      this.eventoService.postUpload(this.file, nomeArquivo[2]).subscribe(
        () => {
          this.dataAtual = new Date().getMilliseconds().toString();
          this.getEventos();
        }
      );
    } else {
      this.evento.imagemURL = this.fileNameToUpdate;
      this.eventoService.postUpload(this.file, this.fileNameToUpdate).subscribe(
        () => {
          this.dataAtual = new Date().getMilliseconds().toString();
          this.getEventos();
        }
      );
    }
  }

  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {
      if (this.modoSalvar === 'post') {
        this.evento = Object.assign({}, this.registerForm.value);

        this.uploadImagem();

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
        this.evento = Object.assign({ id: this.evento.id }, this.registerForm.value);

        this.uploadImagem();

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
    this.dataAtual = new Date().getMilliseconds().toString();
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
