--Aparent@joboverview.fr / @Pass1 
--Bnormand@joboverview.fr / @Pass2 
--Jrousse@joboverview.fr / @Pass3 
--lbregon@joboverview.fr / @Pass4
--proi@joboverview.fr / @Pass5

update AspNetUsers set id = 'APARENT', UserName = 'Agnès Parent'
where UserName = 'aparent@joboverview.fr'
update AspNetUsers set id = 'BNORMAND', UserName = 'Bertrand Normand'
where UserName = 'bnormand@joboverview.fr'
update AspNetUsers set id = 'JROUSSET', UserName = 'Joseph Rousset'
where UserName = 'jrousset@joboverview.fr'
update AspNetUsers set id = 'LBREGON', UserName = 'Laura Brégon'
where UserName = 'lbregon@joboverview.fr'
update AspNetUsers set id = 'PROI', UserName = 'Patrick Roi'
where UserName = 'proi@joboverview.fr'

select * from AspNetUsers


insert AspNetUserClaims (UserId, ClaimType, ClaimValue) values
('LBREGON', 'métier', 'DEV'),
('LBREGON', 'manager', ''),
('BNORMAND', 'métier', 'CDP'),
('BNORMAND', 'manager', ''),
('PROI', 'métier', 'CDS'),
('PROI', 'manager', '')