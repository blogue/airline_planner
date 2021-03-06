USE [airline_planner]
GO
/****** Object:  Table [dbo].[cities]    Script Date: 7/18/2016 4:44:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[cities](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](255) NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[departures_arrivals]    Script Date: 7/18/2016 4:44:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[departures_arrivals](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[departure_city_id] [int] NULL,
	[arrival_city_id] [int] NULL,
	[flight_id] [int] NULL,
	[departure_time] [date] NULL,
	[arrival_time] [date] NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[flights]    Script Date: 7/18/2016 4:44:40 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[flights](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](50) NULL,
	[status] [varchar](50) NULL,
	[departure_time] [date] NULL,
	[arrival_time] [date] NULL
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
