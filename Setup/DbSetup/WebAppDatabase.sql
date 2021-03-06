USE [master]
GO
DROP DATABASE [CrabWebApp]
GO
CREATE DATABASE [CrabWebApp]
GO
EXEC dbo.sp_dbcmptlevel @dbname=N'CrabWebApp', @new_cmptlevel=90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CrabWebApp].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CrabWebApp] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CrabWebApp] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CrabWebApp] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CrabWebApp] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CrabWebApp] SET ARITHABORT OFF 
GO
ALTER DATABASE [CrabWebApp] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CrabWebApp] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CrabWebApp] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CrabWebApp] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CrabWebApp] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CrabWebApp] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CrabWebApp] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CrabWebApp] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CrabWebApp] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CrabWebApp] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CrabWebApp] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CrabWebApp] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CrabWebApp] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CrabWebApp] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CrabWebApp] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CrabWebApp] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CrabWebApp] SET  READ_WRITE 
GO
ALTER DATABASE [CrabWebApp] SET RECOVERY FULL 
GO
ALTER DATABASE [CrabWebApp] SET  MULTI_USER 
GO
ALTER DATABASE [CrabWebApp] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CrabWebApp] SET DB_CHAINING OFF 