
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/07/2019 01:11:40
-- Generated from EDMX file: D:\mychecking\EasyBilling\EasyBilling\Models\EasyBillingModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [KannanTyres];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Barcode Master_Billing Master]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Barcode Master] DROP CONSTRAINT [FK_Barcode Master_Billing Master];
GO
IF OBJECT_ID(N'[dbo].[FK_Billing Details_Billing Details]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Billing Details] DROP CONSTRAINT [FK_Billing Details_Billing Details];
GO
IF OBJECT_ID(N'[dbo].[FK_Dealer_State]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Dealer] DROP CONSTRAINT [FK_Dealer_State];
GO
IF OBJECT_ID(N'[dbo].[FK_Item Tube_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item Tube] DROP CONSTRAINT [FK_Item Tube_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Item Tube_Tyre size]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item Tube] DROP CONSTRAINT [FK_Item Tube_Tyre size];
GO
IF OBJECT_ID(N'[dbo].[FK_Item Tyre_Products]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item Tyre] DROP CONSTRAINT [FK_Item Tyre_Products];
GO
IF OBJECT_ID(N'[dbo].[FK_Item Tyre_Tyre size]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item Tyre] DROP CONSTRAINT [FK_Item Tyre_Tyre size];
GO
IF OBJECT_ID(N'[dbo].[FK_Item Tyre_Vehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Item Tyre] DROP CONSTRAINT [FK_Item Tyre_Vehicle];
GO
IF OBJECT_ID(N'[dbo].[FK_Marchent Account_State]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Marchent Account] DROP CONSTRAINT [FK_Marchent Account_State];
GO
IF OBJECT_ID(N'[dbo].[FK_Products For Sale_Dealer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Products For Sale] DROP CONSTRAINT [FK_Products For Sale_Dealer];
GO
IF OBJECT_ID(N'[dbo].[FK_Purchase details_Purchase Master]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Purchase details] DROP CONSTRAINT [FK_Purchase details_Purchase Master];
GO
IF OBJECT_ID(N'[dbo].[FK_Stock_Purchase Master]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stock] DROP CONSTRAINT [FK_Stock_Purchase Master];
GO
IF OBJECT_ID(N'[dbo].[FK_Stockout_Billing Master]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Stockout] DROP CONSTRAINT [FK_Stockout_Billing Master];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Barcode Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Barcode Master];
GO
IF OBJECT_ID(N'[dbo].[Billing Details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Billing Details];
GO
IF OBJECT_ID(N'[dbo].[Billing Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Billing Master];
GO
IF OBJECT_ID(N'[dbo].[Customer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer];
GO
IF OBJECT_ID(N'[dbo].[Customer shipping address]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customer shipping address];
GO
IF OBJECT_ID(N'[dbo].[Dealer]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Dealer];
GO
IF OBJECT_ID(N'[dbo].[Employee]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee];
GO
IF OBJECT_ID(N'[dbo].[Employee salary expense]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee salary expense];
GO
IF OBJECT_ID(N'[dbo].[Item Tube]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Item Tube];
GO
IF OBJECT_ID(N'[dbo].[Item Tyre]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Item Tyre];
GO
IF OBJECT_ID(N'[dbo].[Manager]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Manager];
GO
IF OBJECT_ID(N'[dbo].[Marchent Account]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marchent Account];
GO
IF OBJECT_ID(N'[dbo].[Marchent account payment details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Marchent account payment details];
GO
IF OBJECT_ID(N'[dbo].[Order Details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Order Details];
GO
IF OBJECT_ID(N'[dbo].[Order Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Order Master];
GO
IF OBJECT_ID(N'[dbo].[Other expense]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Other expense];
GO
IF OBJECT_ID(N'[dbo].[Other Product]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Other Product];
GO
IF OBJECT_ID(N'[dbo].[Placed Order]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Placed Order];
GO
IF OBJECT_ID(N'[dbo].[Product expense]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Product expense];
GO
IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Products For Sale]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products For Sale];
GO
IF OBJECT_ID(N'[dbo].[Proprietor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Proprietor];
GO
IF OBJECT_ID(N'[dbo].[Purchase details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Purchase details];
GO
IF OBJECT_ID(N'[dbo].[Purchase Invoice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Purchase Invoice];
GO
IF OBJECT_ID(N'[dbo].[Purchase Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Purchase Master];
GO
IF OBJECT_ID(N'[dbo].[Quotation Details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Quotation Details];
GO
IF OBJECT_ID(N'[dbo].[Quotation Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Quotation Master];
GO
IF OBJECT_ID(N'[dbo].[Rate update Backup]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rate update Backup];
GO
IF OBJECT_ID(N'[dbo].[State]', 'U') IS NOT NULL
    DROP TABLE [dbo].[State];
GO
IF OBJECT_ID(N'[dbo].[Stock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stock];
GO
IF OBJECT_ID(N'[dbo].[StockForBillingRPT]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockForBillingRPT];
GO
IF OBJECT_ID(N'[dbo].[Stockout]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stockout];
GO
IF OBJECT_ID(N'[dbo].[Tax Group]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tax Group];
GO
IF OBJECT_ID(N'[dbo].[Temp_Bill]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Temp_Bill];
GO
IF OBJECT_ID(N'[dbo].[Temp_placedorder]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Temp_placedorder];
GO
IF OBJECT_ID(N'[dbo].[Temp_Stock]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Temp_Stock];
GO
IF OBJECT_ID(N'[dbo].[Transaction amount details]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Transaction amount details];
GO
IF OBJECT_ID(N'[dbo].[Tyre size]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tyre size];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO
IF OBJECT_ID(N'[dbo].[Vehicle]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vehicle];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Barcode_Masters'
CREATE TABLE [dbo].[Barcode_Masters] (
    [Barcode_Number] nvarchar(50)  NOT NULL,
    [Billing_Number] nvarchar(max)  NULL,
    [Billing_Token_number] nvarchar(250)  NULL,
    [Date] datetime  NOT NULL,
    [Marchent_Token_number] nvarchar(250)  NULL,
    [Image] varbinary(max)  NOT NULL
);
GO

-- Creating table 'Customer_shipping_addresses'
CREATE TABLE [dbo].[Customer_shipping_addresses] (
    [Customer_token_number] nvarchar(250)  NOT NULL,
    [First_name] nvarchar(250)  NULL,
    [Last_name] nvarchar(250)  NULL,
    [Address_Line1] nvarchar(250)  NULL,
    [Address_Line2] nvarchar(250)  NULL,
    [Town_City] nvarchar(250)  NULL,
    [State] nvarchar(50)  NULL,
    [Pin_code] nvarchar(50)  NULL,
    [Phone_number] nvarchar(50)  NULL,
    [Email] nvarchar(250)  NULL,
    [IsUser] bit  NOT NULL
);
GO

-- Creating table 'Dealers'
CREATE TABLE [dbo].[Dealers] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Dealer_code] nvarchar(50)  NULL,
    [Name] nvarchar(250)  NULL,
    [Email] nvarchar(250)  NULL,
    [Address] nvarchar(max)  NULL,
    [Phone_number] nvarchar(50)  NULL,
    [State] int  NULL,
    [State_Name] nvarchar(50)  NULL,
    [Isactive] bit  NOT NULL,
    [Marchent_Token_number] nvarchar(250)  NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [Office_number] nvarchar(50)  NULL,
    [GST_number] nvarchar(150)  NULL,
    [Pan_number] nvarchar(50)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Token_number] nvarchar(50)  NOT NULL,
    [Employee_Id] nvarchar(50)  NOT NULL,
    [Employee_name] nvarchar(100)  NULL,
    [Designation] nvarchar(250)  NULL,
    [Joining_date] datetime  NOT NULL,
    [Contact_number] nvarchar(50)  NULL,
    [Email_id] nvarchar(250)  NULL,
    [Salary] decimal(18,2)  NOT NULL,
    [Leaving_date] datetime  NULL,
    [login_required] bit  NOT NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [Image_path] nvarchar(250)  NULL,
    [Password] nvarchar(50)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Item_Tubes'
CREATE TABLE [dbo].[Item_Tubes] (
    [Token_number] nvarchar(50)  NOT NULL,
    [Tube_size] nvarchar(50)  NULL,
    [Company_token] nvarchar(250)  NULL,
    [Company_name] nvarchar(50)  NULL,
    [Description] nvarchar(50)  NULL,
    [Item_Id] nvarchar(50)  NULL,
    [Size_token] nvarchar(50)  NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Item_Tyres'
CREATE TABLE [dbo].[Item_Tyres] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Item_Id] nvarchar(50)  NULL,
    [Tyre_make] nvarchar(50)  NULL,
    [Tyre_type] nvarchar(50)  NULL,
    [Tyre_feel] nvarchar(50)  NULL,
    [Company_token] nvarchar(250)  NULL,
    [Company_name] nvarchar(50)  NULL,
    [Tyre_token] nvarchar(50)  NULL,
    [Tyre_size] nvarchar(50)  NULL,
    [Vehicle_token] nvarchar(50)  NULL,
    [Vehicle_type] nvarchar(50)  NULL,
    [Description] nvarchar(250)  NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Managers'
CREATE TABLE [dbo].[Managers] (
    [Token_number] nvarchar(50)  NOT NULL,
    [Manager_name] nvarchar(max)  NULL,
    [Email_Id] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Mobile] nvarchar(50)  NULL,
    [State_Code] int  NULL,
    [Pan_Number] nvarchar(50)  NULL,
    [IsActive] bit  NOT NULL,
    [Verification_code] nvarchar(max)  NULL,
    [State_Name] nvarchar(50)  NULL
);
GO

-- Creating table 'Marchent_Accounts'
CREATE TABLE [dbo].[Marchent_Accounts] (
    [Token_number] nvarchar(50)  NOT NULL,
    [Marchent_name] nvarchar(max)  NULL,
    [Email_Id] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Mobile] nvarchar(50)  NULL,
    [Telephone_No] nvarchar(50)  NULL,
    [GSTIN_Number] nvarchar(max)  NULL,
    [CIN_Number] nvarchar(max)  NULL,
    [UIN_Number] nvarchar(max)  NULL,
    [State_Code] int  NULL,
    [Pan_Number] nvarchar(50)  NULL,
    [IsActive] bit  NOT NULL,
    [Verification_code] nvarchar(max)  NULL,
    [State_Name] nvarchar(50)  NULL,
    [License] nvarchar(max)  NULL
);
GO

-- Creating table 'Marchent_account_payment_details'
CREATE TABLE [dbo].[Marchent_account_payment_details] (
    [GLCODE] nvarchar(250)  NOT NULL,
    [Account_name] nvarchar(max)  NULL,
    [Card_number] nvarchar(50)  NULL,
    [Marchent_token] nvarchar(250)  NULL
);
GO

-- Creating table 'Other_Products'
CREATE TABLE [dbo].[Other_Products] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Product_name] nvarchar(250)  NULL,
    [Product_type] nvarchar(250)  NULL,
    [Description] nvarchar(300)  NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [Product_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Placed_Orders'
CREATE TABLE [dbo].[Placed_Orders] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Item_token] nvarchar(250)  NULL,
    [Pieces] int  NOT NULL,
    [Orderplaced] bit  NOT NULL,
    [Customer_token] nvarchar(250)  NULL,
    [IsUser] bit  NOT NULL,
    [Order_Date] datetime  NOT NULL,
    [Ispaid] bit  NOT NULL,
    [Approve_Date] datetime  NOT NULL
);
GO

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Product_Code] nvarchar(50)  NULL,
    [Product_name] nvarchar(250)  NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Proprietors'
CREATE TABLE [dbo].[Proprietors] (
    [Token_number] nvarchar(50)  NOT NULL,
    [Proprietor_name] nvarchar(max)  NULL,
    [Email_Id] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Mobile] nvarchar(50)  NULL,
    [State_Code] int  NULL,
    [Pan_Number] nvarchar(50)  NULL,
    [IsActive] bit  NOT NULL,
    [Verification_code] nvarchar(max)  NULL,
    [State_Name] nvarchar(50)  NULL
);
GO

-- Creating table 'Purchase_details'
CREATE TABLE [dbo].[Purchase_details] (
    [Purchase_details_number] int IDENTITY(1,1) NOT NULL,
    [Purchase_Token_number] nvarchar(250)  NULL,
    [Purcahse_number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Product_Token] nvarchar(250)  NULL,
    [Product_name] nvarchar(250)  NULL,
    [Pieces] int  NOT NULL,
    [Quantity] decimal(18,3)  NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [Taxable_amount] decimal(18,2)  NOT NULL,
    [Tax_Token] nvarchar(250)  NULL,
    [Tax] decimal(18,2)  NOT NULL,
    [Discount_percent] decimal(18,2)  NOT NULL,
    [Discount] decimal(18,2)  NOT NULL,
    [Sub_Total] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'Purchase_Invoices'
CREATE TABLE [dbo].[Purchase_Invoices] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Purchase_invoice_number] nvarchar(250)  NULL,
    [Date] datetime  NOT NULL,
    [Stock_entry_token] nvarchar(250)  NULL
);
GO

-- Creating table 'Purchase_Masters'
CREATE TABLE [dbo].[Purchase_Masters] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Purchase_Number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Marchent_Token_number] nvarchar(250)  NULL,
    [Dealer_Token_number] nvarchar(250)  NULL,
    [Tax_token] nvarchar(250)  NULL,
    [Total_tax] decimal(18,2)  NOT NULL,
    [Rate_including_tax] decimal(18,2)  NOT NULL,
    [Discount_percent] decimal(18,2)  NOT NULL,
    [Total_discount] decimal(18,2)  NOT NULL,
    [Total_amount] decimal(18,2)  NOT NULL,
    [CGST] decimal(18,2)  NOT NULL,
    [SGST] decimal(18,2)  NOT NULL,
    [IGST] decimal(18,2)  NOT NULL,
    [UTGST] decimal(18,2)  NOT NULL,
    [Narration] nvarchar(max)  NULL
);
GO

-- Creating table 'States'
CREATE TABLE [dbo].[States] (
    [State_Code] int  NOT NULL,
    [Name] nvarchar(250)  NULL,
    [State_Identity] nvarchar(50)  NULL,
    [CGST] bit  NOT NULL,
    [SGST] bit  NOT NULL,
    [UTGST] bit  NOT NULL,
    [IGST] bit  NOT NULL,
    [Marchent_Token_number] nvarchar(250)  NULL
);
GO

-- Creating table 'Stocks'
CREATE TABLE [dbo].[Stocks] (
    [Stock_Id] int IDENTITY(1,1) NOT NULL,
    [Purchase_Token_number] nvarchar(250)  NULL,
    [Purcahse_number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Remodify_Date] datetime  NOT NULL,
    [Product_Token] nvarchar(250)  NULL,
    [Remodify_pcs] int  NOT NULL,
    [Pieces] int  NOT NULL,
    [CGST] decimal(18,2)  NOT NULL,
    [SGST] decimal(18,2)  NOT NULL,
    [Product_name] nvarchar(250)  NULL,
    [Marchent_Token_number] nvarchar(250)  NULL
);
GO

-- Creating table 'Stockouts'
CREATE TABLE [dbo].[Stockouts] (
    [Stock_out_Id] int IDENTITY(1,1) NOT NULL,
    [Billing_Token_number] nvarchar(250)  NULL,
    [Billing_number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Product_Token] nvarchar(250)  NULL,
    [Pieces] int  NOT NULL,
    [CGST] decimal(18,2)  NOT NULL,
    [SGST] decimal(18,2)  NOT NULL,
    [Sub_Total] decimal(18,2)  NOT NULL,
    [Marchent_Token_number] nvarchar(250)  NULL
);
GO

-- Creating table 'Tax_Groups'
CREATE TABLE [dbo].[Tax_Groups] (
    [Tax_Token] nvarchar(250)  NOT NULL,
    [Tax_Name] nvarchar(250)  NULL,
    [Tax_Rate] float  NOT NULL,
    [GL_CODE] nvarchar(250)  NULL,
    [IsActive] bit  NOT NULL,
    [Marchent_Token_number] nvarchar(250)  NULL
);
GO

-- Creating table 'Temp_placedorder'
CREATE TABLE [dbo].[Temp_placedorder] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Item_token] nvarchar(250)  NULL,
    [Pieces] int  NULL,
    [Customer_token] nvarchar(250)  NULL,
    [IsUser] bit  NOT NULL,
    [Order_Date] datetime  NOT NULL
);
GO

-- Creating table 'Transaction_amount_details'
CREATE TABLE [dbo].[Transaction_amount_details] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Marchent_Token] nvarchar(250)  NULL,
    [Purchase_Token] nvarchar(250)  NULL,
    [Purchase_number] nvarchar(max)  NULL,
    [Sale_invoice_token] nvarchar(250)  NULL,
    [Sale_invoice_number] nvarchar(max)  NULL,
    [GL_CODE] nvarchar(250)  NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [Card_number] nvarchar(50)  NULL,
    [Cheque_number] nvarchar(50)  NULL
);
GO

-- Creating table 'Tyre_sizes'
CREATE TABLE [dbo].[Tyre_sizes] (
    [Token_number] nvarchar(50)  NOT NULL,
    [Tyre_size1] nvarchar(50)  NULL,
    [With_tube] bit  NOT NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Name] nvarchar(250)  NULL,
    [Email] nvarchar(250)  NULL,
    [Password] nvarchar(250)  NULL,
    [Phone_number] nvarchar(50)  NULL,
    [Alternate_phone_number] nvarchar(50)  NULL,
    [State] int  NULL,
    [State_Name] nvarchar(50)  NULL,
    [Address] nvarchar(max)  NULL,
    [Second_Address] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [Marchent_Token_number] nvarchar(250)  NULL,
    [Applied_date] datetime  NOT NULL
);
GO

-- Creating table 'Vehicles'
CREATE TABLE [dbo].[Vehicles] (
    [Token_number] nvarchar(50)  NOT NULL,
    [Vehicle_type] nvarchar(250)  NULL,
    [Vehicle_make] nvarchar(250)  NULL,
    [Date] datetime  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Billing_Details'
CREATE TABLE [dbo].[Billing_Details] (
    [Billing_details_number] int IDENTITY(1,1) NOT NULL,
    [Billing_Token_number] nvarchar(250)  NULL,
    [Billing_number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Selling_item_token] nvarchar(250)  NULL,
    [Pieces] int  NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [Tax] decimal(18,2)  NOT NULL,
    [Discount] decimal(18,2)  NOT NULL,
    [Sub_Total] decimal(18,2)  NOT NULL,
    [Selling_item_id] nvarchar(250)  NULL,
    [IsGstPercent] bit  NULL
);
GO

-- Creating table 'Billing_Masters'
CREATE TABLE [dbo].[Billing_Masters] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Billing_Number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Total_tax] decimal(18,2)  NOT NULL,
    [Rate_including_tax] decimal(18,2)  NOT NULL,
    [Total_discount] decimal(18,2)  NOT NULL,
    [Total_amount] decimal(18,2)  NOT NULL,
    [Discountper] bit  NOT NULL,
    [Narration] nvarchar(max)  NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL,
    [Amount_paid] decimal(18,2)  NOT NULL,
    [Balance] decimal(18,2)  NOT NULL,
    [Mode_of_payment] nvarchar(50)  NULL,
    [Transaction_Id] nvarchar(500)  NULL,
    [Customer_token_number] nvarchar(250)  NULL,
    [Discount] decimal(18,2)  NULL,
    [IsGstPercent] bit  NULL
);
GO

-- Creating table 'Temp_Bill'
CREATE TABLE [dbo].[Temp_Bill] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Product_Token] nvarchar(250)  NULL,
    [Product_name] nvarchar(250)  NULL,
    [Tyre_token] nvarchar(50)  NULL,
    [Tyre_Size] nvarchar(250)  NULL,
    [Supplier_token] nvarchar(250)  NULL,
    [Supplier_name] nvarchar(250)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Purchase_Price] decimal(18,2)  NOT NULL,
    [CGST] decimal(18,2)  NOT NULL,
    [SGST] decimal(18,2)  NOT NULL,
    [Pieces] int  NOT NULL,
    [Selling_Price] decimal(18,2)  NOT NULL,
    [Amout_after_tax] decimal(18,2)  NOT NULL,
    [Total] decimal(18,2)  NOT NULL,
    [Approve_date] datetime  NOT NULL,
    [Approve] bit  NOT NULL,
    [Administrator_Token_number] nvarchar(250)  NULL,
    [Administrator_name] nvarchar(250)  NULL,
    [Delivery_contact_number] bigint  NOT NULL,
    [Delivery_address] nvarchar(300)  NULL,
    [Item_tyre_token] nvarchar(250)  NULL,
    [Item_tyre_Id] nvarchar(250)  NULL,
    [Purchase_number] nvarchar(250)  NULL,
    [Vehicle_Token] nvarchar(50)  NULL,
    [Vehicle_type] nvarchar(50)  NULL,
    [Description] nvarchar(300)  NULL,
    [Tyre_make] nvarchar(50)  NULL,
    [Tyre_feel] nvarchar(50)  NULL,
    [Tyre_type] nvarchar(50)  NULL,
    [Mac_id] nvarchar(250)  NULL,
    [requestsend] bit  NULL,
    [Selling_CGST] decimal(18,2)  NOT NULL,
    [Selling_SGST] decimal(18,2)  NOT NULL,
    [CGST_SGST_CHECK] bit  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL,
    [Approve_user_id] nvarchar(50)  NULL,
    [Approve_user_name] nvarchar(100)  NULL,
    [Rate_update_user_id] nvarchar(50)  NULL,
    [Rate_update_user_name] nvarchar(100)  NULL,
    [Tyre_number] nvarchar(50)  NULL
);
GO

-- Creating table 'StockForBillingRPTs'
CREATE TABLE [dbo].[StockForBillingRPTs] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Product_name] nvarchar(250)  NULL,
    [Tyre_Size] nvarchar(250)  NULL,
    [Selling_Price] decimal(18,2)  NOT NULL,
    [Item_tyre_Id] nvarchar(250)  NULL,
    [Selling_CGST] decimal(18,2)  NOT NULL,
    [Selling_SGST] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'Order_Details'
CREATE TABLE [dbo].[Order_Details] (
    [Order_details_number] int  NOT NULL,
    [Order_Token_number] nvarchar(250)  NULL,
    [Order_number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Selling_item_token] nvarchar(250)  NULL,
    [Pieces] int  NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [Tax] decimal(18,2)  NOT NULL,
    [Discount] decimal(18,2)  NOT NULL,
    [Sub_Total] decimal(18,2)  NOT NULL,
    [Selling_item_id] nvarchar(250)  NULL,
    [IsGstPercent] bit  NULL,
    [Tyre_number] nvarchar(50)  NULL
);
GO

-- Creating table 'Order_Masters'
CREATE TABLE [dbo].[Order_Masters] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Order_Number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Total_tax] decimal(18,2)  NOT NULL,
    [Rate_including_tax] decimal(18,2)  NOT NULL,
    [Discount] decimal(18,2)  NULL,
    [Total_discount] decimal(18,2)  NOT NULL,
    [Total_amount] decimal(18,2)  NOT NULL,
    [Discountper] bit  NOT NULL,
    [Narration] nvarchar(max)  NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL,
    [Amount_paid] decimal(18,2)  NOT NULL,
    [Balance] decimal(18,2)  NOT NULL,
    [Mode_of_payment] nvarchar(50)  NULL,
    [Transaction_Id] nvarchar(500)  NULL,
    [Customer_token_number] nvarchar(250)  NULL,
    [IsGstPercent] bit  NULL
);
GO

-- Creating table 'Quotation_Details'
CREATE TABLE [dbo].[Quotation_Details] (
    [Quotation_details_number] int  NOT NULL,
    [Quotation_Token_number] nvarchar(250)  NULL,
    [Quotation_number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Selling_item_token] nvarchar(250)  NULL,
    [Pieces] int  NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [Tax] decimal(18,2)  NOT NULL,
    [Discount] decimal(18,2)  NOT NULL,
    [Sub_Total] decimal(18,2)  NOT NULL,
    [Selling_item_id] nvarchar(250)  NULL,
    [IsGstPercent] bit  NULL
);
GO

-- Creating table 'Quotation_Masters'
CREATE TABLE [dbo].[Quotation_Masters] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Quotation_Number] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Total_tax] decimal(18,2)  NOT NULL,
    [Rate_including_tax] decimal(18,2)  NOT NULL,
    [Discount] decimal(18,2)  NULL,
    [Total_discount] decimal(18,2)  NOT NULL,
    [Total_amount] decimal(18,2)  NOT NULL,
    [Discountper] bit  NOT NULL,
    [Narration] nvarchar(max)  NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL,
    [Transaction_Id] nvarchar(500)  NULL,
    [Customer_token_number] nvarchar(250)  NULL,
    [IsGstPercent] bit  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Customer_Name] nvarchar(250)  NULL,
    [Email] nvarchar(250)  NULL,
    [Phone_number] nvarchar(50)  NULL,
    [Address] nvarchar(max)  NULL,
    [Vehicle_type] nvarchar(250)  NULL,
    [Vehicle_number] nvarchar(50)  NOT NULL,
    [Mac_id] nvarchar(250)  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL
);
GO

-- Creating table 'Rate_update_Backups'
CREATE TABLE [dbo].[Rate_update_Backups] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Item_number] nvarchar(250)  NULL,
    [Date] datetime  NULL,
    [Date_of_stock_entry] datetime  NULL,
    [Selling_rate] decimal(18,2)  NULL,
    [Selling_CGST] decimal(18,2)  NULL,
    [Selling_SGST] decimal(18,2)  NULL
);
GO

-- Creating table 'Employee_salary_expenses'
CREATE TABLE [dbo].[Employee_salary_expenses] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Advance_collected] decimal(18,2)  NOT NULL,
    [salary_to_be_paid] decimal(18,2)  NOT NULL,
    [Employee_token_number] nvarchar(250)  NULL,
    [Employee_name] nvarchar(250)  NULL
);
GO

-- Creating table 'Other_expenses'
CREATE TABLE [dbo].[Other_expenses] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Other_expense_type] nvarchar(250)  NOT NULL,
    [Other_expense_amount] decimal(18,2)  NOT NULL
);
GO

-- Creating table 'Product_expenses'
CREATE TABLE [dbo].[Product_expenses] (
    [Token_number] nvarchar(250)  NOT NULL,
    [Amount_for_expense] decimal(18,2)  NOT NULL,
    [Product_token_number] nvarchar(250)  NULL,
    [Product_name] nvarchar(250)  NULL
);
GO

-- Creating table 'Products_For_Sales'
CREATE TABLE [dbo].[Products_For_Sales] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Product_Token] nvarchar(250)  NULL,
    [Product_name] nvarchar(250)  NULL,
    [Tyre_token] nvarchar(50)  NULL,
    [Tyre_Size] nvarchar(250)  NULL,
    [Supplier_token] nvarchar(250)  NULL,
    [Supplier_name] nvarchar(250)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Purchase_Price] decimal(18,2)  NOT NULL,
    [CGST] decimal(18,2)  NOT NULL,
    [SGST] decimal(18,2)  NOT NULL,
    [Pieces] int  NOT NULL,
    [Selling_Price] decimal(18,2)  NOT NULL,
    [Amout_after_tax] decimal(18,2)  NOT NULL,
    [Total] decimal(18,2)  NOT NULL,
    [Approve_date] datetime  NOT NULL,
    [Approve] bit  NOT NULL,
    [Administrator_Token_number] nvarchar(250)  NULL,
    [Administrator_name] nvarchar(250)  NULL,
    [Delivery_contact_number] bigint  NOT NULL,
    [Delivery_address] nvarchar(300)  NULL,
    [Item_tyre_token] nvarchar(250)  NULL,
    [Item_tyre_Id] nvarchar(250)  NULL,
    [Purchase_number] nvarchar(250)  NULL,
    [Vehicle_Token] nvarchar(50)  NULL,
    [Vehicle_type] nvarchar(50)  NULL,
    [Description] nvarchar(300)  NULL,
    [Tyre_make] nvarchar(50)  NULL,
    [Tyre_feel] nvarchar(50)  NULL,
    [Tyre_type] nvarchar(50)  NULL,
    [Mac_id] nvarchar(250)  NULL,
    [requestsend] bit  NULL,
    [Selling_CGST] decimal(18,2)  NOT NULL,
    [Selling_SGST] decimal(18,2)  NOT NULL,
    [CGST_SGST_CHECK] bit  NULL,
    [User_Id] nvarchar(50)  NULL,
    [User_name] nvarchar(100)  NULL,
    [Approve_user_id] nvarchar(50)  NULL,
    [Approve_user_name] nvarchar(100)  NULL,
    [Rate_update_user_id] nvarchar(50)  NULL,
    [Rate_update_user_name] nvarchar(100)  NULL,
    [IsGstPercent] bit  NOT NULL,
    [CalculationByRatePerUnit] bit  NOT NULL,
    [Selling_net_total] decimal(18,2)  NULL
);
GO

-- Creating table 'Temp_Stock'
CREATE TABLE [dbo].[Temp_Stock] (
    [Token_Number] nvarchar(250)  NOT NULL,
    [Product_Token] nvarchar(250)  NULL,
    [Product_name] nvarchar(250)  NULL,
    [Tyre_token] nvarchar(50)  NULL,
    [Tyre_Size] nvarchar(250)  NULL,
    [Supplier_token] nvarchar(250)  NULL,
    [Supplier_name] nvarchar(250)  NOT NULL,
    [Date] datetime  NOT NULL,
    [Purchase_Price] decimal(18,2)  NOT NULL,
    [CGST] decimal(18,2)  NOT NULL,
    [SGST] decimal(18,2)  NOT NULL,
    [Pieces] int  NOT NULL,
    [Selling_Price] decimal(18,2)  NOT NULL,
    [Amout_after_tax] decimal(18,2)  NOT NULL,
    [Total] decimal(18,2)  NOT NULL,
    [Approve_date] datetime  NOT NULL,
    [Approve] bit  NOT NULL,
    [Administrator_Token_number] nvarchar(250)  NULL,
    [Administrator_name] nvarchar(250)  NULL,
    [Delivery_contact_number] bigint  NOT NULL,
    [Delivery_address] nvarchar(300)  NULL,
    [Item_tyre_token] nvarchar(250)  NULL,
    [Item_tyre_Id] nvarchar(250)  NULL,
    [Purchase_number] nvarchar(250)  NULL,
    [Vehicle_Token] nvarchar(50)  NULL,
    [Vehicle_type] nvarchar(50)  NULL,
    [Description] nvarchar(300)  NULL,
    [Tyre_make] nvarchar(50)  NULL,
    [Tyre_feel] nvarchar(50)  NULL,
    [Tyre_type] nvarchar(50)  NULL,
    [Mac_id] nvarchar(250)  NULL,
    [requestsend] bit  NULL,
    [IsGstPercent] bit  NOT NULL,
    [CalculationByRatePerUnit] bit  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Barcode_Number] in table 'Barcode_Masters'
ALTER TABLE [dbo].[Barcode_Masters]
ADD CONSTRAINT [PK_Barcode_Masters]
    PRIMARY KEY CLUSTERED ([Barcode_Number] ASC);
GO

-- Creating primary key on [Customer_token_number] in table 'Customer_shipping_addresses'
ALTER TABLE [dbo].[Customer_shipping_addresses]
ADD CONSTRAINT [PK_Customer_shipping_addresses]
    PRIMARY KEY CLUSTERED ([Customer_token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Dealers'
ALTER TABLE [dbo].[Dealers]
ADD CONSTRAINT [PK_Dealers]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Item_Tubes'
ALTER TABLE [dbo].[Item_Tubes]
ADD CONSTRAINT [PK_Item_Tubes]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Item_Tyres'
ALTER TABLE [dbo].[Item_Tyres]
ADD CONSTRAINT [PK_Item_Tyres]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Managers'
ALTER TABLE [dbo].[Managers]
ADD CONSTRAINT [PK_Managers]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Marchent_Accounts'
ALTER TABLE [dbo].[Marchent_Accounts]
ADD CONSTRAINT [PK_Marchent_Accounts]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [GLCODE] in table 'Marchent_account_payment_details'
ALTER TABLE [dbo].[Marchent_account_payment_details]
ADD CONSTRAINT [PK_Marchent_account_payment_details]
    PRIMARY KEY CLUSTERED ([GLCODE] ASC);
GO

-- Creating primary key on [Token_number] in table 'Other_Products'
ALTER TABLE [dbo].[Other_Products]
ADD CONSTRAINT [PK_Other_Products]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Placed_Orders'
ALTER TABLE [dbo].[Placed_Orders]
ADD CONSTRAINT [PK_Placed_Orders]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Proprietors'
ALTER TABLE [dbo].[Proprietors]
ADD CONSTRAINT [PK_Proprietors]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Purchase_details_number] in table 'Purchase_details'
ALTER TABLE [dbo].[Purchase_details]
ADD CONSTRAINT [PK_Purchase_details]
    PRIMARY KEY CLUSTERED ([Purchase_details_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Purchase_Invoices'
ALTER TABLE [dbo].[Purchase_Invoices]
ADD CONSTRAINT [PK_Purchase_Invoices]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Purchase_Masters'
ALTER TABLE [dbo].[Purchase_Masters]
ADD CONSTRAINT [PK_Purchase_Masters]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [State_Code] in table 'States'
ALTER TABLE [dbo].[States]
ADD CONSTRAINT [PK_States]
    PRIMARY KEY CLUSTERED ([State_Code] ASC);
GO

-- Creating primary key on [Stock_Id] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [PK_Stocks]
    PRIMARY KEY CLUSTERED ([Stock_Id] ASC);
GO

-- Creating primary key on [Stock_out_Id] in table 'Stockouts'
ALTER TABLE [dbo].[Stockouts]
ADD CONSTRAINT [PK_Stockouts]
    PRIMARY KEY CLUSTERED ([Stock_out_Id] ASC);
GO

-- Creating primary key on [Tax_Token] in table 'Tax_Groups'
ALTER TABLE [dbo].[Tax_Groups]
ADD CONSTRAINT [PK_Tax_Groups]
    PRIMARY KEY CLUSTERED ([Tax_Token] ASC);
GO

-- Creating primary key on [Token_number] in table 'Temp_placedorder'
ALTER TABLE [dbo].[Temp_placedorder]
ADD CONSTRAINT [PK_Temp_placedorder]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Transaction_amount_details'
ALTER TABLE [dbo].[Transaction_amount_details]
ADD CONSTRAINT [PK_Transaction_amount_details]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Tyre_sizes'
ALTER TABLE [dbo].[Tyre_sizes]
ADD CONSTRAINT [PK_Tyre_sizes]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [PK_Vehicles]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Billing_details_number] in table 'Billing_Details'
ALTER TABLE [dbo].[Billing_Details]
ADD CONSTRAINT [PK_Billing_Details]
    PRIMARY KEY CLUSTERED ([Billing_details_number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Billing_Masters'
ALTER TABLE [dbo].[Billing_Masters]
ADD CONSTRAINT [PK_Billing_Masters]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Temp_Bill'
ALTER TABLE [dbo].[Temp_Bill]
ADD CONSTRAINT [PK_Temp_Bill]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'StockForBillingRPTs'
ALTER TABLE [dbo].[StockForBillingRPTs]
ADD CONSTRAINT [PK_StockForBillingRPTs]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Order_details_number] in table 'Order_Details'
ALTER TABLE [dbo].[Order_Details]
ADD CONSTRAINT [PK_Order_Details]
    PRIMARY KEY CLUSTERED ([Order_details_number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Order_Masters'
ALTER TABLE [dbo].[Order_Masters]
ADD CONSTRAINT [PK_Order_Masters]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Quotation_details_number] in table 'Quotation_Details'
ALTER TABLE [dbo].[Quotation_Details]
ADD CONSTRAINT [PK_Quotation_Details]
    PRIMARY KEY CLUSTERED ([Quotation_details_number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Quotation_Masters'
ALTER TABLE [dbo].[Quotation_Masters]
ADD CONSTRAINT [PK_Quotation_Masters]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Vehicle_number] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Vehicle_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Rate_update_Backups'
ALTER TABLE [dbo].[Rate_update_Backups]
ADD CONSTRAINT [PK_Rate_update_Backups]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Employee_salary_expenses'
ALTER TABLE [dbo].[Employee_salary_expenses]
ADD CONSTRAINT [PK_Employee_salary_expenses]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Other_expenses'
ALTER TABLE [dbo].[Other_expenses]
ADD CONSTRAINT [PK_Other_expenses]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_number] in table 'Product_expenses'
ALTER TABLE [dbo].[Product_expenses]
ADD CONSTRAINT [PK_Product_expenses]
    PRIMARY KEY CLUSTERED ([Token_number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Products_For_Sales'
ALTER TABLE [dbo].[Products_For_Sales]
ADD CONSTRAINT [PK_Products_For_Sales]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- Creating primary key on [Token_Number] in table 'Temp_Stock'
ALTER TABLE [dbo].[Temp_Stock]
ADD CONSTRAINT [PK_Temp_Stock]
    PRIMARY KEY CLUSTERED ([Token_Number] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [State] in table 'Dealers'
ALTER TABLE [dbo].[Dealers]
ADD CONSTRAINT [FK_Dealer_State]
    FOREIGN KEY ([State])
    REFERENCES [dbo].[States]
        ([State_Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Dealer_State'
CREATE INDEX [IX_FK_Dealer_State]
ON [dbo].[Dealers]
    ([State]);
GO

-- Creating foreign key on [Company_token] in table 'Item_Tubes'
ALTER TABLE [dbo].[Item_Tubes]
ADD CONSTRAINT [FK_Item_Tube_Products]
    FOREIGN KEY ([Company_token])
    REFERENCES [dbo].[Products]
        ([Token_Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_Tube_Products'
CREATE INDEX [IX_FK_Item_Tube_Products]
ON [dbo].[Item_Tubes]
    ([Company_token]);
GO

-- Creating foreign key on [Company_token] in table 'Item_Tyres'
ALTER TABLE [dbo].[Item_Tyres]
ADD CONSTRAINT [FK_Item_Tyre_Products]
    FOREIGN KEY ([Company_token])
    REFERENCES [dbo].[Products]
        ([Token_Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_Tyre_Products'
CREATE INDEX [IX_FK_Item_Tyre_Products]
ON [dbo].[Item_Tyres]
    ([Company_token]);
GO

-- Creating foreign key on [Tyre_token] in table 'Item_Tyres'
ALTER TABLE [dbo].[Item_Tyres]
ADD CONSTRAINT [FK_Item_Tyre_Tyre_size]
    FOREIGN KEY ([Tyre_token])
    REFERENCES [dbo].[Tyre_sizes]
        ([Token_number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_Tyre_Tyre_size'
CREATE INDEX [IX_FK_Item_Tyre_Tyre_size]
ON [dbo].[Item_Tyres]
    ([Tyre_token]);
GO

-- Creating foreign key on [Vehicle_token] in table 'Item_Tyres'
ALTER TABLE [dbo].[Item_Tyres]
ADD CONSTRAINT [FK_Item_Tyre_Vehicle]
    FOREIGN KEY ([Vehicle_token])
    REFERENCES [dbo].[Vehicles]
        ([Token_number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_Tyre_Vehicle'
CREATE INDEX [IX_FK_Item_Tyre_Vehicle]
ON [dbo].[Item_Tyres]
    ([Vehicle_token]);
GO

-- Creating foreign key on [State_Code] in table 'Marchent_Accounts'
ALTER TABLE [dbo].[Marchent_Accounts]
ADD CONSTRAINT [FK_Marchent_Account_State]
    FOREIGN KEY ([State_Code])
    REFERENCES [dbo].[States]
        ([State_Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Marchent_Account_State'
CREATE INDEX [IX_FK_Marchent_Account_State]
ON [dbo].[Marchent_Accounts]
    ([State_Code]);
GO

-- Creating foreign key on [Purchase_Token_number] in table 'Purchase_details'
ALTER TABLE [dbo].[Purchase_details]
ADD CONSTRAINT [FK_Purchase_details_Purchase_Master]
    FOREIGN KEY ([Purchase_Token_number])
    REFERENCES [dbo].[Purchase_Masters]
        ([Token_Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Purchase_details_Purchase_Master'
CREATE INDEX [IX_FK_Purchase_details_Purchase_Master]
ON [dbo].[Purchase_details]
    ([Purchase_Token_number]);
GO

-- Creating foreign key on [Purchase_Token_number] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [FK_Stock_Purchase_Master]
    FOREIGN KEY ([Purchase_Token_number])
    REFERENCES [dbo].[Purchase_Masters]
        ([Token_Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stock_Purchase_Master'
CREATE INDEX [IX_FK_Stock_Purchase_Master]
ON [dbo].[Stocks]
    ([Purchase_Token_number]);
GO

-- Creating foreign key on [Size_token] in table 'Item_Tubes'
ALTER TABLE [dbo].[Item_Tubes]
ADD CONSTRAINT [FK_Item_Tube_Tyre_size]
    FOREIGN KEY ([Size_token])
    REFERENCES [dbo].[Tyre_sizes]
        ([Token_number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Item_Tube_Tyre_size'
CREATE INDEX [IX_FK_Item_Tube_Tyre_size]
ON [dbo].[Item_Tubes]
    ([Size_token]);
GO

-- Creating foreign key on [Billing_Token_number] in table 'Barcode_Masters'
ALTER TABLE [dbo].[Barcode_Masters]
ADD CONSTRAINT [FK_Barcode_Master_Billing_Master]
    FOREIGN KEY ([Billing_Token_number])
    REFERENCES [dbo].[Billing_Masters]
        ([Token_Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Barcode_Master_Billing_Master'
CREATE INDEX [IX_FK_Barcode_Master_Billing_Master]
ON [dbo].[Barcode_Masters]
    ([Billing_Token_number]);
GO

-- Creating foreign key on [Billing_Token_number] in table 'Billing_Details'
ALTER TABLE [dbo].[Billing_Details]
ADD CONSTRAINT [FK_Billing_Details_Billing_Details]
    FOREIGN KEY ([Billing_Token_number])
    REFERENCES [dbo].[Billing_Masters]
        ([Token_Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Billing_Details_Billing_Details'
CREATE INDEX [IX_FK_Billing_Details_Billing_Details]
ON [dbo].[Billing_Details]
    ([Billing_Token_number]);
GO

-- Creating foreign key on [Billing_Token_number] in table 'Stockouts'
ALTER TABLE [dbo].[Stockouts]
ADD CONSTRAINT [FK_Stockout_Billing_Master]
    FOREIGN KEY ([Billing_Token_number])
    REFERENCES [dbo].[Billing_Masters]
        ([Token_Number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Stockout_Billing_Master'
CREATE INDEX [IX_FK_Stockout_Billing_Master]
ON [dbo].[Stockouts]
    ([Billing_Token_number]);
GO

-- Creating foreign key on [Supplier_token] in table 'Products_For_Sales'
ALTER TABLE [dbo].[Products_For_Sales]
ADD CONSTRAINT [FK_Products_For_Sale_Dealer]
    FOREIGN KEY ([Supplier_token])
    REFERENCES [dbo].[Dealers]
        ([Token_number])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Products_For_Sale_Dealer'
CREATE INDEX [IX_FK_Products_For_Sale_Dealer]
ON [dbo].[Products_For_Sales]
    ([Supplier_token]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------