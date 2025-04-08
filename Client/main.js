import { Rok } from "./Rok.js";
import { Predmet } from "./Predmet.js";
import { Fakultet } from "./Fakultet.js";

var listaPredmeta=[];

fetch("http://localhost:5288/api/Predmet/PreuzmiPredmete")
.then(p=>{
    p.json().then(predmeti=>{
        predmeti.forEach(predmet=>{
            
            var p = new Predmet(predmet.id, predmet.naziv);
            listaPredmeta.push(p);
        })

        var listaRokova=[];
fetch("http://localhost:5288/api/Ispit/IspitniRokovi")
.then(p=>{
    p.json().then(rokovi=>{
        rokovi.forEach(rok=>{
            
            var p = new Rok(rok.id, rok.naziv);
            listaRokova.push(p);
        })
        var f = new Fakultet(listaPredmeta, listaRokova);
        f.crtaj(document.body);
    })
})

        
    })
})
//console.log(listaPredmeta);



