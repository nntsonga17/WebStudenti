export class Student{
    constructor(index, ime, prezime, predmet, rok, ocena){
        this.index = index;
        this.ime = ime;
        this.prezime = prezime;
        this.predmet = predmet;
        this.rok = rok;
        this.ocena = ocena;

    }

    crtaj(host){
        var tr = document.createElement("tr");
        host.appendChild(tr);

        var el = document.createElement("td");
        el.innerHTML= this.index;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML= this.ime;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML= this.prezime;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML= this.predmet;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML= this.rok;
        tr.appendChild(el);

        el = document.createElement("td");
        el.innerHTML= this.ocena;
        tr.appendChild(el);
    }
}