import { Lote } from './Lote';
import { RedeSocial } from './RedeSocial';
import { Palestrante } from './Palestrante';

/**
 *
 */

export class Evento {
    constructor() { }
    id: number;
    local: string;
    dataEvento: string;
    tema: string;
    qtdPessoas: number;
    imagemURL: string;
    telefone: string;
    email: string;
    lotes: Lote[];
    redesSociais: RedeSocial[];
    palestrantes: Palestrante[];
}
