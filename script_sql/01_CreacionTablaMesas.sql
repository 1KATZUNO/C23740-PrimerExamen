-- Script provisto por el enunciado del Primer Examen.
-- El diseño de la base de datos NO debe ser modificado por la aplicación.
-- La aplicación se conecta a esta base existente y solo consume el esquema.

USE [ExamenRestaurante]
GO
/****** Object:  Table [dbo].[Mesas]    Script Date: 5/14/2026 10:00:00 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mesas](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[NumeroMesa] [int] NOT NULL,
	[Estado] [int] NOT NULL,
	[Capacidad] [int] NOT NULL,
	[Seccion] [varchar](max) NOT NULL,
	[PrecioPorPersona] [Money] NOT NULL,
	[IdentificacionCliente] [varchar](max)  NULL,
	[NombreCliente] [varchar](max)  NULL,
	[FechaReserva] [date] NULL,
	[CantidadComensales] [int]  NULL,
	[DepositoDeGarantia] [Money]  NULL,


 CONSTRAINT [PK_Mesas] PRIMARY KEY CLUSTERED
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
