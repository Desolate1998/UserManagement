CREATE TABLE Groups (
	GroupCode VARCHAR(10) NOT NULL,
	Name NVARCHAR(50) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	CONSTRAINT pk_Group_Code PRIMARY KEY (GroupCode)
);
		
CREATE TABLE Users (
	EntryId INT IDENTITY NOT NULL,
	FirstName NVARCHAR(100) NOT NULL,
	LastName NVARCHAR(100) NOT NULL,
	Email NVARCHAR(255) NOT NULL,
	PasswordHash NVARCHAR(255) NOT NULL,
	PasswordSalt NVARCHAR(255) NOT NULL,
	CONSTRAINT pk_User_Entry_Id PRIMARY KEY (EntryId),
	CONSTRAINT idx_Users_Email UNIQUE (Email)
);
		
CREATE TABLE Permissions (
	PermissionCode VARCHAR(10) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	CONSTRAINT PK_Permissions PRIMARY KEY (PermissionCode)
);
		
CREATE TABLE GroupPermission (
	GroupCode VARCHAR(10) NOT NULL,
	PermissionCode VARCHAR(10) NOT NULL,
	CONSTRAINT PK_GroupPermission PRIMARY KEY (GroupCode, PermissionCode),
	CONSTRAINT FK_GroupPermission_Groups_GroupsCode FOREIGN KEY (GroupCode) REFERENCES Groups (GroupCode),
	CONSTRAINT FK_GroupPermission_Permissions_PermissionsCode FOREIGN KEY (PermissionCode) REFERENCES Permissions (PermissionCode)
);
		
CREATE TABLE UserGroups (
	GroupCode VARCHAR(10) NOT NULL,
	UserId INT NOT NULL,
	CONSTRAINT PK_UserGroups PRIMARY KEY (GroupCode, UserId),
	CONSTRAINT FK_GroupUser_Groups_GroupsCode FOREIGN KEY (GroupCode) REFERENCES Groups (GroupCode),
	CONSTRAINT FK_GroupUser_Users_UsersEntryId FOREIGN KEY (UserId) REFERENCES Users (EntryId)
);
		
CREATE TABLE LoginStatusLookup (
	StatusCode VARCHAR(3) NOT NULL,
	Description NVARCHAR(255) NOT NULL,
	CONSTRAINT StatusCode PRIMARY KEY (StatusCode)
);
		
CREATE TABLE UserLoginHistory (
	EntryId INT IDENTITY NOT NULL,
	UserId INT,
	Ip NVARCHAR(MAX) NOT NULL,
	Date DATETIME2 NOT NULL,
	StatusCode VARCHAR(3) NOT NULL,
	CONSTRAINT PK_UserLoginHistory PRIMARY KEY (EntryId),
	CONSTRAINT FK_UserLogin_Users_UsersEntryId FOREIGN KEY (UserId) REFERENCES Users (EntryId),
	CONSTRAINT FK_UserLoginHistory_Status_StatusLookup FOREIGN KEY (StatusCode) REFERENCES LoginStatusLookup (StatusCode)
);
