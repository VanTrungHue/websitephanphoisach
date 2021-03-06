﻿create database ctyppsach
go

create table nxb
(
	idnxb int primary key identity(1,1),
	tennxb nvarchar(50),
	diachi nvarchar(50),
	sodt nvarchar(15),
	sotk nvarchar(50)
)
go

create table daily
(
	iddl int primary key identity(1,1),
	tendl nvarchar(50),
	diachi nvarchar(50),
	sodt nvarchar(15),
)
go

create table linhvuc
(
	idlv int primary key identity(1,1),
	tenlv nvarchar(50),
)
go

create table sach
(
	idsach int primary key identity(1,1),
	tensach nvarchar(50),
	tacgia nvarchar(50),
	idnxb int,
	idlv int,
	giaxuat decimal(10,0),
	gianhap decimal(10,0),
	
	foreign key(idnxb) references nxb(idnxb),
	foreign key(idlv) references linhvuc(idlv),
)
go

create table phieunhap
(
	idpn int primary key identity(1,1),
	idnxb int,
	nguoigiaosach nvarchar(50),
	ngaynhap datetime,
	nguoivietphieu nvarchar(50),
	
	foreign key(idnxb) references nxb(idnxb),
)
go

create table ctpn
(
	idpn int,
	idctpn int,
	idsach int,
	soluong int,
	
	foreign key(idpn) references phieunhap(idpn),
	foreign key(idsach) references sach(idsach),
	primary key(idpn, idctpn)
)
go

create table phieuxuat
(
	idpx int primary key identity(1,1),
	iddl int,
	nguoinhan nvarchar(50),
	ngayxuat datetime,
	
	foreign key(iddl) references daily(iddl),
)
go

create table ctpx
(
	idpx int,
	idctpx int,
	idsach int,
	soluong int,
	
	foreign key(idpx) references phieuxuat(idpx),
	foreign key(idsach) references sach(idsach),
	primary key(idpx, idctpx)
)
go

create table tonkho
(
	idtk int primary key identity(1,1),
	idsach int not null,
	thoidiem datetime not null CONSTRAINT DF_tonkho_thoidiem_GETDATE DEFAULT GETDATE(),
	soluongton int,

	foreign key(idsach) references sach(idsach),
)
go

create table hangtoncuadaily
(
	idht int primary key identity(1,1),
	iddl int,
	idsach int,
	soluongchuaban int,

	foreign key(iddl) references daily(iddl),
	foreign key(idsach) references sach(idsach)
)
go

create table congnotheothoigian
(
	id int primary key identity(1,1),
	iddl int,
	congno decimal(10,1),
	thoidiem datetime not null CONSTRAINT DF_congnothethoigian_thoidiem_GETDATE DEFAULT GETDATE(),

	foreign key(iddl) references daily(iddl)
)
go

create table danhmucsachdaban
(
	iddmsdb int primary key identity(1,1),
	iddl int,
	thoigian datetime,
	sotiendathanhtoan decimal(10,1),
	sotienconno decimal(10,1),

	foreign key(iddl) references daily(iddl)
)
go

create table ctdmsdb
(
	idctdmsdb int,
	iddmsdb int,
	idsach int,
	soluong int,

	foreign key(iddmsdb) references danhmucsachdaban(iddmsdb),
	foreign key(idsach) references sach(idsach),
	primary key(iddmsdb, idctdmsdb)
)
go

create table sotienphaitrachonxb
(
	id int primary key identity(1,1),
	idnxb int,
	sotienphaitra decimal(10,1),

	foreign key(idnxb) references nxb(idnxb)
)
go

create table phieutratien
(
	idptt int primary key identity(1,1),
	idnxb int,
	sotientra decimal(10,1),
	tinhtrang nvarchar(50),
	
	foreign key(idnxb) references nxb(idnxb)
)

alter table sotienphaitrachonxb
add thoidiem datetime;


alter table phieutratien
add ngaytaophieu datetime;

--version 2.3
alter table danhmucsachdaban
add tinhtrang varchar(50);

insert into linhvuc values(N'văn học')
insert into linhvuc values(N'toán học')
insert into linhvuc values(N'tiểu thuyết')
insert into linhvuc values(N'Sách giáo khoa')

insert into daily values(N'Nhà sách Phú Thọ', N'12/3 Hồ Chí Minh','01010123232')
insert into daily values(N'Đại lý sách A', N'98/6 Hà Nội', '09071143133')
insert into daily values(N'Đại lý sách B', N'28/4 Đà Nẵng', '090713434633')
insert into daily values(N'Đại lý sách C', N'96/8 Hải Phòng', '09074546533')

insert into nxb values(N'Nhà xuất bản A', N'56/32 Vũ Tàu', '0906123455','213124344253456')
insert into nxb values(N'Nhà xuất bản B', N'78/42 Bình Phước', '0934523455','21312437878743456')
insert into nxb values(N'Nhà xuất bản C', N'96/52 Hà Nội', '09064564455','2131243457583456')
insert into nxb values(N'Nhà xuất bản D', N'26/12 Hồ Chí Minh', '090612522345','2131243445463456')

insert into sach values(N'sách A', N'Tác giả A', 1, 1, 40000, 30000)
insert into sach values(N'sách B', N'Tác giả B', 2, 2, 50000, 40000)
insert into sach values(N'sách C', N'Tác giả C', 3, 3, 60000, 50000)
insert into sach values(N'sách D', N'Tác giả D', 1, 1, 80000, 60000)
insert into sach values(N'sách E', N'Tác giả E', 4, 4, 90000, 70000)


insert into congnotheothoigian values (1,0,29/10/2017)
insert into congnotheothoigian values (2,0,29/10/2017)
insert into congnotheothoigian values (3,0,29/10/2017)
insert into congnotheothoigian values (4,0,29/10/2017)

insert into sotienphaitrachonxb values (1,0,29/10/2017)
insert into sotienphaitrachonxb values (2,0,29/10/2017)
insert into sotienphaitrachonxb values (3,0,29/10/2017)
insert into sotienphaitrachonxb values (4,0,29/10/2017)

insert into tonkho values (1,0,29/10/2017)
insert into tonkho values (2,0,29/10/2017)
insert into tonkho values (3,0,29/10/2017)
insert into tonkho values (4,0,29/10/2017)
insert into tonkho values (5,0,29/10/2017)


