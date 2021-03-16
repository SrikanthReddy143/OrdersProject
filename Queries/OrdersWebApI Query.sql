

Create Table Customers(Id int identity(1,1) primary key,CustomerName Varchar(200),[Password] varchar(50), Emaild varchar(200),
[Address] Varchar(max),MobileNo varchar(15),CreatedDate datetime)
Go
Create Table CustomerOrders(Id int identity(1000,1) primary key,DeliveryAddress varchar(1000),
fk_CustomerId int foreign key REFERENCES Customers(Id),CreatedDate datetime)
GO
Create Table Items(Id int identity(1,1) primary key,ItemName varchar(1000),Price int,fk_OrderId int foreign key 
REFERENCES CustomerOrders(Id),CreatedDate datetime) 
GO
---------------------------------------
-- user defined table data type
CREATE TYPE OrderedItems AS TABLE 
(
ItemName varchar(1000),
Price int
)
GO
------------------------------------------------------
CREATE PROCEDURE [dbo].[usp_CreateCustomer]
@Name varchar(200),
@Email varchar(200),
@Password varchar(200),
@Address varchar(max)=NULL,
@Mobile varchar(15)=NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		IF NOT EXISTS (SELECT * FROM Customers WHERE Emaild =@Email) 
		BEGIN
			INSERT INTO Customers (CustomerName,Emaild,[Password],[Address],MobileNo,CreatedDate)
			values(@Name,@Email,@Password,@Address,@Mobile,GETDATE())
			select 1;
		END
		ELSE 
		BEGIN 
			 select 0;
		END
	END TRY
	BEGIN CATCH
		;Throw 50001,'Error while creatig customer',1;
	END CATCH
 
END
--------------------------------------------------------------
GO
------------------------------------
CREATE PROCEDURE [dbo].[usp_CreateOrders]
@DeliveryAddress varchar(max),
@UserId int,
@OrderedItem_Details [OrderedItems] readonly

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	BEGIN TRY
		
			INSERT INTO CustomerOrders(DeliveryAddress,fk_CustomerId,CreatedDate)values(@DeliveryAddress,@UserId,GETDATE())

			INSERT INTO Items(ItemName,Price,fk_OrderId,CreatedDate)
			SELECT ItemName, Price, @@IDENTITY,GETDATE() FROM @OrderedItem_Details
		
			select 1
	END TRY
	BEGIN CATCH
		;Throw 50001,'Error while Order the items',1;
	END CATCH
 
END
GO

------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[usp_GetOrders]
@OrderId int
AS
BEGIN
	select crd.Id as OrderedId,it.ItemName,it.Price,crd.DeliveryAddress,crd.CreatedDate,crd.fk_CustomerId from Items it 
	left join CustomerOrders crd on crd.Id=it.fk_OrderId where it.fk_OrderId=@OrderId
END




