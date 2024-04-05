print 'Table Activites'
insert Activites (Code, Titre) values
('DBE', 'Définition des besoins'),
('ARF', 'Architecture fonctionnelle'),
('ANF', 'Analyse fonctionnelle'),
('DES', 'Design'),
('INF', 'Infographie'),
('ART', 'Architecture technique'),
('ANT', 'Analyse technique'),
('DEV', 'Développement'),
('RPT', 'Rédaction de plan de test'),
('TES', 'Test')
go

print 'Table ActivitseMetiers'
insert ActivitesMetiers (CodeMetier, CodeActivite) values
('ANA', 'DBE'),('ANA', 'ARF'),('ANA', 'ANF'),
('CDP', 'ARF'),('CDP', 'ANF'),('CDP', 'ART'),('CDP', 'TES'),
('DEV', 'ANF'),('DEV', 'ART'),('DEV', 'ANT'),('DEV', 'DEV'),('DEV', 'TES'),
('DES', 'ANF'),('DES', 'DES'),('DES', 'INF'),
('TES', 'RPT'),('TES', 'TES')
go

print 'Table Taches'
insert Taches (Titre, CodeActivite, Personne, DureePrevue, DureeRestante, CodeModule, CodeLogiciel, NumVersion) values
('AF Marquage', 'ANF', 'RBEAUMONT', 48, 0, 'MARQUAGE', 'GENOMICA', 1.0),
('AT Marquage', 'ANT', 'RBEAUMONT', 96, 0, 'MARQUAGE', 'GENOMICA', 1.0),
('DEV Marquage', 'DEV', 'RBEAUMONT', 80, 0, 'MARQUAGE', 'GENOMICA', 1.0),
 
('AF Paramétrage', 'ANF', 'RFORTIER', 16, 0, 'PARAMETRES', 'GENOMICA', 1.0),
('AT Paramétrage', 'ANT', 'RFORTIER', 48, 0, 'PARAMETRES', 'GENOMICA', 1.0),
('DEV Paramétrage', 'DEV', 'RFORTIER', 40, 0, 'PARAMETRES', 'GENOMICA', 1.0),
 
('AF Polymorphisme génétique', 'ANF', 'MWEBER', 80, 0, 'POLYMORPHISME', 'GENOMICA', 1.0),
('AT Polymorphisme génétique', 'ANT', 'MWEBER', 40, 0, 'POLYMORPHISME', 'GENOMICA', 1.0),
('DEV Polymorphisme génétique', 'DEV', 'MWEBER', 50, 0, 'POLYMORPHISME', 'GENOMICA', 1.0),
 
('AF Séparation', 'ANF', 'RBEAUMONT', 40, 0, 'SEPARATION', 'GENOMICA', 1.0),
('AT Séparation', 'ANT', 'RBEAUMONT', 32, 0, 'SEPARATION', 'GENOMICA', 1.0),
('DEV Séparation', 'DEV', 'RBEAUMONT', 8, 0, 'SEPARATION', 'GENOMICA', 1.0),
 
('AF Séquençage', 'ANF', 'RFORTIER', 32, 0, 'SEQUENCAGE', 'GENOMICA', 1.0),
('AT Séquençage', 'ANT', 'RFORTIER', 72, 0, 'SEQUENCAGE', 'GENOMICA', 1.0),
('DEV Séquençage', 'DEV', 'RFORTIER', 96, 0, 'SEQUENCAGE', 'GENOMICA', 1.0),
 
('AF Utilisateurs et droits', 'ANF', 'MWEBER', 40, 0, 'UTILS_ROLES', 'GENOMICA', 1.0),
('AT Utilisateurs et droits', 'ANT', 'MWEBER', 72, 0, 'UTILS_ROLES', 'GENOMICA', 1.0),
('DEV Utilisateurs et droits', 'DEV', 'MWEBER', 72, 0, 'UTILS_ROLES', 'GENOMICA', 1.0),
 
('AF Variations alléliques', 'ANF', 'RBEAUMONT', 16, 0, 'VAR_ALLELE', 'GENOMICA', 1.0),
('AT Variations alléliques', 'ANT', 'RBEAUMONT', 40, 0, 'VAR_ALLELE', 'GENOMICA', 2.0),
('DEV Variations alléliques', 'DEV', 'RBEAUMONT', 64, 0, 'VAR_ALLELE', 'GENOMICA', 2.0),

--V2
('AF Marquage', 'ANF', 'RBEAUMONT', 64, 0, 'MARQUAGE', 'GENOMICA', 2.0),
('AT Marquage', 'ANT', 'RBEAUMONT', 48, 0, 'MARQUAGE', 'GENOMICA', 2.0),
('DEV Marquage', 'DEV', 'RBEAUMONT', 56, 0, 'MARQUAGE', 'GENOMICA', 2.0),
 
('AF Paramétrage', 'ANF', 'RFORTIER', 80, 0, 'PARAMETRES', 'GENOMICA', 2.0),
('AT Paramétrage', 'ANT', 'RFORTIER', 72, 0, 'PARAMETRES', 'GENOMICA', 2.0),
('DEV Paramétrage', 'DEV', 'RFORTIER', 64, 0, 'PARAMETRES', 'GENOMICA', 2.0),
 
('AF Polymorphisme génétique', 'ANF', 'MWEBER', 40, 0, 'POLYMORPHISME', 'GENOMICA', 2.0),
('AT Polymorphisme génétique', 'ANT', 'MWEBER', 48, 0, 'POLYMORPHISME', 'GENOMICA', 2.0),
('DEV Polymorphisme génétique', 'DEV', 'MWEBER', 32, 0, 'POLYMORPHISME', 'GENOMICA', 2.0),
 
('AF Séparation', 'ANF', 'RBEAUMONT', 80, 0, 'SEPARATION', 'GENOMICA', 2.0),
('AT Séparation', 'ANT', 'RBEAUMONT', 64, 0, 'SEPARATION', 'GENOMICA', 2.0),
('DEV Séparation', 'DEV', 'RBEAUMONT', 48, 0, 'SEPARATION', 'GENOMICA', 2.0),
 
('AF Séquençage', 'ANF', 'RFORTIER', 32, 0, 'SEQUENCAGE', 'GENOMICA', 2.0),
('AT Séquençage', 'ANT', 'RFORTIER', 56, 0, 'SEQUENCAGE', 'GENOMICA', 2.0),
('DEV Séquençage', 'DEV', 'RFORTIER', 72, 0, 'SEQUENCAGE', 'GENOMICA', 2.0),
 
('AF Utilisateurs et droits', 'ANF', 'MWEBER', 40, 0, 'UTILS_ROLES', 'GENOMICA', 2.0),
('AT Utilisateurs et droits', 'ANT', 'MWEBER', 32, 0, 'UTILS_ROLES', 'GENOMICA', 2.0),
('DEV Utilisateurs et droits', 'DEV', 'MWEBER', 48, 0, 'UTILS_ROLES', 'GENOMICA', 2.0),
 
('AF Variations alléliques', 'ANF', 'RBEAUMONT', 16, 0, 'VAR_ALLELE', 'GENOMICA', 2.0),
('AT Variations alléliques', 'ANT', 'RBEAUMONT', 40, 0, 'VAR_ALLELE', 'GENOMICA', 2.0),
('DEV Variations alléliques', 'DEV', 'RBEAUMONT', 64, 0, 'VAR_ALLELE', 'GENOMICA', 2.0)
go

-- Travaux

print 'Table Travaux sur module marquage de la V2'
insert Travaux (IdTache, DateTravail, Heures) values
-- AF
(22, '2023-01-02', 2),
(22, '2023-01-03', 5),
(22, '2023-01-04', 4),
(22, '2023-01-05', 4),
(22, '2023-01-06', 3),
(22, '2023-01-09', 2),
(22, '2023-01-10', 8),
(22, '2023-01-11', 6),
(22, '2023-01-12', 6),
(22, '2023-01-13', 2),
(22, '2023-01-16', 6),
(22, '2023-01-18', 2),
(22, '2023-01-19', 8),
(22, '2023-01-20', 4),
-- AT
(23, '2023-01-02', 3),
(23, '2023-01-03', 2),
(23, '2023-01-04', 2),
(23, '2023-01-05', 1),
(23, '2023-01-06', 1),
(23, '2023-01-09', 3),
(23, '2023-01-10', 8),
(23, '2023-01-11', 2),
(23, '2023-01-12', 1),
(23, '2023-01-13', 2),
(23, '2023-01-18', 2),
(23, '2023-01-20', 2),
-- DEV
(24, '2023-01-02', 3),
(24, '2023-01-03', 1),
(24, '2023-01-04', 2),
(24, '2023-01-05', 3),
(24, '2023-01-06', 4),
(24, '2023-01-09', 3),
(24, '2023-01-12', 1),
(24, '2023-01-13', 4),
(24, '2023-01-16', 2),
(24, '2023-01-18', 4),
(24, '2023-01-20', 2)
go

print 'Table Travaux sur module PARAMETRES de la V2'
insert Travaux (IdTache, DateTravail, Heures) values
--AF
(25, '2022-01-23', 2),
(25, '2022-01-24', 4),
(25, '2022-01-25', 2),
(25, '2022-01-26', 1),
(25, '2022-01-27', 3),
(25, '2022-01-30', 5),
(25, '2022-01-31', 2),
(25, '2022-02-01', 3),
(25, '2022-02-02', 2),
(25, '2022-02-03', 4),
(25, '2022-02-06', 1),
(25, '2022-02-07', 2),
(25, '2022-02-08', 3),
(25, '2022-02-09', 2),
(25, '2022-02-10', 1),
(25, '2022-02-13', 1),
(25, '2022-02-14', 2),
(25, '2022-02-15', 2),
(25, '2022-02-16', 1),
(25, '2022-02-17', 1),
--AT
(26, '2022-01-23', 2),
(26, '2022-01-24', 1),
(26, '2022-01-25', 5),
(26, '2022-01-26', 5),
(26, '2022-01-27', 3),
(26, '2022-01-30', 1),
(26, '2022-01-31', 2),
(26, '2022-02-01', 1),
(26, '2022-02-02', 5),
(26, '2022-02-03', 2),
(26, '2022-02-06', 4),
(26, '2022-02-07', 3),
(26, '2022-02-08', 4),
(26, '2022-02-09', 4),
(26, '2022-02-10', 1),
(26, '2022-02-13', 2),
(26, '2022-02-14', 4),
(26, '2022-02-15', 3),
(26, '2022-02-16', 5),
(26, '2022-02-17', 4),
--DEV
(27, '2022-01-23', 4),
(27, '2022-01-24', 3),
(27, '2022-01-25', 1),
(27, '2022-01-26', 2),
(27, '2022-01-27', 2),
(27, '2022-01-30', 2),
(27, '2022-01-31', 4),
(27, '2022-02-01', 4),
(27, '2022-02-02', 1),
(27, '2022-02-03', 2),
(27, '2022-02-06', 3),
(27, '2022-02-07', 3),
(27, '2022-02-08', 1),
(27, '2022-02-09', 2),
(27, '2022-02-10', 6),
(27, '2022-02-13', 5),
(27, '2022-02-14', 2),
(27, '2022-02-15', 3),
(27, '2022-02-16', 2),
(27, '2022-02-17', 3)
go

print 'Table Travaux sur module POLYMORPHISME de la V2'
insert Travaux (IdTache, DateTravail, Heures) values
--AF
(28, '2022-02-20', 2),
(28, '2022-02-21', 3),
(28, '2022-02-22', 4),
(28, '2022-02-23', 3),
(28, '2022-02-24', 5),
(28, '2022-02-27', 2),
(28, '2022-02-28', 3),
(28, '2022-03-01', 4),
(28, '2022-03-02', 3),
(28, '2022-03-03', 5),
--AT
(29, '2022-02-20', 3),
(29, '2022-02-21', 4),
(29, '2022-02-22', 2),
(29, '2022-02-23', 3),
(29, '2022-02-24', 1),
(29, '2022-02-27', 4),
(29, '2022-02-28', 1),
(29, '2022-03-01', 3),
(29, '2022-03-02', 4),
(29, '2022-03-03', 1),
--DEV
(30, '2022-02-20', 3),
(30, '2022-02-21', 1),
(30, '2022-02-22', 2),
(30, '2022-02-23', 2),
(30, '2022-02-24', 2),
(30, '2022-02-27', 2),
(30, '2022-02-28', 4),
(30, '2022-03-01', 1),
(30, '2022-03-02', 1),
(30, '2022-03-03', 2)
go

print 'Table Travaux sur module séparation de la V2'
insert Travaux (IdTache, DateTravail, Heures) values
-- AF
(31, '2023-01-02', 2),
(31, '2023-01-03', 5),
(31, '2023-01-04', 4),
(31, '2023-01-05', 4),
(31, '2023-01-06', 3),
(31, '2023-01-09', 2),
(31, '2023-01-10', 8),
(31, '2023-01-11', 6),
(31, '2023-01-12', 6),
(31, '2023-01-13', 2),
(31, '2023-01-16', 6),
(31, '2023-01-18', 2),
(31, '2023-01-19', 8),
(31, '2023-01-20', 4),
-- AT
(32, '2023-01-02', 3),
(32, '2023-01-03', 2),
(32, '2023-01-04', 2),
(32, '2023-01-05', 1),
(32, '2023-01-06', 1),
(32, '2023-01-09', 3),
(32, '2023-01-10', 8),
(32, '2023-01-11', 2),
(32, '2023-01-12', 1),
(32, '2023-01-13', 2),
(32, '2023-01-18', 2),
(32, '2023-01-20', 2),
-- DEV
(33, '2023-01-02', 3),
(33, '2023-01-03', 1),
(33, '2023-01-04', 2),
(33, '2023-01-05', 3),
(33, '2023-01-06', 4),
(33, '2023-01-09', 3),
(33, '2023-01-12', 1),
(33, '2023-01-13', 4),
(33, '2023-01-16', 2),
(33, '2023-01-18', 4),
(33, '2023-01-20', 2)
go

print 'Table Travaux sur module SEQUENCAGE de la V2'
insert Travaux (IdTache, DateTravail, Heures) values
--AF
(34, '2022-01-23', 2),
(34, '2022-01-24', 4),
(34, '2022-01-25', 2),
(34, '2022-01-26', 1),
(34, '2022-01-27', 3),
(34, '2022-01-30', 5),
(34, '2022-01-31', 2),
(34, '2022-02-01', 3),
(34, '2022-02-02', 2),
(34, '2022-02-03', 4),
(34, '2022-02-06', 1),
(34, '2022-02-07', 2),
(34, '2022-02-08', 3),
(34, '2022-02-09', 2),
(34, '2022-02-10', 1),
(34, '2022-02-13', 1),
(34, '2022-02-14', 2),
(34, '2022-02-15', 2),
(34, '2022-02-16', 1),
(34, '2022-02-17', 1),
--AT
(35, '2022-01-23', 2),
(35, '2022-01-24', 1),
(35, '2022-01-25', 5),
(35, '2022-01-26', 5),
(35, '2022-01-27', 3),
(35, '2022-01-30', 1),
(35, '2022-01-31', 2),
(35, '2022-02-01', 1),
(35, '2022-02-02', 5),
(35, '2022-02-03', 2),
(35, '2022-02-06', 4),
(35, '2022-02-07', 3),
(35, '2022-02-08', 4),
(35, '2022-02-09', 4),
(35, '2022-02-10', 1),
(35, '2022-02-13', 2),
(35, '2022-02-14', 4),
(35, '2022-02-15', 3),
(35, '2022-02-16', 5),
(35, '2022-02-17', 4),
--DEV
(36, '2022-01-23', 4),
(36, '2022-01-24', 3),
(36, '2022-01-25', 1),
(36, '2022-01-26', 2),
(36, '2022-01-27', 2),
(36, '2022-01-30', 2),
(36, '2022-01-31', 4),
(36, '2022-02-01', 4),
(36, '2022-02-02', 1),
(36, '2022-02-03', 2),
(36, '2022-02-06', 3),
(36, '2022-02-07', 3),
(36, '2022-02-08', 1),
(36, '2022-02-09', 2),
(36, '2022-02-10', 6),
(36, '2022-02-13', 5),
(36, '2022-02-14', 2),
(36, '2022-02-15', 3),
(36, '2022-02-16', 2),
(36, '2022-02-17', 3)
go

print 'Table Travaux sur module UTILS_ROLES de la V2'
insert Travaux (IdTache, DateTravail, Heures) values
--AF
(37, '2022-02-20', 2),
(37, '2022-02-21', 3),
(37, '2022-02-22', 4),
(37, '2022-02-23', 3),
(37, '2022-02-24', 5),
(37, '2022-02-27', 2),
(37, '2022-02-28', 3),
(37, '2022-03-01', 4),
(37, '2022-03-02', 3),
(37, '2022-03-03', 5),
--AT
(38, '2022-02-20', 3),
(38, '2022-02-21', 4),
(38, '2022-02-22', 2),
(38, '2022-02-23', 3),
(38, '2022-02-24', 1),
(38, '2022-02-27', 4),
(38, '2022-02-28', 1),
(38, '2022-03-01', 3),
(38, '2022-03-02', 4),
(38, '2022-03-03', 1),
--DEV
(39, '2022-02-20', 3),
(39, '2022-02-21', 1),
(39, '2022-02-22', 2),
(39, '2022-02-23', 2),
(39, '2022-02-24', 2),
(39, '2022-02-27', 2),
(39, '2022-02-28', 4),
(39, '2022-03-01', 1),
(39, '2022-03-02', 1),
(39, '2022-03-03', 2)
go

-- MWEBER 
print 'Table Travaux pour MWEBER sur V1'
insert Travaux (IdTache, DateTravail, Heures) values
(16, '2022-01-07', 8),
(16, '2022-01-08', 2),
(16, '2022-01-09', 6),
(16, '2022-01-10', 6),
(16, '2022-01-11', 2),
(16, '2022-01-12', 8),
(16, '2022-01-13', 4),
(16, '2022-01-14', 4),

(17, '2022-01-08', 6),
(17, '2022-01-09', 2),
(17, '2022-01-10', 2),
(17, '2022-01-11', 6),
(17, '2022-01-13', 4),
(17, '2022-01-14', 4),
(17, '2022-01-15', 8),
(17, '2022-01-16', 8),
(17, '2022-01-17', 8),
(17, '2022-01-18', 8),

(18, '2022-01-19', 6),
(18, '2022-01-20', 2),
(18, '2022-01-21', 2),
(18, '2022-01-22', 6),
(18, '2022-01-23', 4),
(18, '2022-01-24', 4),
(18, '2022-01-25', 8),
(18, '2022-01-26', 8),
(18, '2022-01-27', 8),
(18, '2022-01-28', 8),

(7, '2022-01-19', 2),
(7, '2022-01-20', 6),
(7, '2022-01-21', 6),
(7, '2022-01-22', 2),
(7, '2022-01-23', 4),
(7, '2022-01-24', 4),
(7, '2022-01-29', 8),
(7, '2022-01-30', 8),
(7, '2022-01-31', 8),
(7, '2022-02-01', 8),
(7, '2022-02-02', 8),

(8, '2022-02-03', 2),
(8, '2022-02-04', 6),
(8, '2022-02-05', 6),
(8, '2022-02-06', 2),
(8, '2022-02-07', 4),
(8, '2022-02-08', 4),
(8, '2022-02-09', 8),
(8, '2022-02-10', 8),

(9, '2022-02-03', 6),
(9, '2022-02-04', 2),
(9, '2022-02-05', 2),
(9, '2022-02-06', 6),
(9, '2022-02-07', 4),
(9, '2022-02-08', 4),
(9, '2022-02-11', 8),
(9, '2022-02-12', 8),
(9, '2022-02-13', 8),
(9, '2022-02-14', 2)
go

-- RBEAUMONT
print 'Table Travaux pour RBEAUMONT sur V1'
insert Travaux (IdTache, DateTravail, Heures) values
(10, '2022-01-07', 2),
(10, '2022-01-08', 6),
(10, '2022-01-09', 8),
(10, '2022-01-10', 2),
(10, '2022-01-11', 6),
(10, '2022-01-12', 4),
(10, '2022-01-13', 4),
(10, '2022-01-14', 8),

(11, '2022-01-07', 6),
(11, '2022-01-08', 2),
(11, '2022-01-10', 6),
(11, '2022-01-11', 2),
(11, '2022-01-12', 4),
(11, '2022-01-13', 4),
(11, '2022-01-15', 8),

(12, '2022-01-16', 8),
(12, '2022-01-17', 8),

(19, '2022-01-19', 6),
(19, '2022-01-20', 2),
(19, '2022-01-21', 2),
(19, '2022-01-22', 6),

(20, '2022-01-19', 2),
(20, '2022-01-20', 6),
(20, '2022-01-21', 6),
(20, '2022-01-22', 2),
(20, '2022-01-23', 4),
(20, '2022-01-24', 4),
(20, '2022-01-29', 8),
(20, '2022-01-30', 8),
(20, '2022-01-31', 8),

(21, '2022-01-19', 6),
(21, '2022-01-20', 2),
(21, '2022-01-21', 2),
(21, '2022-01-22', 6),
(21, '2022-01-23', 4),
(21, '2022-01-24', 4),
(21, '2022-02-01', 8),
(21, '2022-02-02', 8),
(21, '2022-02-03', 8),

(1, '2022-02-04', 6),
(1, '2022-02-05', 2),
(1, '2022-02-06', 2),
(1, '2022-02-07', 6),
(1, '2022-02-08', 4),
(1, '2022-02-09', 4),
(1, '2022-02-10', 8),
(1, '2022-02-11', 8),
(1, '2022-02-12', 8),

(2, '2022-02-04', 2),
(2, '2022-02-05', 6),
(2, '2022-02-06', 6),
(2, '2022-02-07', 2),
(2, '2022-02-08', 4),
(2, '2022-02-09', 4),
(2, '2022-02-13', 8),
(2, '2022-02-14', 8),
(2, '2022-02-15', 8),

(2, '2022-02-16', 2),
(2, '2022-02-17', 6),
(2, '2022-02-18', 6),
(2, '2022-02-19', 2),
(2, '2022-02-20', 4),
(2, '2022-02-21', 4),
(2, '2022-02-22', 8),
(2, '2022-02-23', 8),
(2, '2022-02-24', 8),

(3, '2022-02-16', 6),
(3, '2022-02-17', 2),
(3, '2022-02-18', 2),
(3, '2022-02-19', 6),
(3, '2022-02-20', 4),
(3, '2022-02-21', 4),
(3, '2022-02-25', 8),
(3, '2022-02-26', 8),
(3, '2022-02-27', 8),
(3, '2022-02-28', 8),
(3, '2022-03-01', 8),
(3, '2022-02-02', 8),
(3, '2022-02-03', 8)
go

-------------------------------------------------------------------
-- Mise à jour des temps restants estimés sur les tâches liées à des travaux

update t1
set DureeRestante = DureePrevue - (
	select SUM(w.Heures)
	from Travaux w
	inner join Taches t2 on w.IdTache = t2.Id
	where w.IdTache = t1.Id
	group by w.IdTache
)
from Taches t1
inner join Travaux w on t1.Id = w.IdTache

update Taches set DureeRestante = 0 where DureeRestante < 0
go
