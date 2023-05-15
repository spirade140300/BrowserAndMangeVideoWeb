CREATE TABLE [dbo].[Movies]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(20) NOT NULL, 
    [Path] NCHAR(40) NULL, 
    [Actor] NCHAR(10) NULL, 
    [Framwitdh] NCHAR(5) NULL, 
    [Farmheight] NCHAR(5) NULL, 
	[ContentType] NVARCHAR(10) NULL, 
    [IsDeleted] BIT NULL, 
    [Rating] VARCHAR(10) NULL, 
    [TotalBitrate] NCHAR(10) NULL, 
    [EncodingBitrate] NCHAR(10) NULL, 
    [Size] NCHAR(10) NULL
)
