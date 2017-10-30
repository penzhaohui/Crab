USE [master]
GO
DROP DATABASE [CrabIdentity]
GO
CREATE DATABASE [CrabIdentity]
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'CrabIdentity', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CrabIdentity].[dbo].[sp_fulltext_database] @action = 'disable'
end
GO
ALTER DATABASE [CrabIdentity] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CrabIdentity] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CrabIdentity] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CrabIdentity] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CrabIdentity] SET ARITHABORT OFF 
GO
ALTER DATABASE [CrabIdentity] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CrabIdentity] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CrabIdentity] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CrabIdentity] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CrabIdentity] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CrabIdentity] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CrabIdentity] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CrabIdentity] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CrabIdentity] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CrabIdentity] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CrabIdentity] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CrabIdentity] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CrabIdentity] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CrabIdentity] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CrabIdentity] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CrabIdentity] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CrabIdentity] SET  READ_WRITE 
GO
ALTER DATABASE [CrabIdentity] SET RECOVERY FULL 
GO
ALTER DATABASE [CrabIdentity] SET  MULTI_USER 
GO
ALTER DATABASE [CrabIdentity] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CrabIdentity] SET DB_CHAINING OFF 


USE [CrabIdentity]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Tenant]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Tenant](
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[DisplayName] [nvarchar](256) NOT NULL,
	[Approved] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[LicenseCount] [int] NOT NULL,
	[Contact] [nvarchar](50) NULL,
	[Phone] [nvarchar](50) NULL,
	[Fax] [nvarchar](50) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Email] [nvarchar](50) NULL,
	[Website] [nvarchar](50) NULL,
	[City] [nvarchar](50) NULL,
	[Address] [nvarchar](256) NULL,
	[ZipCode] [nvarchar](10) NULL,
 CONSTRAINT [PK_Tenant] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TenantUser]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TenantUser](
	[Id] [uniqueidentifier] NOT NULL,
	[TenantId] [uniqueidentifier] NOT NULL,
	[Upn] [nvarchar](256) NOT NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crab_CreateTenant]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[crab_CreateTenant]
    @Id                                     uniqueidentifier,
    @Name                                   nvarchar(50),
    @DisplayName                            nvarchar(256),
    @Approved                               bit,
    @CreateDate								datetime,
	@EndDate								datetime,
    @LicenseCount                           int,
    @Contact                                nvarchar(50),
    @Phone                                  nvarchar(50),
    @Fax                                    nvarchar(50),
    @Mobile                                 nvarchar(50),
    @Email                                  nvarchar(50),
    @Website                                nvarchar(50),
    @City                                   nvarchar(50),
    @Address                                nvarchar(256),
    @ZipCode                                nvarchar(10)
AS
BEGIN
    IF( @Id IS NULL )
        SELECT @Id = NEWID()

    --Name Confliction
    IF( EXISTS( SELECT id FROM dbo.Tenant
                    WHERE LOWER(@Name) = LOWER(Name)))
            RETURN 1

    INSERT INTO Tenant(
      Id,
      [Name],
      DisplayName,
      Approved,
      CreateDate,
	  EndDate,
      LicenseCount,
      Contact,
      Phone,
      Fax,
      Mobile,
      Email,
      Website,
      City,
      Address,
      ZipCode) 
      VALUES(
      @Id,
      @Name,
      @DisplayName,
      @Approved,
      @CreateDate,
	  @EndDate,
      @LicenseCount,
      @Contact,
      @Phone,
      @Fax,
      @Mobile,
      @Email,
      @Website,
      @City,
      @Address,
      @ZipCode
      )

    RETURN 0
END





' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_TenantUser_Tenant]') AND parent_object_id = OBJECT_ID(N'[dbo].[TenantUser]'))
ALTER TABLE [dbo].[TenantUser]  WITH CHECK ADD  CONSTRAINT [FK_TenantUser_Tenant] FOREIGN KEY([TenantId])
REFERENCES [dbo].[Tenant] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TenantUser] CHECK CONSTRAINT [FK_TenantUser_Tenant]
