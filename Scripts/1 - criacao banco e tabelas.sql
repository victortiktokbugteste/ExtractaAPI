use claimdb


create table ApplicationMiddlewareLogError (

Id int primary key identity(1,1),
CreateDate DATETIME,
Method nvarchar(max),
Exception nvarchar(max),
Trace nvarchar(max),
StatusCode INT

);


create table Vehicle (

Id int primary key identity(1,1),
Value decimal(14,2),
Branch nvarchar(255),
Model nvarchar(255)

);


create table Person (

Id int primary key identity(1,1),
Name nvarchar(max),
Cpf nvarchar(max),
DateOfBirth DATETIME

);


create table Claim (

Id int primary key identity(1,1),
CreateDate datetime,
UpdateDate datetime,
InsuredId int,
VehicleId int,
ClaimValue decimal(14,2),

FOREIGN KEY (InsuredId) REFERENCES Person(Id),
FOREIGN KEY (VehicleId) REFERENCES Vehicle(Id)
);

