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


INSERT into pokoje values (1,'single - SGL',120)
INSERT into pokoje values (2,'single - SGL',120)
INSERT into pokoje values (3,'double - DBL',220)
INSERT into pokoje values (4,'double - DBL',220)
INSERT into pokoje values (5,'twin - TWN',320)
INSERT into pokoje values (6,'twin - TWN',320)
INSERT into pokoje values (7,'single - SGL',120)
INSERT into pokoje values (8,'kings - KNG',500)


Insert into goscie values ('Tomek','Kasztan','AXM876987','Powstañców 12','Katowice','732 187 988','')
Insert into goscie values ('Adam','Boczek','AXM876234','Gra¿yñskiego 12','Katowice','732 187 444','')