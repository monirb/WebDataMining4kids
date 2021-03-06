USE [master]
GO

/****** Object:  Database [eLearning]    Script Date: 6/1/2014 12:25:06 PM ******/
CREATE DATABASE [eLearning]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'eLearning', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\eLearning.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'eLearning_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\eLearning_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [eLearning] SET COMPATIBILITY_LEVEL = 110
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [eLearning].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [eLearning] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [eLearning] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [eLearning] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [eLearning] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [eLearning] SET ARITHABORT OFF 
GO

ALTER DATABASE [eLearning] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [eLearning] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [eLearning] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [eLearning] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [eLearning] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [eLearning] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [eLearning] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [eLearning] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [eLearning] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [eLearning] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [eLearning] SET  DISABLE_BROKER 
GO

ALTER DATABASE [eLearning] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [eLearning] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [eLearning] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [eLearning] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [eLearning] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [eLearning] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [eLearning] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [eLearning] SET RECOVERY FULL 
GO

ALTER DATABASE [eLearning] SET  MULTI_USER 
GO

ALTER DATABASE [eLearning] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [eLearning] SET DB_CHAINING OFF 
GO

ALTER DATABASE [eLearning] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO

ALTER DATABASE [eLearning] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO

ALTER DATABASE [eLearning] SET  READ_WRITE 
GO


