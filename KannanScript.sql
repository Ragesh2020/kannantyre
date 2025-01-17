USE [master]
GO
/****** Object:  Database [TestKannan]    Script Date: 01-May-20 12:03:44 PM ******/
CREATE DATABASE [TestKannan] ON  PRIMARY 
( NAME = N'TestKannan', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSSQLSERVER\MSSQL\DATA\TestKannan.mdf' , SIZE = 2304KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TestKannan_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL10_50.MSSSQLSERVER\MSSQL\DATA\TestKannan_log.LDF' , SIZE = 1344KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TestKannan] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TestKannan].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TestKannan] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TestKannan] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TestKannan] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TestKannan] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TestKannan] SET ARITHABORT OFF 
GO
ALTER DATABASE [TestKannan] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TestKannan] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [TestKannan] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TestKannan] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TestKannan] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TestKannan] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TestKannan] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TestKannan] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TestKannan] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TestKannan] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TestKannan] SET  ENABLE_BROKER 
GO
ALTER DATABASE [TestKannan] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TestKannan] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TestKannan] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TestKannan] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TestKannan] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TestKannan] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TestKannan] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TestKannan] SET RECOVERY FULL 
GO
ALTER DATABASE [TestKannan] SET  MULTI_USER 
GO
ALTER DATABASE [TestKannan] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TestKannan] SET DB_CHAINING OFF 
GO
EXEC sys.sp_db_vardecimal_storage_format N'TestKannan', N'ON'
GO
USE [TestKannan]
GO
/****** Object:  Table [dbo].[Balance calculation data table]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Balance calculation data table](
	[Token Number] [nvarchar](250) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Billing Token Number] [nvarchar](250) NULL,
	[Billing Number] [nvarchar](max) NULL,
	[Bill Date] [datetime] NOT NULL,
	[Billing Total amount] [decimal](18, 2) NOT NULL,
	[Billing Amount paid] [decimal](18, 2) NOT NULL,
	[Billing Balance] [decimal](18, 2) NOT NULL,
	[Order Token Number] [nvarchar](250) NULL,
	[Order Number] [nvarchar](max) NULL,
	[Order Date] [datetime] NOT NULL,
	[Order Total amount] [decimal](18, 2) NOT NULL,
	[Order Amount paid] [decimal](18, 2) NOT NULL,
	[Order Balance] [decimal](18, 2) NOT NULL,
	[Other expense Token number] [nvarchar](250) NULL,
	[Other expense amount] [decimal](18, 2) NOT NULL,
	[Other expense Date] [datetime] NOT NULL,
	[Other expense ExpenseId] [nvarchar](250) NULL,
	[Product expense Token number] [nvarchar](250) NULL,
	[Product expense Amount for expense] [decimal](18, 2) NOT NULL,
	[Product expense Date] [datetime] NOT NULL,
	[Product expense ExpenseId] [nvarchar](250) NULL,
	[Employee salary expense Token number] [nvarchar](250) NULL,
	[Employee salary expense salary to be paid] [decimal](18, 2) NOT NULL,
	[Employee salary expense Date] [datetime] NOT NULL,
	[Employee salary expense ExpenseId] [nvarchar](250) NULL,
 CONSTRAINT [PK_Balance calculation data table] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Barcode Master]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Barcode Master](
	[Barcode Number] [nvarchar](50) NOT NULL,
	[Billing Number] [nvarchar](max) NULL,
	[Billing Token number] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[Marchent Token number] [nvarchar](250) NULL,
	[Image] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_Barcode_Masters] PRIMARY KEY CLUSTERED 
(
	[Barcode Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Billing Details]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Billing Details](
	[Billing details number] [int] IDENTITY(1,1) NOT NULL,
	[Billing Token number] [nvarchar](250) NULL,
	[Billing number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Selling item token] [nvarchar](250) NULL,
	[Pieces] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[Sub Total] [decimal](18, 2) NOT NULL,
	[Selling item id] [nvarchar](250) NULL,
	[IsGstPercent] [bit] NULL,
 CONSTRAINT [PK_Billing_Details] PRIMARY KEY CLUSTERED 
(
	[Billing details number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Billing Master]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Billing Master](
	[Token Number] [nvarchar](250) NOT NULL,
	[Billing Number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Total tax] [decimal](18, 2) NOT NULL,
	[Rate including tax] [decimal](18, 2) NOT NULL,
	[Total discount] [decimal](18, 2) NOT NULL,
	[Total amount] [decimal](18, 2) NOT NULL,
	[Discountper] [bit] NOT NULL,
	[Narration] [nvarchar](max) NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
	[Amount paid] [decimal](18, 2) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Mode of payment] [nvarchar](50) NULL,
	[Transaction Id] [nvarchar](500) NULL,
	[Customer token number] [nvarchar](250) NULL,
	[Discount] [decimal](18, 2) NULL,
	[IsGstPercent] [bit] NULL,
 CONSTRAINT [PK_Billing_Masters] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Token number] [nvarchar](250) NOT NULL,
	[Customer Name] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Phone number] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[Vehicle type] [nvarchar](250) NULL,
	[Vehicle number] [nvarchar](50) NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED 
(
	[Vehicle number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Customer shipping address]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer shipping address](
	[Customer token number] [nvarchar](250) NOT NULL,
	[First name] [nvarchar](250) NULL,
	[Last name] [nvarchar](250) NULL,
	[Address Line1] [nvarchar](250) NULL,
	[Address Line2] [nvarchar](250) NULL,
	[Town City] [nvarchar](250) NULL,
	[State] [nvarchar](50) NULL,
	[Pin code] [nvarchar](50) NULL,
	[Phone number] [nvarchar](50) NULL,
	[Email] [nvarchar](250) NULL,
	[IsUser] [bit] NOT NULL,
 CONSTRAINT [PK_Customer_shipping_addresses] PRIMARY KEY CLUSTERED 
(
	[Customer token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Dealer]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Dealer](
	[Token number] [nvarchar](250) NOT NULL,
	[Dealer code] [nvarchar](50) NULL,
	[Name] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Address] [nvarchar](max) NULL,
	[Office number] [nvarchar](50) NULL,
	[Phone number] [nvarchar](50) NULL,
	[GST number] [nvarchar](150) NULL,
	[Pan number] [nvarchar](50) NULL,
	[State] [int] NULL,
	[State Name] [nvarchar](50) NULL,
	[Isactive] [bit] NOT NULL,
	[Marchent Token number] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Dealers] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employee]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[Token number] [nvarchar](50) NOT NULL,
	[Employee Id] [nvarchar](50) NOT NULL,
	[Employee name] [nvarchar](100) NULL,
	[Designation] [nvarchar](250) NULL,
	[Joining date] [datetime] NOT NULL,
	[Contact number] [nvarchar](50) NULL,
	[Email id] [nvarchar](250) NULL,
	[Salary] [decimal](18, 2) NOT NULL,
	[Leaving date] [datetime] NULL,
	[login required] [bit] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[Image path] [nvarchar](250) NULL,
	[Password] [nvarchar](50) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Employees] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Employee salary expense]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee salary expense](
	[Token number] [nvarchar](250) NOT NULL,
	[Advance collected] [decimal](18, 2) NOT NULL,
	[salary to be paid] [decimal](18, 2) NOT NULL,
	[Employee token number] [nvarchar](250) NULL,
	[Employee name] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[ExpenseId] [nvarchar](250) NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
	[Monthly salary] [decimal](18, 2) NOT NULL,
	[Salary paid] [decimal](18, 2) NOT NULL,
	[Month] [int] NOT NULL,
	[Year] [int] NOT NULL,
	[Salary updated date] [datetime] NOT NULL,
	[Updated salary] [decimal](18, 2) NOT NULL,
	[Salary paid date] [datetime] NOT NULL,
 CONSTRAINT [PK_Employee_salary_expenses] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Item Tube]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item Tube](
	[Token number] [nvarchar](50) NOT NULL,
	[Item Id] [nvarchar](50) NULL,
	[Size token] [nvarchar](50) NULL,
	[Tube size] [nvarchar](50) NULL,
	[Company token] [nvarchar](250) NULL,
	[Company name] [nvarchar](50) NULL,
	[Description] [nvarchar](50) NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Item_Tubes] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Item Tyre]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Item Tyre](
	[Token number] [nvarchar](250) NOT NULL,
	[Item Id] [nvarchar](50) NULL,
	[Tyre make] [nvarchar](50) NULL,
	[Tyre type] [nvarchar](50) NULL,
	[Tyre feel] [nvarchar](50) NULL,
	[Company token] [nvarchar](250) NULL,
	[Company name] [nvarchar](50) NULL,
	[Tyre token] [nvarchar](50) NULL,
	[Tyre size] [nvarchar](50) NULL,
	[Vehicle token] [nvarchar](50) NULL,
	[Vehicle type] [nvarchar](50) NULL,
	[Description] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Item_Tyres] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Manager]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Manager](
	[Token number] [nvarchar](50) NOT NULL,
	[Manager name] [nvarchar](max) NULL,
	[Email Id] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Mobile] [nvarchar](50) NULL,
	[State Code] [int] NULL,
	[Pan Number] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[Verification code] [nvarchar](max) NULL,
	[State Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Managers] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Marchent Account]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marchent Account](
	[Token number] [nvarchar](50) NOT NULL,
	[Marchent name] [nvarchar](max) NULL,
	[Email Id] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Mobile] [nvarchar](50) NULL,
	[Telephone No] [nvarchar](50) NULL,
	[GSTIN Number] [nvarchar](max) NULL,
	[CIN Number] [nvarchar](max) NULL,
	[UIN Number] [nvarchar](max) NULL,
	[State Code] [int] NULL,
	[Pan Number] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[Verification code] [nvarchar](max) NULL,
	[State Name] [nvarchar](50) NULL,
	[License] [nvarchar](max) NULL,
 CONSTRAINT [PK_Marchent_Accounts] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Marchent account payment details]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Marchent account payment details](
	[GLCODE] [nvarchar](250) NOT NULL,
	[Account name] [nvarchar](max) NULL,
	[Card number] [nvarchar](50) NULL,
	[Marchent token] [nvarchar](250) NULL,
 CONSTRAINT [PK_Marchent_account_payment_details] PRIMARY KEY CLUSTERED 
(
	[GLCODE] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order Details]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order Details](
	[Order details number] [int] NOT NULL,
	[Order Token number] [nvarchar](250) NULL,
	[Order number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Selling item token] [nvarchar](250) NULL,
	[Pieces] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[Sub Total] [decimal](18, 2) NOT NULL,
	[Selling item id] [nvarchar](250) NULL,
	[IsGstPercent] [bit] NULL,
	[Tyre number] [nvarchar](50) NULL,
 CONSTRAINT [PK_Order_Details] PRIMARY KEY CLUSTERED 
(
	[Order details number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Order Master]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order Master](
	[Token Number] [nvarchar](250) NOT NULL,
	[Order Number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Total tax] [decimal](18, 2) NOT NULL,
	[Rate including tax] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NULL,
	[Total discount] [decimal](18, 2) NOT NULL,
	[Total amount] [decimal](18, 2) NOT NULL,
	[Discountper] [bit] NOT NULL,
	[Narration] [nvarchar](max) NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
	[Amount paid] [decimal](18, 2) NOT NULL,
	[Balance] [decimal](18, 2) NOT NULL,
	[Mode of payment] [nvarchar](50) NULL,
	[Transaction Id] [nvarchar](500) NULL,
	[Customer token number] [nvarchar](250) NULL,
	[IsGstPercent] [bit] NULL,
 CONSTRAINT [PK_Order_Masters] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Other expense]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Other expense](
	[Token number] [nvarchar](250) NOT NULL,
	[Other expense type] [nvarchar](250) NOT NULL,
	[Other expense amount] [decimal](18, 2) NOT NULL,
	[Date] [datetime] NOT NULL,
	[ExpenseId] [nvarchar](250) NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Other_expenses] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Other Product]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Other Product](
	[Token number] [nvarchar](250) NOT NULL,
	[Product name] [nvarchar](250) NULL,
	[Product type] [nvarchar](250) NULL,
	[Description] [nvarchar](300) NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[Product id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Other_Products] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Placed Order]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Placed Order](
	[Token number] [nvarchar](250) NOT NULL,
	[Item token] [nvarchar](250) NULL,
	[Pieces] [int] NOT NULL,
	[Orderplaced] [bit] NOT NULL,
	[Customer token] [nvarchar](250) NULL,
	[IsUser] [bit] NOT NULL,
	[Order Date] [datetime] NOT NULL,
	[Ispaid] [bit] NOT NULL,
	[Approve Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Placed_Orders] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Product expense]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product expense](
	[Token number] [nvarchar](250) NOT NULL,
	[Amount for expense] [decimal](18, 2) NOT NULL,
	[Product token number] [nvarchar](250) NULL,
	[Product name] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[ExpenseId] [nvarchar](250) NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Product_expenses] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[Token Number] [nvarchar](250) NOT NULL,
	[Product Code] [nvarchar](50) NULL,
	[Product name] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Products For Sale]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products For Sale](
	[Token Number] [nvarchar](250) NOT NULL,
	[Product Token] [nvarchar](250) NULL,
	[Product name] [nvarchar](250) NULL,
	[Tyre token] [nvarchar](50) NULL,
	[Tyre Size] [nvarchar](250) NULL,
	[Supplier token] [nvarchar](250) NULL,
	[Supplier name] [nvarchar](250) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Purchase Price] [decimal](18, 2) NOT NULL,
	[CGST] [decimal](18, 2) NOT NULL,
	[SGST] [decimal](18, 2) NOT NULL,
	[Pieces] [int] NOT NULL,
	[Selling Price] [decimal](18, 2) NOT NULL,
	[Amout after tax] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Approve date] [datetime] NOT NULL,
	[Approve] [bit] NOT NULL,
	[Administrator Token number] [nvarchar](250) NULL,
	[Administrator name] [nvarchar](250) NULL,
	[Delivery contact number] [bigint] NOT NULL,
	[Delivery address] [nvarchar](300) NULL,
	[Item tyre token] [nvarchar](250) NULL,
	[Item tyre Id] [nvarchar](250) NULL,
	[Purchase number] [nvarchar](250) NULL,
	[Vehicle Token] [nvarchar](50) NULL,
	[Vehicle type] [nvarchar](50) NULL,
	[Description] [nvarchar](300) NULL,
	[Tyre make] [nvarchar](50) NULL,
	[Tyre feel] [nvarchar](50) NULL,
	[Tyre type] [nvarchar](50) NULL,
	[Mac id] [nvarchar](250) NULL,
	[requestsend] [bit] NULL,
	[Selling CGST] [decimal](18, 2) NOT NULL,
	[Selling SGST] [decimal](18, 2) NOT NULL,
	[CGST-SGST-CHECK] [bit] NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
	[Approve user id] [nvarchar](50) NULL,
	[Approve user name] [nvarchar](100) NULL,
	[Rate update user id] [nvarchar](50) NULL,
	[Rate update user name] [nvarchar](100) NULL,
	[IsGstPercent] [bit] NOT NULL,
	[CalculationByRatePerUnit] [bit] NOT NULL,
	[Selling net total] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Products_For_Sales] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Proprietor]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Proprietor](
	[Token number] [nvarchar](50) NOT NULL,
	[Proprietor name] [nvarchar](max) NULL,
	[Email Id] [nvarchar](max) NULL,
	[Address] [nvarchar](max) NULL,
	[Mobile] [nvarchar](50) NULL,
	[State Code] [int] NULL,
	[Pan Number] [nvarchar](50) NULL,
	[IsActive] [bit] NOT NULL,
	[Verification code] [nvarchar](max) NULL,
	[State Name] [nvarchar](50) NULL,
 CONSTRAINT [PK_Proprietors] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Purchase details]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchase details](
	[Purchase details number] [int] IDENTITY(1,1) NOT NULL,
	[Purchase Token number] [nvarchar](250) NULL,
	[Purcahse number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Product Token] [nvarchar](250) NULL,
	[Product name] [nvarchar](250) NULL,
	[Pieces] [int] NOT NULL,
	[Quantity] [decimal](18, 3) NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Taxable amount] [decimal](18, 2) NOT NULL,
	[Tax Token] [nvarchar](250) NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[Discount percent] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[Sub Total] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Purchase_details] PRIMARY KEY CLUSTERED 
(
	[Purchase details number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Purchase Invoice]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchase Invoice](
	[Token number] [nvarchar](250) NOT NULL,
	[Purchase invoice number] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[Stock entry token] [nvarchar](250) NULL,
 CONSTRAINT [PK_Purchase_Invoices] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Purchase Master]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Purchase Master](
	[Token Number] [nvarchar](250) NOT NULL,
	[Purchase Number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Marchent Token number] [nvarchar](250) NULL,
	[Dealer Token number] [nvarchar](250) NULL,
	[Tax token] [nvarchar](250) NULL,
	[Total tax] [decimal](18, 2) NOT NULL,
	[Rate including tax] [decimal](18, 2) NOT NULL,
	[Discount percent] [decimal](18, 2) NOT NULL,
	[Total discount] [decimal](18, 2) NOT NULL,
	[Total amount] [decimal](18, 2) NOT NULL,
	[CGST] [decimal](18, 2) NOT NULL,
	[SGST] [decimal](18, 2) NOT NULL,
	[IGST] [decimal](18, 2) NOT NULL,
	[UTGST] [decimal](18, 2) NOT NULL,
	[Narration] [nvarchar](max) NULL,
 CONSTRAINT [PK_Purchase_Masters] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Quotation Details]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotation Details](
	[Quotation details number] [int] NOT NULL,
	[Quotation Token number] [nvarchar](250) NULL,
	[Quotation number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Selling item token] [nvarchar](250) NULL,
	[Pieces] [int] NOT NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Tax] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[Sub Total] [decimal](18, 2) NOT NULL,
	[Selling item id] [nvarchar](250) NULL,
	[IsGstPercent] [bit] NULL,
 CONSTRAINT [PK_Quotation_Details] PRIMARY KEY CLUSTERED 
(
	[Quotation details number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Quotation Master]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Quotation Master](
	[Token Number] [nvarchar](250) NOT NULL,
	[Quotation Number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Total tax] [decimal](18, 2) NOT NULL,
	[Rate including tax] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NULL,
	[Total discount] [decimal](18, 2) NOT NULL,
	[Total amount] [decimal](18, 2) NOT NULL,
	[Discountper] [bit] NOT NULL,
	[Narration] [nvarchar](max) NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
	[Transaction Id] [nvarchar](500) NULL,
	[Customer token number] [nvarchar](250) NULL,
	[IsGstPercent] [bit] NULL,
 CONSTRAINT [PK_Quotation_Masters] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Rate update Backup]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Rate update Backup](
	[Token number] [nvarchar](250) NOT NULL,
	[Item number] [nvarchar](250) NULL,
	[Date] [datetime] NULL,
	[Date of stock entry] [datetime] NULL,
	[Selling rate] [decimal](18, 2) NULL,
	[Selling CGST] [decimal](18, 2) NULL,
	[Selling SGST] [decimal](18, 2) NULL,
 CONSTRAINT [PK_Rate_update_Backups] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[State]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[State](
	[State Code] [int] NOT NULL,
	[Name] [nvarchar](250) NULL,
	[State Identity] [nvarchar](50) NULL,
	[CGST] [bit] NOT NULL,
	[SGST] [bit] NOT NULL,
	[UTGST] [bit] NOT NULL,
	[IGST] [bit] NOT NULL,
	[Marchent Token number] [nvarchar](250) NULL,
 CONSTRAINT [PK_States] PRIMARY KEY CLUSTERED 
(
	[State Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Stock]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stock](
	[Stock Id] [int] IDENTITY(1,1) NOT NULL,
	[Purchase Token number] [nvarchar](250) NULL,
	[Purcahse number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Remodify Date] [datetime] NOT NULL,
	[Product Token] [nvarchar](250) NULL,
	[Remodify pcs] [int] NOT NULL,
	[Pieces] [int] NOT NULL,
	[CGST] [decimal](18, 2) NOT NULL,
	[SGST] [decimal](18, 2) NOT NULL,
	[Product name] [nvarchar](250) NULL,
	[Marchent Token number] [nvarchar](250) NULL,
 CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED 
(
	[Stock Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StockForBillingRPT]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StockForBillingRPT](
	[Token Number] [nvarchar](250) NOT NULL,
	[Product name] [nvarchar](250) NULL,
	[Tyre Size] [nvarchar](250) NULL,
	[Selling Price] [decimal](18, 2) NOT NULL,
	[Item tyre Id] [nvarchar](250) NULL,
	[Selling CGST] [decimal](18, 2) NOT NULL,
	[Selling SGST] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_StockForBillingRPTs] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Stockout]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Stockout](
	[Stock out Id] [int] IDENTITY(1,1) NOT NULL,
	[Billing Token number] [nvarchar](250) NULL,
	[Billing number] [nvarchar](max) NULL,
	[Date] [datetime] NOT NULL,
	[Product Token] [nvarchar](250) NULL,
	[Pieces] [int] NOT NULL,
	[CGST] [decimal](18, 2) NOT NULL,
	[SGST] [decimal](18, 2) NOT NULL,
	[Sub Total] [decimal](18, 2) NOT NULL,
	[Marchent Token number] [nvarchar](250) NULL,
 CONSTRAINT [PK_Stockouts] PRIMARY KEY CLUSTERED 
(
	[Stock out Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tax Group]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tax Group](
	[Tax Token] [nvarchar](250) NOT NULL,
	[Tax Name] [nvarchar](250) NULL,
	[Tax Rate] [float] NOT NULL,
	[GL CODE] [nvarchar](250) NULL,
	[IsActive] [bit] NOT NULL,
	[Marchent Token number] [nvarchar](250) NULL,
 CONSTRAINT [PK_Tax_Groups] PRIMARY KEY CLUSTERED 
(
	[Tax Token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Temp_Bill]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temp_Bill](
	[Token Number] [nvarchar](250) NOT NULL,
	[Product Token] [nvarchar](250) NULL,
	[Product name] [nvarchar](250) NULL,
	[Tyre token] [nvarchar](50) NULL,
	[Tyre Size] [nvarchar](250) NULL,
	[Supplier token] [nvarchar](250) NULL,
	[Supplier name] [nvarchar](250) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Purchase Price] [decimal](18, 2) NOT NULL,
	[CGST] [decimal](18, 2) NOT NULL,
	[SGST] [decimal](18, 2) NOT NULL,
	[Pieces] [int] NOT NULL,
	[Selling Price] [decimal](18, 2) NOT NULL,
	[Amout after tax] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Approve date] [datetime] NOT NULL,
	[Approve] [bit] NOT NULL,
	[Administrator Token number] [nvarchar](250) NULL,
	[Administrator name] [nvarchar](250) NULL,
	[Delivery contact number] [bigint] NOT NULL,
	[Delivery address] [nvarchar](300) NULL,
	[Item tyre token] [nvarchar](250) NULL,
	[Item tyre Id] [nvarchar](250) NULL,
	[Purchase number] [nvarchar](250) NULL,
	[Vehicle Token] [nvarchar](50) NULL,
	[Vehicle type] [nvarchar](50) NULL,
	[Description] [nvarchar](300) NULL,
	[Tyre make] [nvarchar](50) NULL,
	[Tyre feel] [nvarchar](50) NULL,
	[Tyre type] [nvarchar](50) NULL,
	[Mac id] [nvarchar](250) NULL,
	[requestsend] [bit] NULL,
	[Selling CGST] [decimal](18, 2) NOT NULL,
	[Selling SGST] [decimal](18, 2) NOT NULL,
	[CGST-SGST-CHECK] [bit] NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
	[Approve user id] [nvarchar](50) NULL,
	[Approve user name] [nvarchar](100) NULL,
	[Rate update user id] [nvarchar](50) NULL,
	[Rate update user name] [nvarchar](100) NULL,
	[Tyre number] [nvarchar](50) NULL,
 CONSTRAINT [PK_Temp_Bill] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Temp_placedorder]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temp_placedorder](
	[Token number] [nvarchar](250) NOT NULL,
	[Item token] [nvarchar](250) NULL,
	[Pieces] [int] NULL,
	[Customer token] [nvarchar](250) NULL,
	[IsUser] [bit] NOT NULL,
	[Order Date] [datetime] NOT NULL,
 CONSTRAINT [PK_Temp_placedorder] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Temp_Stock]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temp_Stock](
	[Token Number] [nvarchar](250) NOT NULL,
	[Product Token] [nvarchar](250) NULL,
	[Product name] [nvarchar](250) NULL,
	[Tyre token] [nvarchar](50) NULL,
	[Tyre Size] [nvarchar](250) NULL,
	[Supplier token] [nvarchar](250) NULL,
	[Supplier name] [nvarchar](250) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Purchase Price] [decimal](18, 2) NOT NULL,
	[CGST] [decimal](18, 2) NOT NULL,
	[SGST] [decimal](18, 2) NOT NULL,
	[Pieces] [int] NOT NULL,
	[Selling Price] [decimal](18, 2) NOT NULL,
	[Amout after tax] [decimal](18, 2) NOT NULL,
	[Total] [decimal](18, 2) NOT NULL,
	[Approve date] [datetime] NOT NULL,
	[Approve] [bit] NOT NULL,
	[Administrator Token number] [nvarchar](250) NULL,
	[Administrator name] [nvarchar](250) NULL,
	[Delivery contact number] [bigint] NOT NULL,
	[Delivery address] [nvarchar](300) NULL,
	[Item tyre token] [nvarchar](250) NULL,
	[Item tyre Id] [nvarchar](250) NULL,
	[Purchase number] [nvarchar](250) NULL,
	[Vehicle Token] [nvarchar](50) NULL,
	[Vehicle type] [nvarchar](50) NULL,
	[Description] [nvarchar](300) NULL,
	[Tyre make] [nvarchar](50) NULL,
	[Tyre feel] [nvarchar](50) NULL,
	[Tyre type] [nvarchar](50) NULL,
	[Mac id] [nvarchar](250) NULL,
	[requestsend] [bit] NULL,
	[IsGstPercent] [bit] NOT NULL,
	[CalculationByRatePerUnit] [bit] NOT NULL,
 CONSTRAINT [PK_Temp_Stock] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Transaction amount details]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Transaction amount details](
	[Token Number] [nvarchar](250) NOT NULL,
	[Marchent Token] [nvarchar](250) NULL,
	[Purchase Token] [nvarchar](250) NULL,
	[Purchase number] [nvarchar](max) NULL,
	[Sale invoice token] [nvarchar](250) NULL,
	[Sale invoice number] [nvarchar](max) NULL,
	[GL CODE] [nvarchar](250) NULL,
	[Amount] [decimal](18, 2) NOT NULL,
	[Card number] [nvarchar](50) NULL,
	[Cheque number] [nvarchar](50) NULL,
 CONSTRAINT [PK_Transaction_amount_details] PRIMARY KEY CLUSTERED 
(
	[Token Number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Tyre size]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tyre size](
	[Token number] [nvarchar](50) NOT NULL,
	[Tyre size] [nvarchar](50) NULL,
	[With tube] [bit] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Tyre_sizes] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Token number] [nvarchar](250) NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Email] [nvarchar](250) NULL,
	[Password] [nvarchar](250) NULL,
	[Phone number] [nvarchar](50) NULL,
	[Alternate phone number] [nvarchar](50) NULL,
	[State] [int] NULL,
	[State Name] [nvarchar](50) NULL,
	[Address] [nvarchar](max) NULL,
	[Second Address] [nvarchar](max) NULL,
	[IsActive] [bit] NOT NULL,
	[Marchent Token number] [nvarchar](250) NULL,
	[Applied date] [datetime] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Vehicle]    Script Date: 01-May-20 12:03:45 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vehicle](
	[Token number] [nvarchar](50) NOT NULL,
	[Vehicle type] [nvarchar](250) NULL,
	[Vehicle make] [nvarchar](250) NULL,
	[Date] [datetime] NOT NULL,
	[Mac id] [nvarchar](250) NULL,
	[User Id] [nvarchar](50) NULL,
	[User name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Vehicles] PRIMARY KEY CLUSTERED 
(
	[Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'2e2ee389-1952-4b83-912d-b302b127a7c0', CAST(0x0000AB53000390DF AS DateTime), N'385f9700-5313-4434-b277-e3b05201ed55', N'KANP000116', CAST(0x0000AB53000390DF AS DateTime), CAST(8.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), CAST(4.84 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB53000390DF AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB53000390DF AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB53000390DF AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB53000390DF AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'37abce7a-899f-4ce2-b89a-13af7df5cbed', CAST(0x0000AB61007372E1 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'71af3860-0c64-4253-868d-956896ec7a9b', CAST(960.00 AS Decimal(18, 2)), CAST(0x0000AB61007372E1 AS DateTime), N'ESAL0000009')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'4e3b515e-6504-479b-b24b-98f5f8a8fdfa', CAST(0x0000AB5001537843 AS DateTime), N'613c434d-3ae1-4ce6-8bd2-546f2918b010', N'KANP000113', CAST(0x0000AB5001537843 AS DateTime), CAST(1200.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(1076.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB5001537843 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001537843 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001537843 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001537843 AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'52379b6b-6ad0-4ea6-9946-0c1b427f45a9', CAST(0x0000AB5B017ED898 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'd019d10c-dc0e-4345-8a10-759d05b92cce', CAST(48980.00 AS Decimal(18, 2)), CAST(0x0000AB5B017ED898 AS DateTime), N'ESAL0000008')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'653179cb-faf5-49f4-a000-91ba5aa27a5a', CAST(0x0000AB5B017EBDA4 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'b17065ab-dca8-4a1e-9f9a-1d9a62e03e3b', CAST(49980.00 AS Decimal(18, 2)), CAST(0x0000AB5B017EBDA4 AS DateTime), N'ESAL0000007')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'681db0e9-8058-4c59-be60-2f30ec10e332', CAST(0x0000AB59009BDD78 AS DateTime), N'b3fb807d-0402-4bf6-8721-288d4924a218', N'KANP000117', CAST(0x0000AB59009BDD78 AS DateTime), CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB59009BDD78 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB59009BDD78 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB59009BDD78 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB59009BDD78 AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'775f54ea-3339-4408-916c-8e1d4718be42', CAST(0x0000AB2B00000000 AS DateTime), N'53206f4d-13a4-48d1-8da2-b3e8ba5ff801', N'KANP000111', CAST(0x0000AB2B00000000 AS DateTime), CAST(220.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(115.60 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB2B00000000 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB2B00000000 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB2B00000000 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB2B00000000 AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'857c2de2-699b-41d9-819c-bb14d3b4dde9', CAST(0x0000AB380181C698 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'96c5bd52-8b3a-4b37-9743-88e02af3d1d1', CAST(90.00 AS Decimal(18, 2)), CAST(0x0000AB380181C698 AS DateTime), N'ESAL0000001')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'937c5380-efe3-4212-a869-355af298b856', CAST(0x0000AB5001578C1C AS DateTime), N'91cff023-b8de-4e42-8df1-da89b5d66600', N'KANP000115', CAST(0x0000AB5001578C1C AS DateTime), CAST(500.00 AS Decimal(18, 2)), CAST(12.00 AS Decimal(18, 2)), CAST(478.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB5001578C1C AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001578C1C AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001578C1C AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001578C1C AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'a0c2e28f-9e72-4aa7-b285-d41e17ee6132', CAST(0x0000AB3801873920 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'222c0c32-6837-4cd5-bbb5-b432c6f101c2', CAST(60.00 AS Decimal(18, 2)), CAST(0x0000AB3801873920 AS DateTime), N'ESAL0000003')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'ac9d8241-40fc-4132-8de6-3b09bf713e48', CAST(0x0000AB6100000000 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'50ff0bbd-888a-45b4-b0a9-186664bb01b6', CAST(200.00 AS Decimal(18, 2)), CAST(0x0000AB6100000000 AS DateTime), N'EPRO0000001', NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'b49de362-0b57-4695-958e-701791eba069', CAST(0x0000AB25015F9A53 AS DateTime), N'53206f4d-13a4-48d1-8da2-b3e8ba5ff701', N'KANP000112', CAST(0x0000AB25015F9A53 AS DateTime), CAST(220.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(115.60 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB25015F9A53 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB25015F9A53 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB25015F9A53 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB25015F9A53 AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'bb413b86-3e25-48c3-97c1-c810f33a184c', CAST(0x0000AB2A00000000 AS DateTime), N'53206f4d-13a4-48d1-8da2-b3e8ba5f801', N'KANP000110', CAST(0x0000AB2A00000000 AS DateTime), CAST(220.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(115.60 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB2A00000000 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB2A00000000 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB2A00000000 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB2A00000000 AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'd0190596-f315-45f8-b475-ab4210c62ffd', CAST(0x0000AB5900942FF0 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'58ca05a2-1054-492c-89b5-e61a00fe4933', CAST(-30.00 AS Decimal(18, 2)), CAST(0x0000AB5900942FF0 AS DateTime), N'ESAL0000005')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'd2140985-f894-47fe-99b0-4cf12a0e5cd4', CAST(0x0000AB5B017ADD10 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'cecd6f42-0793-4926-ab77-c34cf5b7930d', CAST(9878.00 AS Decimal(18, 2)), CAST(0x0000AB5B017ADD10 AS DateTime), N'ESAL0000006')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'e251e30b-6e80-4f59-a47d-bc4a6f35d2ee', CAST(0x0000AB5001574F04 AS DateTime), N'a1ee592e-40d5-4a18-9959-af02fcb43e9c', N'KANP000114', CAST(0x0000AB5001574F04 AS DateTime), CAST(400.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(391.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB5001574F04 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001574F04 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001574F04 AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB5001574F04 AS DateTime), NULL)
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'e6c00d1f-0224-4b18-a83b-62505565d80f', CAST(0x0000AB380181CFF8 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'2b07c2c9-d54e-44bc-9b10-6f03111d2dbf', CAST(70.00 AS Decimal(18, 2)), CAST(0x0000AB380181CFF8 AS DateTime), N'ESAL0000002')
INSERT [dbo].[Balance calculation data table] ([Token Number], [Date], [Billing Token Number], [Billing Number], [Bill Date], [Billing Total amount], [Billing Amount paid], [Billing Balance], [Order Token Number], [Order Number], [Order Date], [Order Total amount], [Order Amount paid], [Order Balance], [Other expense Token number], [Other expense amount], [Other expense Date], [Other expense ExpenseId], [Product expense Token number], [Product expense Amount for expense], [Product expense Date], [Product expense ExpenseId], [Employee salary expense Token number], [Employee salary expense salary to be paid], [Employee salary expense Date], [Employee salary expense ExpenseId]) VALUES (N'eba68c63-a01c-4e35-ac94-378edb152f05', CAST(0x0000AB39000802C8 AS DateTime), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, NULL, CAST(0x0000AB62004FC75A AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0x0000AB62004FC75A AS DateTime), NULL, N'df8dc2da-ffbd-446a-814e-e5a4157a78ea', CAST(-30.00 AS Decimal(18, 2)), CAST(0x0000AB39000802C8 AS DateTime), N'ESAL0000004')
SET IDENTITY_INSERT [dbo].[Billing Details] ON 

INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (1, N'53206f4d-13a4-48d1-8da2-b3e8ba5ff701', N'KANP000112', CAST(0x0000AB25015F9A53 AS DateTime), N'e35c08bc-3bee-4233-8f04-7d9494e6b9db', 1, CAST(220.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), N'TYAPO6.00/16TUL-2855', 1)
INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (2, N'53206f4d-13a4-48d1-8da2-b3e8ba5ff801', N'KANP000111', CAST(0x0000AB2B00000000 AS DateTime), N'e35c08bc-3bee-4233-8f04-7d9494e6b9db', 1, CAST(220.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), N'TYAPO6.00/16TUL-2855', 1)
INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (3, N'53206f4d-13a4-48d1-8da2-b3e8ba5f801', N'KANP000110', CAST(0x0000AB2A00000000 AS DateTime), N'e35c08bc-3bee-4233-8f04-7d9494e6b9db', 1, CAST(220.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), N'TYAPO6.00/16TUL-2855', 1)
INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (4, N'613c434d-3ae1-4ce6-8bd2-546f2918b010', N'KANP000113', CAST(0x0000AB5001537843 AS DateTime), N'de730c58-a20c-450e-a682-72051fedf82f', 5, CAST(1200.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(1200.00 AS Decimal(18, 2)), N'PO-9729', 1)
INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (5, N'a1ee592e-40d5-4a18-9959-af02fcb43e9c', N'KANP000114', CAST(0x0000AB5001574F04 AS DateTime), N'de730c58-a20c-450e-a682-72051fedf82f', 2, CAST(400.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), N'PO-9729', 1)
INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (6, N'91cff023-b8de-4e42-8df1-da89b5d66600', N'KANP000115', CAST(0x0000AB5001578C1C AS DateTime), N'de730c58-a20c-450e-a682-72051fedf82f', 1, CAST(500.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), N'PO-9729', 1)
INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (7, N'385f9700-5313-4434-b277-e3b05201ed55', N'KANP000116', CAST(0x0000AB53000390DF AS DateTime), N'de730c58-a20c-450e-a682-72051fedf82f', 1, CAST(8.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), N'PO-9729', 1)
INSERT [dbo].[Billing Details] ([Billing details number], [Billing Token number], [Billing number], [Date], [Selling item token], [Pieces], [Amount], [Tax], [Discount], [Sub Total], [Selling item id], [IsGstPercent]) VALUES (8, N'b3fb807d-0402-4bf6-8721-288d4924a218', N'KANP000117', CAST(0x0000AB59009BDD78 AS DateTime), N'de730c58-a20c-450e-a682-72051fedf82f', 1, CAST(100.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'PO-9729', 1)
SET IDENTITY_INSERT [dbo].[Billing Details] OFF
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'385f9700-5313-4434-b277-e3b05201ed55', N'KANP000116', CAST(0x0000AB53000390DF AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), CAST(0.16 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), 1, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(3.00 AS Decimal(18, 2)), CAST(4.84 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(2.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'53206f4d-13a4-48d1-8da2-b3e8ba5f801', N'KANP000110', CAST(0x0000AB2A00000000 AS DateTime), CAST(10.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), CAST(4.40 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), 1, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(100.00 AS Decimal(18, 2)), CAST(115.60 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(2.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'53206f4d-13a4-48d1-8da2-b3e8ba5ff701', N'KANP000112', CAST(0x0000AB25015F9A53 AS DateTime), CAST(10.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), CAST(4.40 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), 1, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(100.00 AS Decimal(18, 2)), CAST(115.60 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(2.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'53206f4d-13a4-48d1-8da2-b3e8ba5ff801', N'KANP000111', CAST(0x0000AB2B00000000 AS DateTime), CAST(10.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), CAST(4.40 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), 1, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(100.00 AS Decimal(18, 2)), CAST(115.60 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(2.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'613c434d-3ae1-4ce6-8bd2-546f2918b010', N'KANP000113', CAST(0x0000AB5001537843 AS DateTime), CAST(20.00 AS Decimal(18, 2)), CAST(1200.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), CAST(1200.00 AS Decimal(18, 2)), 1, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(100.00 AS Decimal(18, 2)), CAST(1076.00 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(2.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'91cff023-b8de-4e42-8df1-da89b5d66600', N'KANP000115', CAST(0x0000AB5001578C1C AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(500.00 AS Decimal(18, 2)), 1, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(12.00 AS Decimal(18, 2)), CAST(478.00 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(2.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'a1ee592e-40d5-4a18-9959-af02fcb43e9c', N'KANP000114', CAST(0x0000AB5001574F04 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), CAST(8.00 AS Decimal(18, 2)), CAST(400.00 AS Decimal(18, 2)), 1, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(1.00 AS Decimal(18, 2)), CAST(391.00 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(2.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Billing Master] ([Token Number], [Billing Number], [Date], [Total tax], [Rate including tax], [Total discount], [Total amount], [Discountper], [Narration], [Mac id], [User Id], [User name], [Amount paid], [Balance], [Mode of payment], [Transaction Id], [Customer token number], [Discount], [IsGstPercent]) VALUES (N'b3fb807d-0402-4bf6-8721-288d4924a218', N'KANP000117', CAST(0x0000AB59009BDD78 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), 0, N'', N'B808CFCEAB3B', N'KAN8927', NULL, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), N'Cash', N'', N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', CAST(100.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Customer] ([Token number], [Customer Name], [Email], [Phone number], [Address], [Vehicle type], [Vehicle number], [Mac id], [User Id], [User name]) VALUES (N'0293aaac-9a37-46d4-96a8-7c3c80badb1a', N'Sourav', N'souravganguly@gmail.com', N'8334895299', N'Bangalore', N'3 wheeler + ggcghvhv', N'Veh001', N'B808CFCEAB3B', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Dealer] ([Token number], [Dealer code], [Name], [Email], [Address], [Office number], [Phone number], [GST number], [Pan number], [State], [State Name], [Isactive], [Marchent Token number], [Date], [Mac id], [User Id], [User name]) VALUES (N'3296bca8-9969-4741-a877-6654f9dc49a6', NULL, N'SONA MOTORS', N'sonatvs@gmail.com', N'JLB ROAD LAKSHMIPURAM MYSORE-570004', N'8274247525', N'2424242424', N'29AAQFS8663R1ZQ', N'', NULL, NULL, 1, NULL, CAST(0x0000AA8000853590 AS DateTime), N'00155DDC8301', N'', NULL)
INSERT [dbo].[Dealer] ([Token number], [Dealer code], [Name], [Email], [Address], [Office number], [Phone number], [GST number], [Pan number], [State], [State Name], [Isactive], [Marchent Token number], [Date], [Mac id], [User Id], [User name]) VALUES (N'369824d9-e05d-4d64-9cfe-0bf6444b75ab', NULL, N'Sourav Ganguly', N'souravganguly707@gmail.com', N'SRI NILAYAM PG(GENTS), #19, 3 rd Cross, Green Garden Layout, Sai Nagar Road, Kundalahalli Gate, Bangalore', N'', N'8334895299', N'', N'', NULL, NULL, 1, NULL, CAST(0x0000AA330041A4B0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Dealer] ([Token number], [Dealer code], [Name], [Email], [Address], [Office number], [Phone number], [GST number], [Pan number], [State], [State Name], [Isactive], [Marchent Token number], [Date], [Mac id], [User Id], [User name]) VALUES (N'5ad8a35b-ee3e-4ced-b28d-89325e688b15', NULL, N'SONA MOTORS', N'sonatvs@gmail.com', N'JLB ROAD LAKSHMIPURAM MYSORE-570004', N'8274247525', N'2424242424', N'29AAQFS8663R1ZQ', N'', NULL, NULL, 1, NULL, CAST(0x0000AA8000853590 AS DateTime), N'00155DDC8301', N'', NULL)
INSERT [dbo].[Dealer] ([Token number], [Dealer code], [Name], [Email], [Address], [Office number], [Phone number], [GST number], [Pan number], [State], [State Name], [Isactive], [Marchent Token number], [Date], [Mac id], [User Id], [User name]) VALUES (N'7d570b4f-d0b5-4ba8-8a1b-f46fed7097bb', NULL, N'APOLLO TYRES LTD', N'apollo@apollotyres.com', N'C124, INDUSTRIAL ESTATE, YADAVAGIRI MYSORE,570020', N'8212512056', N'9986716744', N'29AAACA6990QIZU', N'AAACA6990Q', NULL, NULL, 1, NULL, CAST(0x0000AA3E006D0A10 AS DateTime), N'00155DDC8301', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Dealer] ([Token number], [Dealer code], [Name], [Email], [Address], [Office number], [Phone number], [GST number], [Pan number], [State], [State Name], [Isactive], [Marchent Token number], [Date], [Mac id], [User Id], [User name]) VALUES (N'c98974d7-d65e-42c2-88e0-440291f7af0a', NULL, N'HINDUSTAN AGENCIES', N'danthihindusthan@yahoo.co.in', N'#154 RAMAVILAS ROAD K.R MOHALLA MYSORE', N'247-2472222', N'1234567891', N'29AEIPK0164P1Z1', N'', NULL, NULL, 1, NULL, CAST(0x0000AA3B00834960 AS DateTime), N'00155DDC8301', N'', NULL)
INSERT [dbo].[Dealer] ([Token number], [Dealer code], [Name], [Email], [Address], [Office number], [Phone number], [GST number], [Pan number], [State], [State Name], [Isactive], [Marchent Token number], [Date], [Mac id], [User Id], [User name]) VALUES (N'f8f7144f-4ef9-4173-8da7-df5ac1b58132', NULL, N'PULIYORATH AUTOMOBILES', N'puliyorathmdy@gmail.com', N'PULIYORATH AUTOMOBILES CALICUT ROAD, MANANTHAVADY, WAYANAD', N'', N'8111952194', N'32ALTPM8244N1ZD', N'', NULL, NULL, 1, NULL, CAST(0x0000AA07004C5AE0 AS DateTime), N'00155DDC8301', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Employee] ([Token number], [Employee Id], [Employee name], [Designation], [Joining date], [Contact number], [Email id], [Salary], [Leaving date], [login required], [Date], [Mac id], [Image path], [Password], [User Id], [User name]) VALUES (N'385cd7c3-015b-4e64-8b01-348942b0d18a', N'KAN5949', N'soutV', N'Employee', CAST(0x0000AB5B00000000 AS DateTime), N'8987656787', N'souravganguly707@gmail.com', CAST(9898.00 AS Decimal(18, 2)), CAST(0x00008D3F00000000 AS DateTime), 0, CAST(0x0000AB5B0170C19D AS DateTime), N'A0B3CC7724D8', NULL, N'LJKV5069LJ', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Employee] ([Token number], [Employee Id], [Employee name], [Designation], [Joining date], [Contact number], [Email id], [Salary], [Leaving date], [login required], [Date], [Mac id], [Image path], [Password], [User Id], [User name]) VALUES (N'f8e47ac7-3265-418e-9536-7a99e239aea5', N'KAN4867', N'Sumit', N'Employee', CAST(0x0000AB5400000000 AS DateTime), N'8765432343', N'sumit@gmail.com', CAST(50000.00 AS Decimal(18, 2)), CAST(0x00008D3F00000000 AS DateTime), 0, CAST(0x0000AB5A01599049 AS DateTime), N'A0B3CC7724D8', NULL, N'CREQ1837CR', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Employee] ([Token number], [Employee Id], [Employee name], [Designation], [Joining date], [Contact number], [Email id], [Salary], [Leaving date], [login required], [Date], [Mac id], [Image path], [Password], [User Id], [User name]) VALUES (N'fd7a4a83-4903-442b-9624-e56959d49fad', N'KAN8927', N'Akshay CK', N'Propritor', CAST(0x0000AB1900000000 AS DateTime), N'7338558449', N'akshayck777@gmail.com', CAST(2000.00 AS Decimal(18, 2)), CAST(0x0000902C00000000 AS DateTime), 0, CAST(0x0000AB5A00000000 AS DateTime), N'00155DDC8301', NULL, N'FZIX3047FZ', N'KAN7597', N'fghr44444')
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'222c0c32-6837-4cd5-bbb5-b432c6f101c2', CAST(40.00 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), N'fd7a4a83-4903-442b-9624-e56959d49fad', N'Akshay CK', CAST(0x0000AB3801873920 AS DateTime), N'ESAL0000003', N'B808CFCEAB3B', N'KAN8927', N'Akshay CK', CAST(100.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AA3101502E80 AS DateTime), CAST(200.00 AS Decimal(18, 2)), CAST(0x0000AB57018739B8 AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'2b07c2c9-d54e-44bc-9b10-6f03111d2dbf', CAST(30.00 AS Decimal(18, 2)), CAST(70.00 AS Decimal(18, 2)), N'fd7a4a83-4903-442b-9624-e56959d49fad', N'Akshay CK', CAST(0x0000AB380181CFF8 AS DateTime), N'ESAL0000002', N'B808CFCEAB3B', N'KAN8927', N'Akshay CK', CAST(100.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AA3101502E80 AS DateTime), CAST(100.00 AS Decimal(18, 2)), CAST(0x0000AB570181D033 AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'58ca05a2-1054-492c-89b5-e61a00fe4933', CAST(1030.00 AS Decimal(18, 2)), CAST(-30.00 AS Decimal(18, 2)), N'fd7a4a83-4903-442b-9624-e56959d49fad', N'Akshay CK', CAST(0x0000AB5900942FF0 AS DateTime), N'ESAL0000005', N'B808CFCEAB3B', N'KAN8927', N'Akshay CK', CAST(1000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AB5900000000 AS DateTime), CAST(1000.00 AS Decimal(18, 2)), CAST(0x0000AB5900943033 AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'71af3860-0c64-4253-868d-956896ec7a9b', CAST(1040.00 AS Decimal(18, 2)), CAST(960.00 AS Decimal(18, 2)), N'fd7a4a83-4903-442b-9624-e56959d49fad', N'Akshay CK', CAST(0x0000AB61007372E1 AS DateTime), N'ESAL0000009', N'A0B3CC7724D8', N'KAN8927', N'Akshay CK', CAST(2000.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AB5A00000000 AS DateTime), CAST(2000.00 AS Decimal(18, 2)), CAST(0x0000AB610073732B AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'96c5bd52-8b3a-4b37-9743-88e02af3d1d1', CAST(10.00 AS Decimal(18, 2)), CAST(90.00 AS Decimal(18, 2)), N'fd7a4a83-4903-442b-9624-e56959d49fad', N'Akshay CK', CAST(0x0000AB380181C698 AS DateTime), N'ESAL0000001', N'B808CFCEAB3B', N'KAN8927', N'Akshay CK', CAST(100.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AA3101502E80 AS DateTime), CAST(100.00 AS Decimal(18, 2)), CAST(0x0000AB570181C740 AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'b17065ab-dca8-4a1e-9f9a-1d9a62e03e3b', CAST(20.00 AS Decimal(18, 2)), CAST(49980.00 AS Decimal(18, 2)), N'f8e47ac7-3265-418e-9536-7a99e239aea5', N'Sumit', CAST(0x0000AB5B017EBDA4 AS DateTime), N'ESAL0000007', N'A0B3CC7724D8', N'KAN4867', N'Sumit', CAST(50000.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AB5A01599049 AS DateTime), CAST(50000.00 AS Decimal(18, 2)), CAST(0x0000AB5B017EBE49 AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'cecd6f42-0793-4926-ab77-c34cf5b7930d', CAST(20.00 AS Decimal(18, 2)), CAST(9878.00 AS Decimal(18, 2)), N'385cd7c3-015b-4e64-8b01-348942b0d18a', N'soutV', CAST(0x0000AB5B017ADD10 AS DateTime), N'ESAL0000006', N'A0B3CC7724D8', N'KAN8927', N'Akshay CK', CAST(2000.00 AS Decimal(18, 2)), CAST(20.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AB5A00000000 AS DateTime), CAST(2000.00 AS Decimal(18, 2)), CAST(0x0000AB5B017ADE73 AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'd019d10c-dc0e-4345-8a10-759d05b92cce', CAST(1020.00 AS Decimal(18, 2)), CAST(48980.00 AS Decimal(18, 2)), N'f8e47ac7-3265-418e-9536-7a99e239aea5', N'Sumit', CAST(0x0000AB5B017ED898 AS DateTime), N'ESAL0000008', N'A0B3CC7724D8', N'KAN4867', N'Sumit', CAST(50000.00 AS Decimal(18, 2)), CAST(1000.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AB5A01599049 AS DateTime), CAST(50000.00 AS Decimal(18, 2)), CAST(0x0000AB5B017ED9D0 AS DateTime))
INSERT [dbo].[Employee salary expense] ([Token number], [Advance collected], [salary to be paid], [Employee token number], [Employee name], [Date], [ExpenseId], [Mac id], [User Id], [User name], [Monthly salary], [Salary paid], [Month], [Year], [Salary updated date], [Updated salary], [Salary paid date]) VALUES (N'df8dc2da-ffbd-446a-814e-e5a4157a78ea', CAST(130.00 AS Decimal(18, 2)), CAST(-30.00 AS Decimal(18, 2)), N'fd7a4a83-4903-442b-9624-e56959d49fad', N'Akshay CK', CAST(0x0000AB39000802C8 AS DateTime), N'ESAL0000004', N'B808CFCEAB3B', N'KAN8927', N'Akshay CK', CAST(100.00 AS Decimal(18, 2)), CAST(90.00 AS Decimal(18, 2)), 2, 2020, CAST(0x0000AA3101502E80 AS DateTime), CAST(200.00 AS Decimal(18, 2)), CAST(0x0000AB580008033D AS DateTime))
INSERT [dbo].[Item Tube] ([Token number], [Item Id], [Size token], [Tube size], [Company token], [Company name], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'780ed7f2-55dc-4355-9299-3d55dddd32cf', N'TUcor3x7-3046', N'4ba40273-7073-41f3-8237-c5418e71b268', N'3x7', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'', CAST(0x0000AA8E0047AF90 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Item Tube] ([Token number], [Item Id], [Size token], [Tube size], [Company token], [Company name], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'8ed53f1b-cb4c-4d08-b19d-6bfa8c3c38a8', N'TUcor3x1-878', N'2313fb42-44bf-4417-a642-88141fddcc1e', N'3x1', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'', CAST(0x0000AA9401791B10 AS DateTime), N'00155DC00109', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'46d25d5f-8500-4e72-bddd-1d77274b64ed', N'TYmrf6000/15TUB-8096', N'New', N'Tube', N'Normal', N'b3b60c2f-36df-4842-8907-4ca2a03bc854', N'mrf', N'2246fb52-6e68-46e9-b958-6dee63af159a', N'6000/15', N'78c7aa0e-c579-452a-b05b-f08f1a3a1da2', N'4 wheeler + jeep', N'', CAST(0x0000AA690109DE80 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'7519b844-b888-4759-abdc-9f67211b06eb', N'TYcor3x7TUB-7496', N'Resole', N'Tube', N'Normal', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'4ba40273-7073-41f3-8237-c5418e71b268', N'3x7', N'ce45a90f-8b43-4268-a255-0f29bb632870', N'3 wheeler + maker', N'', CAST(0x0000A9CC00D9CDD0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'7a783509-c0f3-42ba-83aa-a8b7db341a37', N'TYcor4x1TUB-1025', N'New', N'Tube', N'Normal', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'67fed3a6-5ad4-4dc8-a7a9-0c961cb7fb5b', N'4x1', N'45324e81-f80e-4b8e-817f-d06c8e0587d9', N'3 wheeler + ggcghvhv', N'', CAST(0x0000AA67016135E0 AS DateTime), N'00155DC00109', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'9792df5c-8483-4380-9146-c1a3ca442be1', N'TYcor3x7TUL-8374', N'Old', N'Tubeless', N'Normal', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'4ba40273-7073-41f3-8237-c5418e71b268', N'3x7', N'ce45a90f-8b43-4268-a255-0f29bb632870', N'3 wheeler + maker', N'', CAST(0x0000AA31017B4D90 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'a4edcdec-5bc2-4d90-ade0-e89f0d249174', N'TYcor3x7TUB-4447', N'Old', N'Tube', N'Normal', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'4ba40273-7073-41f3-8237-c5418e71b268', N'3x7', N'ce45a90f-8b43-4268-a255-0f29bb632870', N'3 wheeler + maker', N'', CAST(0x0000AA31017AC0F0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'cab26fba-1b7f-45f5-b2e3-b06f8761195d', N'TYAPO6.00/16TUL-2855', N'New', N'Tubeless', N'Radial', N'bb27a8af-00f2-4c3f-a58b-7a1d28252c08', N'APOLLO', N'4d876a3e-b59e-49f3-87f6-c28578f04fc9', N'6.00/16', N'ce45a90f-8b43-4268-a255-0f29bb632870', N'3 wheeler + maker', N'', CAST(0x0000A9F100BE1040 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'd9067db8-7a7c-497c-bf06-b6b75d78d4e7', N'TYcor3x1TUL-5494', N'Resole', N'Tubeless', N'Normal', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'2313fb42-44bf-4417-a642-88141fddcc1e', N'3x1', N'45324e81-f80e-4b8e-817f-d06c8e0587d9', N'3 wheeler + ggcghvhv', N'', CAST(0x0000A9CC00E32470 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Item Tyre] ([Token number], [Item Id], [Tyre make], [Tyre type], [Tyre feel], [Company token], [Company name], [Tyre token], [Tyre size], [Vehicle token], [Vehicle type], [Description], [Date], [Mac id], [User Id], [User name]) VALUES (N'f98d545e-d78e-416e-977e-35a7c3eb9867', N'TYcor145/80r12 amazer 4g lifeTUL-4358', N'New', N'Tubeless', N'Radial', N'ab014f7e-42a0-4b38-9fe3-492bae068c30', N'corsa', N'061f2217-a811-4b5e-97e0-7617287a51cd', N'145/80r12 amazer 4g life', N'ce45a90f-8b43-4268-a255-0f29bb632870', N'3 wheeler + maker', N'', CAST(0x0000AAF500A976D0 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Marchent Account] ([Token number], [Marchent name], [Email Id], [Address], [Mobile], [Telephone No], [GSTIN Number], [CIN Number], [UIN Number], [State Code], [Pan Number], [IsActive], [Verification code], [State Name], [License]) VALUES (N'c67b9ea6-af66-4569-83fa-7bba7d03f076', N'Elephanttree', N'Elephanttree@gmail.com', N'Bangalore', N'8334895299', NULL, N'22987564', NULL, NULL, 19, N'PAN000001', 1, N'g7RoPx,YQW', N'Karnataka', N'KADAMBARI-LC-A0B3CC7724D8-SOURAV-PC')
INSERT [dbo].[Marchent Account] ([Token number], [Marchent name], [Email Id], [Address], [Mobile], [Telephone No], [GSTIN Number], [CIN Number], [UIN Number], [State Code], [Pan Number], [IsActive], [Verification code], [State Name], [License]) VALUES (N'c67b9ea6-af66-4569-83fa-7bbu7d03f076', N'Kannantyres', N'kannantyres@kannantyres.com', N'Bangalore', N'8334895299', NULL, N'22987564', NULL, NULL, 19, N'PAN000001', 1, N'kannantyres12345', N'Karnataka', N'KADAMBARI-LC-A0B3CC7724D8-SOURAV-PC')
INSERT [dbo].[Marchent Account] ([Token number], [Marchent name], [Email Id], [Address], [Mobile], [Telephone No], [GSTIN Number], [CIN Number], [UIN Number], [State Code], [Pan Number], [IsActive], [Verification code], [State Name], [License]) VALUES (N'f', N'Indranil Ganguly', N'souravganguly707@gmail.com', N'Bangalore', N'8334895299', N'56565765655', N'GSTIN002', N'cin12345', N'UIN002', 19, N'PAN000002', 1, N'Sourav12345', N'Karnataka', N'KADAMBARI-LC-A0B3CC7724D8-SOURAV-PC')
INSERT [dbo].[Other expense] ([Token number], [Other expense type], [Other expense amount], [Date], [ExpenseId], [Mac id], [User Id], [User name]) VALUES (N'228deb09-baa7-4bfe-b834-48c2fabd3927', N'tyre', CAST(300.00 AS Decimal(18, 2)), CAST(0x0000AB6100000000 AS DateTime), N'EOTH0000001', N'A0B3CC7724D8', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Other Product] ([Token number], [Product name], [Product type], [Description], [Date], [Mac id], [Product id], [User Id], [User name]) VALUES (N'cbc653f1-f2cb-4443-a288-fed0e3a9a282', N'apollo tyre', N'Product', N'', CAST(0x0000AAFE011C4FA1 AS DateTime), N'B808CFCEAB3B', N'PO-1383', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Other Product] ([Token number], [Product name], [Product type], [Description], [Date], [Mac id], [Product id], [User Id], [User name]) VALUES (N'de730c58-a20c-450e-a682-72051fedf82f', N'demo', N'Services', N'demo', CAST(0x0000AB5000983ACB AS DateTime), N'B808CFCEAB3B', N'PO-9729', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Product expense] ([Token number], [Amount for expense], [Product token number], [Product name], [Date], [ExpenseId], [Mac id], [User Id], [User name]) VALUES (N'50ff0bbd-888a-45b4-b0a9-186664bb01b6', CAST(200.00 AS Decimal(18, 2)), N'cbc653f1-f2cb-4443-a288-fed0e3a9a282', N'apollo tyre', CAST(0x0000AB6100000000 AS DateTime), N'EPRO0000001', N'A0B3CC7724D8', N'KAN8927', N'Akshay CK')
INSERT [dbo].[Products] ([Token Number], [Product Code], [Product name], [Date], [Mac id], [User Id], [User name]) VALUES (N'ab014f7e-42a0-4b38-9fe3-492bae068c30', NULL, N'corsa', CAST(0x0000AA31017A7AA0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Products] ([Token Number], [Product Code], [Product name], [Date], [Mac id], [User Id], [User name]) VALUES (N'b3b60c2f-36df-4842-8907-4ca2a03bc854', NULL, N'mrf', CAST(0x0000AA6901099830 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Products] ([Token Number], [Product Code], [Product name], [Date], [Mac id], [User Id], [User name]) VALUES (N'bb27a8af-00f2-4c3f-a58b-7a1d28252c08', NULL, N'APOLLO', CAST(0x0000A9F100BDC9F0 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Products For Sale] ([Token Number], [Product Token], [Product name], [Tyre token], [Tyre Size], [Supplier token], [Supplier name], [Date], [Purchase Price], [CGST], [SGST], [Pieces], [Selling Price], [Amout after tax], [Total], [Approve date], [Approve], [Administrator Token number], [Administrator name], [Delivery contact number], [Delivery address], [Item tyre token], [Item tyre Id], [Purchase number], [Vehicle Token], [Vehicle type], [Description], [Tyre make], [Tyre feel], [Tyre type], [Mac id], [requestsend], [Selling CGST], [Selling SGST], [CGST-SGST-CHECK], [User Id], [User name], [Approve user id], [Approve user name], [Rate update user id], [Rate update user name], [IsGstPercent], [CalculationByRatePerUnit], [Selling net total]) VALUES (N'65c22089-4630-468d-bf8d-20ce6ef25c4d', NULL, N'demo', NULL, NULL, N'3296bca8-9969-4741-a877-6654f9dc49a6', N'SONA MOTORS', CAST(0x0000AB7400BC4AAF AS DateTime), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), 1, CAST(0.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(0x0000AB7400BC4AAF AS DateTime), 1, NULL, NULL, 8334895299, NULL, NULL, N'PO-9729', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, N'KAN8927', N'Akshay CK', NULL, NULL, NULL, NULL, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Products For Sale] ([Token Number], [Product Token], [Product name], [Tyre token], [Tyre Size], [Supplier token], [Supplier name], [Date], [Purchase Price], [CGST], [SGST], [Pieces], [Selling Price], [Amout after tax], [Total], [Approve date], [Approve], [Administrator Token number], [Administrator name], [Delivery contact number], [Delivery address], [Item tyre token], [Item tyre Id], [Purchase number], [Vehicle Token], [Vehicle type], [Description], [Tyre make], [Tyre feel], [Tyre type], [Mac id], [requestsend], [Selling CGST], [Selling SGST], [CGST-SGST-CHECK], [User Id], [User name], [Approve user id], [Approve user name], [Rate update user id], [Rate update user name], [IsGstPercent], [CalculationByRatePerUnit], [Selling net total]) VALUES (N'891917aa-a98a-43d8-81b3-918283064171', NULL, N'apollo tyre', NULL, NULL, N'3296bca8-9969-4741-a877-6654f9dc49a6', N'SONA MOTORS', CAST(0x0000AB7400B79325 AS DateTime), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), 1, CAST(0.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(0x0000AB7400B79325 AS DateTime), 1, NULL, NULL, 8334895299, NULL, NULL, N'PO-1383', NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, N'KAN8927', N'Akshay CK', NULL, NULL, NULL, NULL, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Products For Sale] ([Token Number], [Product Token], [Product name], [Tyre token], [Tyre Size], [Supplier token], [Supplier name], [Date], [Purchase Price], [CGST], [SGST], [Pieces], [Selling Price], [Amout after tax], [Total], [Approve date], [Approve], [Administrator Token number], [Administrator name], [Delivery contact number], [Delivery address], [Item tyre token], [Item tyre Id], [Purchase number], [Vehicle Token], [Vehicle type], [Description], [Tyre make], [Tyre feel], [Tyre type], [Mac id], [requestsend], [Selling CGST], [Selling SGST], [CGST-SGST-CHECK], [User Id], [User name], [Approve user id], [Approve user name], [Rate update user id], [Rate update user name], [IsGstPercent], [CalculationByRatePerUnit], [Selling net total]) VALUES (N'a169a925-89e7-4d8c-b2e1-1f2195fb8495', NULL, N'corsa', NULL, N'3x7', N'3296bca8-9969-4741-a877-6654f9dc49a6', N'SONA MOTORS', CAST(0x0000AB7400B7931E AS DateTime), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), 1, CAST(0.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(0x0000AB7400B7931E AS DateTime), 1, NULL, NULL, 8334895299, NULL, NULL, N'TYcor3x7TUB-7496', NULL, NULL, N'3 wheeler + maker', NULL, N'Resole', N'Normal', N'Tube', NULL, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, N'KAN8927', N'Akshay CK', NULL, NULL, NULL, NULL, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Products For Sale] ([Token Number], [Product Token], [Product name], [Tyre token], [Tyre Size], [Supplier token], [Supplier name], [Date], [Purchase Price], [CGST], [SGST], [Pieces], [Selling Price], [Amout after tax], [Total], [Approve date], [Approve], [Administrator Token number], [Administrator name], [Delivery contact number], [Delivery address], [Item tyre token], [Item tyre Id], [Purchase number], [Vehicle Token], [Vehicle type], [Description], [Tyre make], [Tyre feel], [Tyre type], [Mac id], [requestsend], [Selling CGST], [Selling SGST], [CGST-SGST-CHECK], [User Id], [User name], [Approve user id], [Approve user name], [Rate update user id], [Rate update user name], [IsGstPercent], [CalculationByRatePerUnit], [Selling net total]) VALUES (N'e35c08bc-3bee-4233-8f04-7d9494e6b9db', N'bb27a8af-00f2-4c3f-a58b-7a1d28252c08', N'APOLLO', N'4d876a3e-b59e-49f3-87f6-c28578f04fc9', N'6.00/16', N'7d570b4f-d0b5-4ba8-8a1b-f46fed7097bb', N'APOLLO TYRES LTD', CAST(0x0000AB1500000000 AS DateTime), CAST(100.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), 99, CAST(200.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), CAST(0x0000AB1500000000 AS DateTime), 1, NULL, NULL, 0, N'Ph. number : 9986716744 - Email : apollo@apollotyres.com - Pan number : AAACA6990Q - GST number : 29AAACA6990QIZU - Address : C124, INDUSTRIAL ESTATE, YADAVAGIRI MYSORE,570020', N'cab26fba-1b7f-45f5-b2e3-b06f8761195d', N'TYAPO6.00/16TUL-2855', N'inv001', NULL, N'3 wheeler + maker', NULL, N'New', N'Radial', N'Tubeless', N'B808CFCEAB3B', 0, CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), NULL, N'KAN8927', N'Akshay CK', N'KAN8927', N'Akshay CK', N'KAN8927', N'Akshay CK', 1, 1, CAST(220.00 AS Decimal(18, 2)))
INSERT [dbo].[Products For Sale] ([Token Number], [Product Token], [Product name], [Tyre token], [Tyre Size], [Supplier token], [Supplier name], [Date], [Purchase Price], [CGST], [SGST], [Pieces], [Selling Price], [Amout after tax], [Total], [Approve date], [Approve], [Administrator Token number], [Administrator name], [Delivery contact number], [Delivery address], [Item tyre token], [Item tyre Id], [Purchase number], [Vehicle Token], [Vehicle type], [Description], [Tyre make], [Tyre feel], [Tyre type], [Mac id], [requestsend], [Selling CGST], [Selling SGST], [CGST-SGST-CHECK], [User Id], [User name], [Approve user id], [Approve user name], [Rate update user id], [Rate update user name], [IsGstPercent], [CalculationByRatePerUnit], [Selling net total]) VALUES (N'ffc01987-eea8-4f83-b42d-98dd0503eacc', NULL, N'corsa', NULL, N'3x1', N'3296bca8-9969-4741-a877-6654f9dc49a6', N'SONA MOTORS', CAST(0x0000AB7400B79324 AS DateTime), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), 1, CAST(0.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(1.00 AS Decimal(18, 2)), CAST(0x0000AB7400B79324 AS DateTime), 1, NULL, NULL, 8334895299, NULL, NULL, N'TYcor3x1TUL-5494', NULL, NULL, N'3 wheeler + ggcghvhv', NULL, N'Resole', N'Normal', N'Tubeless', NULL, 0, CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), NULL, N'KAN8927', N'Akshay CK', NULL, NULL, NULL, NULL, 0, 0, CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[Purchase Invoice] ([Token number], [Purchase invoice number], [Date], [Stock entry token]) VALUES (N'8b6bb6b9-2c1f-433f-ac1f-7d37e69d7aff', N'inv001', CAST(0x00008EAC00000000 AS DateTime), NULL)
INSERT [dbo].[Rate update Backup] ([Token number], [Item number], [Date], [Date of stock entry], [Selling rate], [Selling CGST], [Selling SGST]) VALUES (N'49c3f7cd-113b-49f6-b4a1-b292fa7015b3', N'TYAPO6.00/16TUL-2855', CAST(0x0000AB1500000000 AS DateTime), CAST(0x0000AB1500000000 AS DateTime), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)))
INSERT [dbo].[State] ([State Code], [Name], [State Identity], [CGST], [SGST], [UTGST], [IGST], [Marchent Token number]) VALUES (19, N'Karnataka', N'KA', 1, 1, 0, 0, NULL)
INSERT [dbo].[State] ([State Code], [Name], [State Identity], [CGST], [SGST], [UTGST], [IGST], [Marchent Token number]) VALUES (29, N'West bengal', N'WB', 1, 1, 0, 0, NULL)
SET IDENTITY_INSERT [dbo].[Stock] ON 

INSERT [dbo].[Stock] ([Stock Id], [Purchase Token number], [Purcahse number], [Date], [Remodify Date], [Product Token], [Remodify pcs], [Pieces], [CGST], [SGST], [Product name], [Marchent Token number]) VALUES (1, NULL, NULL, CAST(0x0000AAFE00000000 AS DateTime), CAST(0x0000AAFE00000000 AS DateTime), N'ce166256-2d96-4cf1-90e7-932c06d029c7', 99, 10, CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), NULL, NULL)
INSERT [dbo].[Stock] ([Stock Id], [Purchase Token number], [Purcahse number], [Date], [Remodify Date], [Product Token], [Remodify pcs], [Pieces], [CGST], [SGST], [Product name], [Marchent Token number]) VALUES (2, NULL, NULL, CAST(0x0000AAFE00000000 AS DateTime), CAST(0x0000AAFE00000000 AS DateTime), N'8f1e50c1-9270-499b-9735-63df4e4edf4a', 99, 10, CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), NULL, NULL)
INSERT [dbo].[Stock] ([Stock Id], [Purchase Token number], [Purcahse number], [Date], [Remodify Date], [Product Token], [Remodify pcs], [Pieces], [CGST], [SGST], [Product name], [Marchent Token number]) VALUES (3, NULL, NULL, CAST(0x0000AB1500000000 AS DateTime), CAST(0x0000AB1500000000 AS DateTime), N'4cb03c56-b72e-4a0a-8501-8e841beea239', 99, 100, CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), NULL, NULL)
INSERT [dbo].[Stock] ([Stock Id], [Purchase Token number], [Purcahse number], [Date], [Remodify Date], [Product Token], [Remodify pcs], [Pieces], [CGST], [SGST], [Product name], [Marchent Token number]) VALUES (4, NULL, NULL, CAST(0x0000AB1500000000 AS DateTime), CAST(0x0000AB1500000000 AS DateTime), N'e35c08bc-3bee-4233-8f04-7d9494e6b9db', 99, 100, CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), NULL, NULL)
SET IDENTITY_INSERT [dbo].[Stock] OFF
INSERT [dbo].[Temp_Bill] ([Token Number], [Product Token], [Product name], [Tyre token], [Tyre Size], [Supplier token], [Supplier name], [Date], [Purchase Price], [CGST], [SGST], [Pieces], [Selling Price], [Amout after tax], [Total], [Approve date], [Approve], [Administrator Token number], [Administrator name], [Delivery contact number], [Delivery address], [Item tyre token], [Item tyre Id], [Purchase number], [Vehicle Token], [Vehicle type], [Description], [Tyre make], [Tyre feel], [Tyre type], [Mac id], [requestsend], [Selling CGST], [Selling SGST], [CGST-SGST-CHECK], [User Id], [User name], [Approve user id], [Approve user name], [Rate update user id], [Rate update user name], [Tyre number]) VALUES (N'6629a9f1-af2f-4a75-a49f-78397b15419a', N'bb27a8af-00f2-4c3f-a58b-7a1d28252c08', N'APOLLO', N'4d876a3e-b59e-49f3-87f6-c28578f04fc9', N'6.00/16', N'7d570b4f-d0b5-4ba8-8a1b-f46fed7097bb', N'APOLLO TYRES LTD', CAST(0x0000ABA600AFF4AB AS DateTime), CAST(100.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), 1, CAST(200.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), CAST(220.00 AS Decimal(18, 2)), CAST(0x0000AB1500000000 AS DateTime), 1, NULL, NULL, 0, N'Ph. number : 9986716744 - Email : apollo@apollotyres.com - Pan number : AAACA6990Q - GST number : 29AAACA6990QIZU - Address : C124, INDUSTRIAL ESTATE, YADAVAGIRI MYSORE,570020', N'cab26fba-1b7f-45f5-b2e3-b06f8761195d', N'TYAPO6.00/16TUL-2855', N'inv001', NULL, N'3 wheeler + maker', NULL, N'New', N'Radial', N'Tubeless', N'B808CFCEAB3B', 0, CAST(5.00 AS Decimal(18, 2)), CAST(5.00 AS Decimal(18, 2)), NULL, N'KAN8927', N'Akshay CK', N'KAN8927', N'Akshay CK', N'KAN8927', N'Akshay CK', NULL)
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'061f2217-a811-4b5e-97e0-7617287a51cd', N'145/80r12 amazer 4g life', 0, CAST(0x0000AAF500A93080 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'1044b5b5-23d3-409d-b33e-d69cac0b94c4', N'aaaaaty5fgfg', 0, CAST(0x0000AAF2017A7AA0 AS DateTime), N'A0B3CC7724D8', N'', NULL)
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'2246fb52-6e68-46e9-b958-6dee63af159a', N'6000/15', 0, CAST(0x0000AA6901099830 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'2313fb42-44bf-4417-a642-88141fddcc1e', N'3x1', 0, CAST(0x0000AA3B01709760 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'24350257-4085-42dd-967f-2fd2da8e1859', N'2x1', 0, CAST(0x0000AA3B0170DDB0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'3f44a4c5-dfe3-4c21-a57d-d3ce4c7b2148', N'wawasasasaaa', 0, CAST(0x0000AA3B017A7AA0 AS DateTime), N'A0B3CC7724D8', N'', NULL)
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'4ba40273-7073-41f3-8237-c5418e71b268', N'3x7', 0, CAST(0x0000AA31017A7AA0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'4d876a3e-b59e-49f3-87f6-c28578f04fc9', N'6.00/16', 0, CAST(0x0000A9F100BDC9F0 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Tyre size] ([Token number], [Tyre size], [With tube], [Date], [Mac id], [User Id], [User name]) VALUES (N'67fed3a6-5ad4-4dc8-a7a9-0c961cb7fb5b', N'4x1', 0, CAST(0x0000AA3B0170DDB0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Vehicle] ([Token number], [Vehicle type], [Vehicle make], [Date], [Mac id], [User Id], [User name]) VALUES (N'45324e81-f80e-4b8e-817f-d06c8e0587d9', N'3 wheeler', N'ggcghvhv', CAST(0x0000AA37017FB290 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
INSERT [dbo].[Vehicle] ([Token number], [Vehicle type], [Vehicle make], [Date], [Mac id], [User Id], [User name]) VALUES (N'78c7aa0e-c579-452a-b05b-f08f1a3a1da2', N'4 wheeler', N'jeep', CAST(0x0000AA690109DE80 AS DateTime), N'000C2904148E', N'kannantyres@kannantyres.com', NULL)
INSERT [dbo].[Vehicle] ([Token number], [Vehicle type], [Vehicle make], [Date], [Mac id], [User Id], [User name]) VALUES (N'ce45a90f-8b43-4268-a255-0f29bb632870', N'3 wheeler', N'maker', CAST(0x0000AA31017A7AA0 AS DateTime), N'A0B3CC7724D8', N'KAN7597', N'fghr44444')
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Barcode_Master_Billing_Master]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Barcode_Master_Billing_Master] ON [dbo].[Barcode Master]
(
	[Billing Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Billing_Details_Billing_Details]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Billing_Details_Billing_Details] ON [dbo].[Billing Details]
(
	[Billing Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Dealer_State]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Dealer_State] ON [dbo].[Dealer]
(
	[State] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Item_Tube_Products]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Item_Tube_Products] ON [dbo].[Item Tube]
(
	[Company token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Item_Tube_Tyre_size]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Item_Tube_Tyre_size] ON [dbo].[Item Tube]
(
	[Size token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Item_Tyre_Products]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Item_Tyre_Products] ON [dbo].[Item Tyre]
(
	[Company token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Item_Tyre_Tyre_size]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Item_Tyre_Tyre_size] ON [dbo].[Item Tyre]
(
	[Tyre token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Item_Tyre_Vehicle]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Item_Tyre_Vehicle] ON [dbo].[Item Tyre]
(
	[Vehicle token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_FK_Marchent_Account_State]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Marchent_Account_State] ON [dbo].[Marchent Account]
(
	[State Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Products_For_Sale_Dealer]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Products_For_Sale_Dealer] ON [dbo].[Products For Sale]
(
	[Supplier token] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Purchase_details_Purchase_Master]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Purchase_details_Purchase_Master] ON [dbo].[Purchase details]
(
	[Purchase Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Stock_Purchase_Master]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Stock_Purchase_Master] ON [dbo].[Stock]
(
	[Purchase Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_FK_Stockout_Billing_Master]    Script Date: 01-May-20 12:03:46 PM ******/
CREATE NONCLUSTERED INDEX [IX_FK_Stockout_Billing_Master] ON [dbo].[Stockout]
(
	[Billing Token number] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Barcode Master]  WITH CHECK ADD  CONSTRAINT [FK_Barcode_Master_Billing_Master] FOREIGN KEY([Billing Token number])
REFERENCES [dbo].[Billing Master] ([Token Number])
GO
ALTER TABLE [dbo].[Barcode Master] CHECK CONSTRAINT [FK_Barcode_Master_Billing_Master]
GO
ALTER TABLE [dbo].[Billing Details]  WITH CHECK ADD  CONSTRAINT [FK_Billing_Details_Billing_Details] FOREIGN KEY([Billing Token number])
REFERENCES [dbo].[Billing Master] ([Token Number])
GO
ALTER TABLE [dbo].[Billing Details] CHECK CONSTRAINT [FK_Billing_Details_Billing_Details]
GO
ALTER TABLE [dbo].[Dealer]  WITH CHECK ADD  CONSTRAINT [FK_Dealer_State] FOREIGN KEY([State])
REFERENCES [dbo].[State] ([State Code])
GO
ALTER TABLE [dbo].[Dealer] CHECK CONSTRAINT [FK_Dealer_State]
GO
ALTER TABLE [dbo].[Item Tube]  WITH CHECK ADD  CONSTRAINT [FK_Item_Tube_Products] FOREIGN KEY([Company token])
REFERENCES [dbo].[Products] ([Token Number])
GO
ALTER TABLE [dbo].[Item Tube] CHECK CONSTRAINT [FK_Item_Tube_Products]
GO
ALTER TABLE [dbo].[Item Tube]  WITH CHECK ADD  CONSTRAINT [FK_Item_Tube_Tyre_size] FOREIGN KEY([Size token])
REFERENCES [dbo].[Tyre size] ([Token number])
GO
ALTER TABLE [dbo].[Item Tube] CHECK CONSTRAINT [FK_Item_Tube_Tyre_size]
GO
ALTER TABLE [dbo].[Item Tyre]  WITH CHECK ADD  CONSTRAINT [FK_Item_Tyre_Products] FOREIGN KEY([Company token])
REFERENCES [dbo].[Products] ([Token Number])
GO
ALTER TABLE [dbo].[Item Tyre] CHECK CONSTRAINT [FK_Item_Tyre_Products]
GO
ALTER TABLE [dbo].[Item Tyre]  WITH CHECK ADD  CONSTRAINT [FK_Item_Tyre_Tyre_size] FOREIGN KEY([Tyre token])
REFERENCES [dbo].[Tyre size] ([Token number])
GO
ALTER TABLE [dbo].[Item Tyre] CHECK CONSTRAINT [FK_Item_Tyre_Tyre_size]
GO
ALTER TABLE [dbo].[Item Tyre]  WITH CHECK ADD  CONSTRAINT [FK_Item_Tyre_Vehicle] FOREIGN KEY([Vehicle token])
REFERENCES [dbo].[Vehicle] ([Token number])
GO
ALTER TABLE [dbo].[Item Tyre] CHECK CONSTRAINT [FK_Item_Tyre_Vehicle]
GO
ALTER TABLE [dbo].[Marchent Account]  WITH CHECK ADD  CONSTRAINT [FK_Marchent_Account_State] FOREIGN KEY([State Code])
REFERENCES [dbo].[State] ([State Code])
GO
ALTER TABLE [dbo].[Marchent Account] CHECK CONSTRAINT [FK_Marchent_Account_State]
GO
ALTER TABLE [dbo].[Products For Sale]  WITH CHECK ADD  CONSTRAINT [FK_Products_For_Sale_Dealer] FOREIGN KEY([Supplier token])
REFERENCES [dbo].[Dealer] ([Token number])
GO
ALTER TABLE [dbo].[Products For Sale] CHECK CONSTRAINT [FK_Products_For_Sale_Dealer]
GO
ALTER TABLE [dbo].[Purchase details]  WITH CHECK ADD  CONSTRAINT [FK_Purchase_details_Purchase_Master] FOREIGN KEY([Purchase Token number])
REFERENCES [dbo].[Purchase Master] ([Token Number])
GO
ALTER TABLE [dbo].[Purchase details] CHECK CONSTRAINT [FK_Purchase_details_Purchase_Master]
GO
ALTER TABLE [dbo].[Stock]  WITH CHECK ADD  CONSTRAINT [FK_Stock_Purchase_Master] FOREIGN KEY([Purchase Token number])
REFERENCES [dbo].[Purchase Master] ([Token Number])
GO
ALTER TABLE [dbo].[Stock] CHECK CONSTRAINT [FK_Stock_Purchase_Master]
GO
ALTER TABLE [dbo].[Stockout]  WITH CHECK ADD  CONSTRAINT [FK_Stockout_Billing_Master] FOREIGN KEY([Billing Token number])
REFERENCES [dbo].[Billing Master] ([Token Number])
GO
ALTER TABLE [dbo].[Stockout] CHECK CONSTRAINT [FK_Stockout_Billing_Master]
GO
USE [master]
GO
ALTER DATABASE [TestKannan] SET  READ_WRITE 
GO
