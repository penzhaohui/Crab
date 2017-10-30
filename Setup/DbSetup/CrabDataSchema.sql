SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ExtensionValues]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ExtensionValues](
	[RecordId] [uniqueidentifier] NOT NULL,
	[FieldId] [uniqueidentifier] NOT NULL,
	[Value] [nvarchar](256) NULL,
 CONSTRAINT [PK_ExtensionValue] PRIMARY KEY CLUSTERED 
(
	[RecordId] ASC,
	[FieldId] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Customer]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Customer](
	[TenantId] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](250) NULL,
	[Address] [nvarchar](250) NULL,
	[PhoneNumber] [nvarchar](50) NULL,
	[Description] [nvarchar](250) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShippingExportContract]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ShippingExportContract](
	[TenantId] [uniqueidentifier] NOT NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[Creator] [uniqueidentifier] NULL,
	[Number] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[Shipper] [uniqueidentifier] NULL,
	[Consignee] [uniqueidentifier] NULL,
	[NotifyPart] [uniqueidentifier] NULL,
	[CreditId] [nvarchar](50) NULL,
	[ExportSite] [nvarchar](50) NULL,
	[Destination] [nvarchar](50) NULL,
	[Batch] [bit] NULL,
	[Reship] [bit] NULL,
	[PaymentMethod] [nvarchar](20) NULL,
	[CreateDate] [datetime] NULL CONSTRAINT [DF_ExportContract_CreateDate]  DEFAULT (getdate()),
	[WorkflowId] [uniqueidentifier] NULL,
	[Amount] [decimal](18, 2) NULL CONSTRAINT [DF_ExportContract_Amount]  DEFAULT ((0)),
	[ShippingMarks] [nvarchar](50) NULL,
	[ProductName] [nvarchar](50) NULL,
	[Quantity] [decimal](18, 2) NULL,
	[Gross] [decimal](18, 2) NULL,
	[Net] [decimal](18, 2) NULL,
	[Capacity] [decimal](18, 2) NULL,
 CONSTRAINT [PK_ExportContract] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ShippingExportContract', N'COLUMN',N'Batch'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Can be batched' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingExportContract', @level2type=N'COLUMN',@level2name=N'Batch'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ShippingExportContract', N'COLUMN',N'Reship'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Can be reshipped' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ShippingExportContract', @level2type=N'COLUMN',@level2name=N'Reship'
