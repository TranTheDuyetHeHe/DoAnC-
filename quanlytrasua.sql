create database quanlytrasua
go

use quanlytrasua

create table tablefood
(
id int identity primary key,
name nvarchar(100) not null default N'Bàn chưa đặt tên',
status nvarchar(100) not null default N'Trống'
)
go

create table account
(
username nvarchar(100) primary key,
displayname nvarchar(100) not null default N'CTER',
password nvarchar(100) not null default 0,
type int not null default 0,  --1 : admin, 0 : staff 
)
go

create table foodcategory
(
id int identity primary key,
name nvarchar(100) not null default N'Chưa đặt tên'
)
go

create table food
(
id int identity primary key,
name nvarchar(100) not null default N'Chưa đặt tên' ,
idcategory int not null,
price float not null default 0,
foreign key (idcategory) references foodcategory(id)
)
go

create table bill
(
id int identity primary key,
datecheckin date not null default getdate(),
datecheckout date,
idtable int not null,
status int not null default 0 --1 : đã thanh toán, 0 : chưa thanh toán 
foreign key (idtable) references tablefood(id)  
)
go

create table billinfo
(
id int identity primary key,
idbill int not null,
idfood int not null,
count int not null default 0,
foreign key (idbill) references bill(id),  
foreign key (idfood) references food(id)  
)
go

insert into account
(
username,
displayname,
password,
type
)

values
(
'admin', --usename(nvarchar(100))
'duyet', --displayname(nvarchar(100))
'1', --password(nvarchar(100))
1 
)

insert into account
(
username,
displayname,
password,
type
)

values
(
'staff', --usename(nvarchar(100))
'quy', --displayname(nvarchar(100))
'1', --password(nvarchar(100))
0 
)

select * from account

CREATE PROC USB_GetAccountByUserName
@username nvarchar(100)
as 
begin
 select * from dbo.account where username = @username
 end

 EXEC dbo.USB_GetAccountByUserName @username = N'admin'

 create proc USB_Login
 @username nvarchar(100), @password nvarchar(100)
 AS
 BEGIN
 select * from dbo.account where username = @username and password = @password
 END

 DECLARE @i INT = 0
 WHILE @i <= 10 

 BEGIN
  insert tablefood (name) values (N'Bàn' + CAST(@i as nvarchar(100)))
	SET @i =  @i + 1
 END

 select * from tablefood

 create Proc USB_GetTableList
 AS SELECT * from tablefood

 exec dbo.USB_GetTableList

 select * from dbo.bill

 select * from dbo.billinfo

 select * from food

 select * from foodcategory

--thêm foodcategory
insert foodcategory
(name)
values(N'Bột sữa')

insert foodcategory
(name)
values(N'Trân châu')

insert foodcategory
(name)
values(N'Đường')

insert foodcategory
(name)
values(N'Kem')

insert foodcategory
(name)
values(N'Trà')

--thêm món ăn
insert food
(name, idcategory, price )
values(N'Trà sữa','1','10000')

insert food
(name, idcategory, price )
values(N'Trân châu','2','10000')

insert food
(name, idcategory, price )
values(N'Đường đen','3','10000')

insert food
(name, idcategory, price )
values(N'Kem tươi','4','10000')

insert food
(name, idcategory, price )
values(N'Trà nhài','5','10000')

--thêm bill
insert bill
(datecheckin,datecheckout,idtable,status)
values
(GETDATE (),
NULL,
1,
0)

insert bill
(datecheckin,datecheckout,idtable,status)
values
(GETDATE (),
NULL,
2,
0)

insert bill
(datecheckin,datecheckout,idtable,status)
values
(GETDATE (),
NULL,
3,
0)

insert bill
(datecheckin,datecheckout,idtable,status)
values
(GETDATE (),
GETDATE(),
4,
1)

--Thêm bill info
insert billinfo
(idbill,idfood,count)
values('1','1',1)

insert billinfo
(idbill,idfood,count)
values('2','1',3)

insert billinfo
(idbill,idfood,count)
values('3','3',1)

insert billinfo
(idbill,idfood,count)
values('4','2',1)

select * from tablefood

select * from bill where idtable = 1 and status = 0

select * from billinfo where idbill = 1

select f.name, bi.count, f.price, f.price*bi.count as totalprice from billinfo as bi , bill as b , food as f where bi.idbill = b.id and bi.idfood = f.id and b.status = 0 and b.idtable = 1

alter proc USB_insertBill
@idtable Int 
AS
BEGIN
insert bill
(datecheckin,datecheckout,idtable,status,discount)
values
(GETDATE (),
NULL,
@idtable,
0,
0)
END

alter proc USB_insertBillinfo
@idbill int , @idfood int , @count int
AS
BEGIN
	
	declare  @isExitsBillInfo int 
	declare @foodcount int = 1

	select @isExitsBillInfo= id, @foodcount = b.count from billinfo as b where idbill = @idbill and idfood = @idfood

	if(@isExitsBillInfo > 0)
	Begin
	declare @newCount int = @foodcount + @count
	if(@newCount >0)
		update billinfo set count = @foodcount + @count where idfood = @idfood
	else	
		delete billinfo where idbill = @idbill and idfood = @idfood
	END	
	else
	begin
	insert billinfo
	(idbill,idfood,count)
	values(@idbill,@idfood,@count)
	end
END

select max(id) from bill

update bill set status =1 where id =1

alter trigger UTG_UpdateBillinfo
on billinfo for insert, update
as
begin
	declare @idBill int

	select  @idBill = idbill from inserted

	declare @idTable int 

	select @idTable = idtable from bill where id = @idBill and status = 0

	declare @count int

	select @count = COUNT(*) from billinfo where idbill = @idBill
	
	if(@count > 0)
		update tablefood set status = N'Có người' where id = @idTable
	else
		update tablefood set status = N'Trống' where id = @idTable
end

drop trigger UTG_Updatetable
on tablefood for update
as
begin
	declare @idtable int
	declare @status nvarchar(100)
	select @idtable = id, @status = inserted.status from inserted

	declare @idbill int
	select @idbill = id from bill where idtable = @idtable and status = 0

	declare @countbillinfo int
	select @countbillinfo = COUNT(*) from billinfo where idbill = @idbill

	if(@countbillinfo > 0 and @status <> N'Có người')
		update tablefood set status = N'Có người' where id = @idtable
	else if (@countbillinfo < 0 and @status <> N'Trống')
		update tablefood set status = N'Trống' where id = @idtable
end

alter trigger UTG_UpdateBill
on bill for update
as
begin
	declare @idBill int 

	select @idBill = id from inserted

	declare @idTable int 

	select @idTable = idtable from bill where id = @idBill 

	declare @count int = 0

	select @count = COUNT(*) from bill where idtable = @idTable and status = 0

	if(@count = 0)
		update tablefood set status =N'Trống' where id = @idTable
end

delete dbo.bill

delete dbo.billinfo

alter table bill
add discount int

update bill set discount = 0

alter proc USB_Switchtable
@idtable1 int , @idtable2 int  
as begin
	declare @idFirstbill int
	declare @idsecondbill int
	
	declare @isfirsttableEmty int = 1
	declare @isSecondtableEmty int = 1

	select  @idsecondbill = id from bill where idtable = @idtable2 and status = 0
	select  @idFirstbill = id from bill where idtable = @idtable1 and status = 0
	
	if(@idFirstbill is null)
	begin
		insert bill
			(datecheckin,datecheckout,idtable,status)
			values
		(GETDATE (),
		NULL,
		@idtable1,
		0)
		select @idFirstbill = max(id) from bill  where idtable = @idtable1 and status = 0
	end

	select @isfirsttableEmty = COUNT(*) from billinfo where idbill = @idFirstbill

	if(@idsecondbill is null)
	begin
		insert bill
			(datecheckin,datecheckout,idtable,status)
			values
		(GETDATE (),
		NULL,
		@idtable2,
		0)
		select @idsecondbill = max(id) from bill  where idtable = @idtable2 and status = 0
	end

		select @isSecondtableEmty = COUNT(*) from billinfo where idbill = @idsecondbill

	select id into idbillinfotable from billinfo where idbill = @idsecondbill

	update billinfo set idbill = @idsecondbill where idbill = @idFirstbill

	update billinfo set idbill = @idFirstbill where id in (select * from idbillinfotable)

	drop table idbillinfotable

	if(@isfirsttableEmty = 0)
		update tablefood set status = N'Trống' where id = @idtable2
	if(@isSecondtableEmty = 0)
		update tablefood set status = N'Trống' where id = @idtable1
end

update tablefood set status =N'Trống'

alter table dbo.bill 
alter column totalprice float

delete bill
delete billinfo

alter proc USB_GetListBilByDate
@checkin date , @checkout date
as
begin
	select t.name AS [Tên bàn] , b.totalprice as [Tổng tiền] , datecheckin as [Ngày vào] , datecheckout as [Ngày ra] , discount as [Giảm giá] 
	from bill as b, tablefood as t 
	where datecheckin >= @checkin and datecheckout <= @checkout and b.status = 1 
	and t.id = b.idtable 
end

select * from bill

exec USB

select * from account

create proc USB_UpdateAccount
@username nvarchar(100) , @displayname nvarchar(100), @password nvarchar(100) , @newpassword nvarchar(100)
as
begin
	declare @isrightpass int

	select @isrightpass = COUNT(*) from account where username = @username and password = @password

	if(@isrightpass = 1)
	begin
		if( @newpassword = null or @newpassword ='')
		begin
			update account set displayname = @displayname where username = @username
		end
		else 
			update account set displayname = @displayname , password = @newpassword where username = @username
	end
end

insert food (name, idcategory, price) values (N'', 0, 0.0)

update food set name =	N''	, idcategory = 5 , price = 0 where id = 5


select * from bill

alter Trigger UTG_DeleteBillInfo
on billinfo for delete 
as
begin
	declare @idBillInfo int
	declare @idBill int
	select @idBillInfo = id , @idBill = deleted.idbill from deleted

	declare @idTable int 
	select @idTable = idtable from bill where id = @idBill

	declare @count int = 0
	select @count = COUNT(*) from billinfo as bi , bill as b where b.id = bi.idbill and b.id = @idBill and b.status = 0

	if(@count = 0)
		update tablefood set status =N'Trống' where id = @idTable
	
end

select * from foodcategory

select * from food

insert foodcategory (name) values (N'Sandwich')

select * from food

update foodcategory set name = N'Trà sữa đỉnh cao' where id = 1

select * from tablefood
update tablefood set name = 'Bàn 1'  where id = 1

select username, displayname , type from account

CREATE FUNCTION [dbo].[fuConvertToUnsign1] ( @strInput NVARCHAR(4000) ) RETURNS NVARCHAR(4000) AS BEGIN IF @strInput IS NULL RETURN @strInput IF @strInput = '' RETURN @strInput DECLARE @RT NVARCHAR(4000) DECLARE @SIGN_CHARS NCHAR(136) DECLARE @UNSIGN_CHARS NCHAR (136) SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệế ìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý ĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍ ÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' +NCHAR(272)+ NCHAR(208) SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeee iiiiiooooooooooooooouuuuuuuuuuyyyyy AADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIII OOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD' DECLARE @COUNTER int DECLARE @COUNTER1 int SET @COUNTER = 1 WHILE (@COUNTER <=LEN(@strInput)) BEGIN SET @COUNTER1 = 1 WHILE (@COUNTER1 <=LEN(@SIGN_CHARS)+1) BEGIN IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@strInput,@COUNTER ,1) ) BEGIN IF @COUNTER=1 SET @strInput = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)-1) ELSE SET @strInput = SUBSTRING(@strInput, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@strInput, @COUNTER+1,LEN(@strInput)- @COUNTER) BREAK END SET @COUNTER1 = @COUNTER1 +1 END SET @COUNTER = @COUNTER +1 END SET @strInput = replace(@strInput,' ','-') RETURN @strInput END


select * from food where dbo.fuConvertToUnsign1 (name) Like N'%' + dbo.fuConvertToUnsign1(N'ca') + '%' 

select * from account

select * from bill

update account set password ='1' where username =	N'hahaha'

select * from account