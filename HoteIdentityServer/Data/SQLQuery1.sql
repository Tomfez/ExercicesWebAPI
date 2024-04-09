--Aparent@joboverview.fr / @Pass1 
--Bnormand@joboverview.fr / @Pass2 
--Jrousse@joboverview.fr / @Pass3 

update AspNetUsers set id = 'APARENT', UserName = 'Agnès Parent'
where UserName = 'aparent@joboverview.fr'
update AspNetUsers set id = 'BNORMAND', UserName = 'Bertrand Normand'
where UserName = 'bnormand@joboverview.fr'
update AspNetUsers set id = 'JROUSSET', UserName = 'Joseph Rousset'
where UserName = 'jrousset@joboverview.fr'

select * from AspNetUsers