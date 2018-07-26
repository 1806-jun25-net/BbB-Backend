Create Table Usr (
Id Int Identity Not Null,
UserName NVarChar(16) Unique Not Null,
EmailAddress NVarChar(30) Unique Not Null,
Pass NVarChar(32) Not Null,
Company NVarChar(20),
Credit Decimal Not Null,
Rating Decimal Not Null,
Constraint PK_Usr Primary Key (Id));

Create Table Destination (
Id Int Identity Not Null,
Title NVarChar(16),
StreetAddress NVarChar(30),
Constraint PK_Destination Primary Key (Id));

Create Table Driver (
Id Int Identity Not Null,
UserId Int Not Null,
Seats Int,
MeetLoc NVarChar(16),
Rating Decimal,
Constraint PK_Driver Primary Key (Id),
Constraint FK_Driver Foreign Key (UserId) References Usr (Id));

Create Table Msg (
Id Int Identity Not Null,
SenderId Int Not Null,
ReceiverId Int Not Null,
DTime DateTime,
Msg NVarChar(255),
Constraint PK_Msg Primary Key (Id),
Constraint FK1_Msg Foreign Key (SenderId) References Usr (Id),
Constraint FK2_Msg Foreign Key (ReceiverId) References Usr (Id));

Create Table Drive (
Id Int Identity Not Null,
DriverId Int Not Null,
DestinationId Int Not Null,
DType NVarChar(16),
DTime DateTime,
Constraint PK_Drive Primary Key (Id),
Constraint FK1_Drive Foreign Key (DriverId) References Driver (Id),
Constraint FK2_Drive Foreign Key (DestinationId) References Destination (Id));

Create Table MenuItem (
Id Int Identity Not Null,
DestinationId Int Not Null,
ItemName NVarChar(32),
Cost Decimal,
Constraint PK_MenuItem Primary Key (Id),
Constraint FK_DestinationId Foreign Key (DestinationId) References Destination (Id));

Create Table UserJoin (
DriveId Int Not Null,
UserId Int Not Null,
Constraint PK_UserJoin Primary Key (DriveId, UserId),
Constraint FK1_UserJoin Foreign Key (DriveId) References Drive (Id),
Constraint FK2_UserJoin Foreign Key (UserId) References Usr (Id));

Create Table ArchiveDrive (
Id Int Identity Not Null,
DriverId Int Not Null,
DestinationId Int Not Null,
DType NVarChar(16),
DTime DateTime,
Constraint PK_ArchiveDrive Primary Key (Id),
Constraint FK1_DriverId Foreign Key (DriverId) References Driver (Id),
Constraint FK2_DestinationId Foreign Key (DestinationId) References Destination (Id));

Create Table UserReview (
Id Int Identity Not Null,
DriverId Int Not Null,
UserId Int Not Null,
Rating Int Not Null,
ArchiveDriveId Int Not Null,
Constraint PK_UserReview Primary Key (Id),
Constraint FK1_UserReview Foreign Key (DriverId) References Driver (Id),
Constraint FK2_UserReview Foreign Key (UserId) References Usr (Id),
Constraint FK3_UserReview Foreign Key (ArchiveDriveId) References ArchiveDrive (Id));

Create Table DriverReview (
Id Int Identity Not Null,
DriverId Int Not Null,
UserId Int Not Null,
Rating Int Not Null,
ArchiveDriveId Int Not Null,
Constraint PK_JoinerReview Primary Key (Id),
Constraint FK1_JoinerReview Foreign Key (DriverId) References Driver (Id),
Constraint FK2_JoinerReview Foreign Key (UserId) References Usr (Id),
Constraint FK3_JoinerReview Foreign Key (ArchiveDriveId) References ArchiveDrive (Id));

Create Table UserPickup (
Id Int Identity Not Null,
UserId Int Not Null,
DriveId Int Not Null,
Constraint PK_UserPickup Primary Key (Id),
Constraint FK1_UserPickup Foreign Key (UserId) References Usr (Id),
Constraint FK2_UserPickup Foreign Key (DriveId) References Drive (Id));

Create Table OrderItem (
OrderId Int Not Null,
ItemId Int Not Null,
Quantity Int Not Null,
Msg NVarChar(255),
Constraint PK_OrderItem Primary Key (OrderId, ItemId),
Constraint FK1_OrderItem Foreign Key (OrderId) References UserPickup (Id),
Constraint FK2_OrderItem Foreign Key (ItemId) References MenuItem (Id));

Create Table ArchiveOrder (
Id Int Identity Not Null,
UserId Int Not Null,
ArchiveDriveId Int Not Null,
Constraint PK_ArchiveOrder Primary Key (Id),
Constraint FK1_ArchiveOrder Foreign Key (UserId) References Usr (Id),
Constraint FK2_ArchiveOrder Foreign Key (ArchiveDriveId) References ArchiveDrive (Id));

Create Table ArchiveUserJoin (
ArchiveDriveId Int Not Null,
UserId Int Not Null,
Constraint PK_ArchiveUserJoin Primary Key (ArchiveDriveId, UserId),
Constraint FK1_ArchiveUserJoin Foreign Key (ArchiveDriveId) References ArchiveDrive (Id),
Constraint FK2_ArchiveUserJoin Foreign Key (UserId) References Usr (Id));

Create Table ArchiveItem (
Id Int Identity Not Null,
ArchiveOrderId Int Not Null,
ItemName NVarChar(32),
Quantity Int,
Cost Decimal,
Msg NVarChar(255),
Constraint PK_ArchiveItem Primary Key (Id),
Constraint FK_ArchiveItem Foreign Key (ArchiveOrderId) References ArchiveOrder (Id));