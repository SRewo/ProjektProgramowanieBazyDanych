create database hotel; -- separately

use hotel;

create table pokoje(
	id_pokoju int Primary key identity,
	numer smallint not null,
	typ nvarchar(30) not null,
	cena_za_dobe money not null
)

create table goscie(
	id_goscia int Primary key identity,
	imie nvarchar(30) not null,
	nazwisko nvarchar(50) not null,
	numer_dowodu nvarchar(9) not null,
	ulica nvarchar(40),
	miasto nvarchar(20),
	telefon nvarchar(20) not null,
	email nvarchar(20) 
)

create table rezerwacje(
	id_rezerwacji int Primary Key identity,
	id_goscia int Foreign key references goscie,
	data_rezerwacji date not null,
	od date not null,
	do date null,
	cena_laczna money not null
)

create table zarezerwowane_pokoje(
	id_pokoju int Foreign key references pokoje,
	id_rezerwacji int Foreign key references rezerwacje,
)

create table uzytkownicy(
	id_uzytkownika int primary key identity,
	login nvarchar(30) not null,
	password varchar(64) not null,
	typ_uzytkownika nvarchar(30) not null
)


INSERT into pokoje values (101,'single - SGL',120)
INSERT into pokoje values (102,'single - SGL',120)
INSERT into pokoje values (103,'double - DBL',220)
INSERT into pokoje values (201,'double - DBL',220)
INSERT into pokoje values (202,'twin - TWN',320)
INSERT into pokoje values (203,'twin - TWN',320)
INSERT into pokoje values (204,'single - SGL',120)
INSERT into pokoje values (302,'kings - KNG',500)


Insert into goscie values ('Tomek','Kasztan','AXM876987','Powsta�c�w 12','Katowice','732 187 988','')
Insert into goscie values ('Adam','Boczek','AXM876234','Gra�y�skiego 12','Katowice','732 187 444','')

Insert into uzytkownicy values ('admin', '8C6976E5B5410415BDE908BD4DEE15DFB167A9C873FC4BB8A81F6F2AB448A918', 'Szef')
