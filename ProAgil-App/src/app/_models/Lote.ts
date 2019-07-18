export interface Lote {
    id: number;
    nome: string;
    preco: number;
    dataIncio ?: Date;
    dataFim ?: Date;
    quantidade: number;
    eventoId: number;
}
