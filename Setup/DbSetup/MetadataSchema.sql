SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WorkflowDefinition]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[WorkflowDefinition](
	[Id] [uniqueidentifier] NOT NULL,
	[TenantId] [uniqueidentifier] NULL,
	[WorkflowType] [int] NOT NULL,
	[Xoml] [ntext] NOT NULL,
	[Rules] [ntext] NULL,
 CONSTRAINT [PK_WorkflowDefinition] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataNodes]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataNodes](
	[TenantId] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[NodeType] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_DataNode] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DataProperties]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DataProperties](
	[TenantId] [uniqueidentifier] NULL,
	[Id] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NOT NULL,
	[PropertyType] [int] NOT NULL,
	[Value] [nvarchar](256) NULL,
 CONSTRAINT [PK_DataProperties] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crab_GetWorkflowDefinition]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'


CREATE PROCEDURE [dbo].[crab_GetWorkflowDefinition]
@TenantId    uniqueidentifier,
@WorkflowType int
AS
BEGIN
    IF NOT EXISTS (SELECT Id FROM WorkflowDefinition 
         WHERE TenantId=@TenantId AND WorkflowType=@WorkflowType)
        SELECT * FROM WorkflowDefinition WHERE TenantId IS NULL AND WorkflowType=@WorkflowType
    ELSE
        SELECT * FROM WorkflowDefinition WHERE TenantId =@TenantId AND WorkflowType=@WorkflowType

    RETURN @@ROWCOUNT
END

' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crab_UpdateWorkflowDefinition]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'

Create PROCEDURE [dbo].[crab_UpdateWorkflowDefinition]
@TenantId    uniqueidentifier,
@WorkflowType int,
@Xoml NText,
@Rules NText
AS
BEGIN
   IF NOT EXISTS (SELECT Id FROM WorkflowDefinition 
         WHERE TenantId=@TenantId AND WorkflowType=@WorkflowType)
      INSERT INTO WorkflowDefinition
        VALUES
        (NewId(),
         @TenantId,
         @WorkflowType,
         @Xoml,
         @Rules)
    ELSE
       UPDATE WorkflowDefinition
       SET Xoml = @Xoml, Rules= @Rules
       WHERE TenantId=@TenantId AND WorkflowType=@WorkflowType
    RETURN @@ROWCOUNT
END
' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[crab_GetDataProperties]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[crab_GetDataProperties]
@TenantId    uniqueidentifier,
@ParentId    uniqueidentifier
AS
BEGIN
    IF(@TenantId IS NULL)
        SELECT * FROM dbo.DataProperties
        WHERE ParentId = @ParentId AND TenantId IS NULL
        ORDER BY PropertyType
    ELSE
    BEGIN
       SELECT * FROM dbo.DataProperties d WHERE d.ParentId = @ParentId AND d.TenantId IS NULL
           AND NOT EXISTS(SELECT * FROM dbo.DataProperties t WHERE t.ParentId=@ParentId AND
                          t.PropertyType = d.PropertyType AND t.TenantId=@TenantId)
       UNION ALL
       SELECT * FROM dbo.DataProperties WHERE ParentId = @ParentId AND TenantId = @TenantId
       ORDER BY PropertyType
    END
    RETURN @@ROWCOUNT
END' 
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_DataProperties_DataNodes]') AND parent_object_id = OBJECT_ID(N'[dbo].[DataProperties]'))
ALTER TABLE [dbo].[DataProperties]  WITH CHECK ADD  CONSTRAINT [FK_DataProperties_DataNodes] FOREIGN KEY([ParentId])
REFERENCES [dbo].[DataNodes] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DataProperties] CHECK CONSTRAINT [FK_DataProperties_DataNodes]
