USE [master]
GO
/****** Object:  Database [VeterinariaDb]    Script Date: 22/9/2026 22:09:07 ******/
CREATE DATABASE [VeterinariaDb]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'VeterinariaDb', FILENAME = N'/var/opt/mssql/data/VeterinariaDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'VeterinariaDb_log', FILENAME = N'/var/opt/mssql/data/VeterinariaDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [VeterinariaDb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [VeterinariaDb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [VeterinariaDb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [VeterinariaDb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [VeterinariaDb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [VeterinariaDb] SET ARITHABORT OFF 
GO
ALTER DATABASE [VeterinariaDb] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [VeterinariaDb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [VeterinariaDb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [VeterinariaDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [VeterinariaDb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [VeterinariaDb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [VeterinariaDb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [VeterinariaDb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [VeterinariaDb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [VeterinariaDb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [VeterinariaDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [VeterinariaDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [VeterinariaDb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [VeterinariaDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [VeterinariaDb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [VeterinariaDb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [VeterinariaDb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [VeterinariaDb] SET RECOVERY FULL 
GO
ALTER DATABASE [VeterinariaDb] SET  MULTI_USER 
GO
ALTER DATABASE [VeterinariaDb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [VeterinariaDb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [VeterinariaDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [VeterinariaDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [VeterinariaDb] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'VeterinariaDb', N'ON'
GO
ALTER DATABASE [VeterinariaDb] SET QUERY_STORE = ON
GO
ALTER DATABASE [VeterinariaDb] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO)
GO
USE [VeterinariaDb]
GO
ALTER DATABASE SCOPED CONFIGURATION SET ACCELERATED_PLAN_FORCING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ASYNC_STATS_UPDATE_WAIT_AT_LOW_PRIORITY = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET BATCH_MODE_ADAPTIVE_JOINS = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET BATCH_MODE_MEMORY_GRANT_FEEDBACK = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET BATCH_MODE_ON_ROWSTORE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET CE_FEEDBACK = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET DEFERRED_COMPILATION_TV = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET DOP_FEEDBACK = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET DW_COMPATIBILITY_LEVEL = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ELEVATE_ONLINE = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ELEVATE_RESUMABLE = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET EXEC_QUERY_STATS_FOR_SCALAR_FUNCTIONS = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET FORCE_SHOWPLAN_RUNTIME_PARAMETER_COLLECTION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET GLOBAL_TEMPORARY_TABLE_AUTO_DROP = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET INTERLEAVED_EXECUTION_TVF = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ISOLATE_SECURITY_POLICY_CARDINALITY = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LAST_QUERY_PLAN_STATS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEDGER_DIGEST_STORAGE_ENDPOINT = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LIGHTWEIGHT_QUERY_PROFILING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MEMORY_GRANT_FEEDBACK_PERCENTILE_GRANT = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MEMORY_GRANT_FEEDBACK_PERSISTENCE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET OPTIMIZED_PLAN_FORCING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET OPTIMIZE_FOR_AD_HOC_WORKLOADS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SENSITIVE_PLAN_OPTIMIZATION = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PAUSED_RESUMABLE_INDEX_ABORT_DURATION_MINUTES = 1440;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET ROW_MODE_MEMORY_GRANT_FEEDBACK = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET TSQL_SCALAR_UDF_INLINING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET VERBOSE_TRUNCATION_WARNINGS = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET XTP_PROCEDURE_EXECUTION_STATISTICS = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION SET XTP_QUERY_EXECUTION_STATISTICS = OFF;
GO
USE [VeterinariaDb]
GO
/****** Object:  Table [dbo].[AplicacionVacuna]    Script Date: 22/9/2026 22:09:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AplicacionVacuna](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdConsulta] [bigint] NOT NULL,
	[IdVacuna] [bigint] NOT NULL,
	[FechaAplicacion] [date] NOT NULL,
	[ProximaDosis] [date] NULL,
	[Observaciones] [nvarchar](250) NULL,
	[PrecioUnitario] [decimal](18, 2) NULL,
 CONSTRAINT [PK_AplicacionVacuna] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Consulta]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consulta](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [bigint] NOT NULL,
	[IdMascota] [bigint] NOT NULL,
	[FechaHora] [datetime2](0) NOT NULL,
	[Motivo] [nvarchar](250) NULL,
	[PesoKg] [decimal](6, 2) NULL,
	[Temperatura] [decimal](4, 2) NULL,
	[Diagnostico] [nvarchar](max) NOT NULL,
	[Observaciones] [nvarchar](max) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Consulta] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetalleConsulta]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetalleConsulta](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdConsulta] [bigint] NOT NULL,
	[IdTratamiento] [bigint] NOT NULL,
	[Cantidad] [int] NOT NULL,
	[PrecioUnitario] [decimal](18, 2) NOT NULL,
	[Subtotal] [decimal](18, 2) NOT NULL,
	[Indicaciones] [nvarchar](max) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_DetalleConsulta] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Especie]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Especie](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Especie] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mascota]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mascota](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdPropietario] [bigint] NOT NULL,
	[IdRaza] [bigint] NOT NULL,
	[Nombre] [nvarchar](80) NOT NULL,
	[Sexo] [nvarchar](10) NOT NULL,
	[FechaNacimiento] [date] NULL,
	[Color] [nvarchar](50) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Mascota] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MetodoPago]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MetodoPago](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_MetodoPago] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pago]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pago](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdConsulta] [bigint] NOT NULL,
	[IdMetodoPago] [bigint] NOT NULL,
	[Fecha] [datetime2](0) NOT NULL,
	[Importe] [decimal](18, 2) NOT NULL,
	[Estado] [nvarchar](30) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Pago] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Propietario]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Propietario](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DNI] [nvarchar](20) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[Telefono] [nvarchar](30) NULL,
	[CorreoElectronico] [nvarchar](100) NULL,
	[Direccion] [nvarchar](200) NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Propietario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Raza]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Raza](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdEspecie] [bigint] NOT NULL,
	[Nombre] [nvarchar](80) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Raza] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TipoUsuario]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TipoUsuario](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_TipoUsuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tratamiento]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tratamiento](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TipoTratamiento] [nvarchar](50) NOT NULL,
	[Descripcion] [nvarchar](max) NOT NULL,
	[Dosis] [nvarchar](100) NULL,
	[Precio] [decimal](18, 2) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Tratamiento] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Usuario]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Usuario](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[IdTipoUsuario] [bigint] NOT NULL,
	[NombreUsuario] [nvarchar](50) NOT NULL,
	[HashContrasena] [nvarchar](255) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[Apellido] [nvarchar](100) NOT NULL,
	[DNI] [nvarchar](20) NOT NULL,
	[Matricula] [nvarchar](50) NULL,
	[Activo] [bit] NOT NULL,
	[Direccion] [nvarchar](200) NULL,
	[Telefono] [nvarchar](30) NULL,
	[CorreoElectronico] [nvarchar](100) NULL,
	[FechaNacimiento] [date] NULL,
	[Sexo] [nvarchar](10) NULL,
 CONSTRAINT [PK_Usuario] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vacuna]    Script Date: 22/9/2026 22:09:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vacuna](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Nombre] [nvarchar](100) NOT NULL,
	[PeriodoMesesRecomendado] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[IdEspecie] [bigint] NOT NULL,
	[Precio] [decimal](18, 2) NOT NULL,
 CONSTRAINT [PK_Vacuna] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[AplicacionVacuna] ON 

INSERT [dbo].[AplicacionVacuna] ([Id], [IdConsulta], [IdVacuna], [FechaAplicacion], [ProximaDosis], [Observaciones], [PrecioUnitario]) VALUES (1, 1, 1, CAST(N'2026-10-20' AS Date), CAST(N'2027-10-20' AS Date), NULL, NULL)
SET IDENTITY_INSERT [dbo].[AplicacionVacuna] OFF
SET IDENTITY_INSERT [dbo].[Consulta] ON 

INSERT [dbo].[Consulta] ([Id], [IdUsuario], [IdMascota], [FechaHora], [Motivo], [PesoKg], [Temperatura], [Diagnostico], [Observaciones], [Activo]) VALUES (1, 10, 1, CAST(N'2026-09-20T20:47:52.0000000' AS DateTime2), N'kjijljk', CAST(20.00 AS Decimal(6, 2)), CAST(37.50 AS Decimal(4, 2)), N'bhhjhuhuhjknlk', N'Deshidratada', 1)
INSERT [dbo].[Consulta] ([Id], [IdUsuario], [IdMascota], [FechaHora], [Motivo], [PesoKg], [Temperatura], [Diagnostico], [Observaciones], [Activo]) VALUES (2, 10, 1, CAST(N'2026-09-20T22:26:20.0000000' AS DateTime2), N'DASDASDA', CAST(37.50 AS Decimal(6, 2)), CAST(37.00 AS Decimal(4, 2)), N'uihuhjnlknj', N'jkjñlmñ', 1)
SET IDENTITY_INSERT [dbo].[Consulta] OFF
SET IDENTITY_INSERT [dbo].[DetalleConsulta] ON 

INSERT [dbo].[DetalleConsulta] ([Id], [IdConsulta], [IdTratamiento], [Cantidad], [PrecioUnitario], [Subtotal], [Indicaciones], [Activo]) VALUES (1, 1, 1, 1, CAST(60.00 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), NULL, 1)
INSERT [dbo].[DetalleConsulta] ([Id], [IdConsulta], [IdTratamiento], [Cantidad], [PrecioUnitario], [Subtotal], [Indicaciones], [Activo]) VALUES (2, 1, 2, 1, CAST(15.00 AS Decimal(18, 2)), CAST(15.00 AS Decimal(18, 2)), NULL, 1)
INSERT [dbo].[DetalleConsulta] ([Id], [IdConsulta], [IdTratamiento], [Cantidad], [PrecioUnitario], [Subtotal], [Indicaciones], [Activo]) VALUES (3, 2, 1, 1, CAST(60.00 AS Decimal(18, 2)), CAST(60.00 AS Decimal(18, 2)), NULL, 1)
SET IDENTITY_INSERT [dbo].[DetalleConsulta] OFF
SET IDENTITY_INSERT [dbo].[Especie] ON 

INSERT [dbo].[Especie] ([Id], [Nombre], [Activo]) VALUES (1, N'Canino', 1)
INSERT [dbo].[Especie] ([Id], [Nombre], [Activo]) VALUES (2, N'Felino', 1)
INSERT [dbo].[Especie] ([Id], [Nombre], [Activo]) VALUES (3, N'Ave', 1)
INSERT [dbo].[Especie] ([Id], [Nombre], [Activo]) VALUES (4, N'Roedor', 1)
SET IDENTITY_INSERT [dbo].[Especie] OFF
SET IDENTITY_INSERT [dbo].[Mascota] ON 

INSERT [dbo].[Mascota] ([Id], [IdPropietario], [IdRaza], [Nombre], [Sexo], [FechaNacimiento], [Color], [Activo]) VALUES (1, 1, 7, N'Guillermina', N'Hembra', CAST(N'2026-09-20' AS Date), N'Marron', 1)
INSERT [dbo].[Mascota] ([Id], [IdPropietario], [IdRaza], [Nombre], [Sexo], [FechaNacimiento], [Color], [Activo]) VALUES (2, 1, 3, N'Marcos', N'Macho', CAST(N'2026-02-20' AS Date), N'Blanco', 1)
INSERT [dbo].[Mascota] ([Id], [IdPropietario], [IdRaza], [Nombre], [Sexo], [FechaNacimiento], [Color], [Activo]) VALUES (3, 1, 1, N'Juana', N'Macho', CAST(N'2026-09-20' AS Date), NULL, 1)
SET IDENTITY_INSERT [dbo].[Mascota] OFF
SET IDENTITY_INSERT [dbo].[MetodoPago] ON 

INSERT [dbo].[MetodoPago] ([Id], [Nombre], [Activo]) VALUES (1, N'Efectivo', 1)
INSERT [dbo].[MetodoPago] ([Id], [Nombre], [Activo]) VALUES (2, N'Tarjeta de Débito', 1)
INSERT [dbo].[MetodoPago] ([Id], [Nombre], [Activo]) VALUES (3, N'Tarjeta de Crédito', 1)
INSERT [dbo].[MetodoPago] ([Id], [Nombre], [Activo]) VALUES (4, N'Transferencia Bancaria', 1)
SET IDENTITY_INSERT [dbo].[MetodoPago] OFF
SET IDENTITY_INSERT [dbo].[Propietario] ON 

INSERT [dbo].[Propietario] ([Id], [DNI], [Nombre], [Apellido], [Telefono], [CorreoElectronico], [Direccion], [Activo]) VALUES (1, N'38343166', N'Juana', N'López', N'3875434770', N'morminadany@gmail.com', N'Mendoza N° 435', 0)
SET IDENTITY_INSERT [dbo].[Propietario] OFF
SET IDENTITY_INSERT [dbo].[Raza] ON 

INSERT [dbo].[Raza] ([Id], [IdEspecie], [Nombre], [Activo]) VALUES (1, 1, N'Labrador Retriever', 1)
INSERT [dbo].[Raza] ([Id], [IdEspecie], [Nombre], [Activo]) VALUES (2, 1, N'Pastor Alemán', 1)
INSERT [dbo].[Raza] ([Id], [IdEspecie], [Nombre], [Activo]) VALUES (3, 1, N'SSASAS', 0)
INSERT [dbo].[Raza] ([Id], [IdEspecie], [Nombre], [Activo]) VALUES (4, 1, N'Mestizo Canino', 1)
INSERT [dbo].[Raza] ([Id], [IdEspecie], [Nombre], [Activo]) VALUES (5, 2, N'Siamés', 1)
INSERT [dbo].[Raza] ([Id], [IdEspecie], [Nombre], [Activo]) VALUES (6, 2, N'Persa', 1)
INSERT [dbo].[Raza] ([Id], [IdEspecie], [Nombre], [Activo]) VALUES (7, 2, N'Mestizo Felino', 1)
SET IDENTITY_INSERT [dbo].[Raza] OFF
SET IDENTITY_INSERT [dbo].[TipoUsuario] ON 

INSERT [dbo].[TipoUsuario] ([Id], [Nombre], [Activo]) VALUES (1, N'Administrador', 1)
INSERT [dbo].[TipoUsuario] ([Id], [Nombre], [Activo]) VALUES (2, N'Veterinario', 1)
INSERT [dbo].[TipoUsuario] ([Id], [Nombre], [Activo]) VALUES (3, N'Secretario', 1)
SET IDENTITY_INSERT [dbo].[TipoUsuario] OFF
SET IDENTITY_INSERT [dbo].[Tratamiento] ON 

INSERT [dbo].[Tratamiento] ([Id], [TipoTratamiento], [Descripcion], [Dosis], [Precio], [Activo]) VALUES (1, N'General', N'Antiparasitario', N'1 cada 6 meses', CAST(60.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[Tratamiento] ([Id], [TipoTratamiento], [Descripcion], [Dosis], [Precio], [Activo]) VALUES (2, N'Nutricional', N'Alimento con protecion uniraria', N'2 veces al dia', CAST(15.00 AS Decimal(18, 2)), 0)
SET IDENTITY_INSERT [dbo].[Tratamiento] OFF
SET IDENTITY_INSERT [dbo].[Usuario] ON 

INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (1, 1, N'admin', N'admin123', N'Administrador', N'Sistema', N'12345678', N'ADM-001', 1, NULL, NULL, NULL, NULL, NULL)
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (2, 2, N'vet1', N'$2a$11$eF78YCuDgyd.Ek/H5XZdEuXLzARhN.x9V2KfJnYAGyEKsEfSPLib.', N'Lucia', N'Perez', N'23456789', NULL, 1, N'Calandria N° 584', N'3875434771', N'perezlucia@gmail.com', CAST(N'1994-09-21' AS Date), N'Mujer')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (3, 1, N'GMARTIN.ADMIN', N'$2a$11$nlEpynUEH/e24W8GJH2mGekgUp0GoX9j4hS4h4MVuD/yPdDFaKq7u', N'Martín', N'Gómez', N'34567890', NULL, 0, N'Calandria N° 487', N'38754256', N'gomezmartin@hotmail.com', CAST(N'2008-09-17' AS Date), N'Hombre')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (4, 3, N'secretario', N'$2a$11$lB3gNdyUphX/b08KDTsr7eT1gaxfwnS9ajX/N/Y.QWsktPVvPpv9.', N'Daniela', N'Mormina', N'38343166', NULL, 1, N'Mendoza N° 437', N'3875434770', N'morminadany@gmail.com', CAST(N'1994-10-25' AS Date), N'Mujer')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (5, 2, N'DMORMINAA', N'$2a$11$z/Dp/m9GMqNG3dBdoJlZB.5gnA5CwpB2m971O4yU7J8KU8eut2Jy6', N'Daniela', N'Mormina', N'3834316', NULL, 0, N'Mendoza N° 154', N'5448455656', N'morminaDani@gmail.com', CAST(N'1994-09-17' AS Date), N'Mujer')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (6, 2, N'dmormi', N'$2a$11$RD5H3kd2M3iJeBJnABg6WOjuZqRCV3/zAjizFq1foR2suyGjHePru', N'Daniela', N'Elizabeth', N'38343167', NULL, 0, N'Mendoza N° 458', N'38757440', N'morminad@gmail.com', CAST(N'1995-09-17' AS Date), N'Hombre')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (7, 2, N'LLOPEZ', N'$2a$11$aG7z379Ov1CQxUNzrV8GhuW8prJvWg2FZQoCiOGnSziGsIdP/.O5W', N'Laura a', N'Lopez', N'15254585', NULL, 1, N'calandria N° 5845', N'3875434', N'lopezlaura@gmail.com', CAST(N'1998-09-17' AS Date), N'Mujer')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (8, 2, N'DNJANKA', N'$2a$11$alaj9Ctmmk/kQA2u7eWOdes4ivrSXO3oXyyRdvonWH7HCKFkF69Fa', N'da', N'FDDFSDF', N'38343168', NULL, 1, N'mendoza N° 452', N'387546055', N'mormina@gmail.com', CAST(N'1998-09-19' AS Date), N'Mujer')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (9, 1, N'LLOPEZ.ADM', N'$2a$11$B9fnqzmMt4X/9Sb7q/hVgug0V12up3RX9xSEs8MOaXf5TOhwSjdGe', N'Luana', N'Lopez', N'38343170', NULL, 1, N'Mendoza N° 890', N'3875435770', N'lopezluana@gmail.com', CAST(N'1995-10-25' AS Date), N'Mujer')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (10, 2, N'MROGRIGUEZ.VET', N'$2a$11$EVpd4404qOkCIdmiRikNP.9KwrX9bVxa3TlCviloob8GuRd96jA3e', N'MARIANO', N'RODRIGUEZ', N'39343166', NULL, 1, N'AV. ITALIA N° 484', N'3624424770', N'rodriguezmariano@gmaill.com', CAST(N'2008-09-22' AS Date), N'Hombre')
INSERT [dbo].[Usuario] ([Id], [IdTipoUsuario], [NombreUsuario], [HashContrasena], [Nombre], [Apellido], [DNI], [Matricula], [Activo], [Direccion], [Telefono], [CorreoElectronico], [FechaNacimiento], [Sexo]) VALUES (11, 3, N'CMORALES.SEC', N'$2a$11$.h/YtO5FDpLO2kuEMhCpke0oNzAVam08SyJkxuLVEerhFX7bDIXiu', N'Clara', N'Morales', N'45261588', NULL, 1, N'Salta N° 787', N'37945847820', N'moralesclara@gmail.com', CAST(N'2000-09-22' AS Date), N'Mujer')
SET IDENTITY_INSERT [dbo].[Usuario] OFF
SET IDENTITY_INSERT [dbo].[Vacuna] ON 

INSERT [dbo].[Vacuna] ([Id], [Nombre], [PeriodoMesesRecomendado], [Activo], [IdEspecie], [Precio]) VALUES (1, N'Q2', 12, 1, 2, CAST(50.00 AS Decimal(18, 2)))
SET IDENTITY_INSERT [dbo].[Vacuna] OFF
/****** Object:  Index [IX_AplicacionVacuna_IdConsulta]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_AplicacionVacuna_IdConsulta] ON [dbo].[AplicacionVacuna]
(
	[IdConsulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_AplicacionVacuna_IdVacuna]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_AplicacionVacuna_IdVacuna] ON [dbo].[AplicacionVacuna]
(
	[IdVacuna] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Consulta_FechaHora]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Consulta_FechaHora] ON [dbo].[Consulta]
(
	[FechaHora] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Consulta_IdMascota]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Consulta_IdMascota] ON [dbo].[Consulta]
(
	[IdMascota] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Consulta_IdUsuario]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Consulta_IdUsuario] ON [dbo].[Consulta]
(
	[IdUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleConsulta_IdConsulta]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleConsulta_IdConsulta] ON [dbo].[DetalleConsulta]
(
	[IdConsulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_DetalleConsulta_IdTratamiento]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_DetalleConsulta_IdTratamiento] ON [dbo].[DetalleConsulta]
(
	[IdTratamiento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Mascota_IdPropietario]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Mascota_IdPropietario] ON [dbo].[Mascota]
(
	[IdPropietario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Mascota_IdRaza]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Mascota_IdRaza] ON [dbo].[Mascota]
(
	[IdRaza] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pago_IdConsulta]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Pago_IdConsulta] ON [dbo].[Pago]
(
	[IdConsulta] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Pago_IdMetodoPago]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Pago_IdMetodoPago] ON [dbo].[Pago]
(
	[IdMetodoPago] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Propietario_DNI]    Script Date: 22/9/2026 22:09:09 ******/
ALTER TABLE [dbo].[Propietario] ADD  CONSTRAINT [UQ_Propietario_DNI] UNIQUE NONCLUSTERED 
(
	[DNI] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Raza_IdEspecie]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Raza_IdEspecie] ON [dbo].[Raza]
(
	[IdEspecie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Usuario_IdTipoUsuario]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Usuario_IdTipoUsuario] ON [dbo].[Usuario]
(
	[IdTipoUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Usuario_NombreUsuario]    Script Date: 22/9/2026 22:09:09 ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_Usuario_NombreUsuario] ON [dbo].[Usuario]
(
	[NombreUsuario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_Vacuna_IdEspecie]    Script Date: 22/9/2026 22:09:09 ******/
CREATE NONCLUSTERED INDEX [IX_Vacuna_IdEspecie] ON [dbo].[Vacuna]
(
	[IdEspecie] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Consulta] ADD  CONSTRAINT [DF_Consulta_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[DetalleConsulta] ADD  CONSTRAINT [DF_DetalleConsulta_Cantidad]  DEFAULT ((1)) FOR [Cantidad]
GO
ALTER TABLE [dbo].[DetalleConsulta] ADD  CONSTRAINT [DF_DetalleConsulta_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Especie] ADD  CONSTRAINT [DF_Especie_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Mascota] ADD  CONSTRAINT [DF_Mascota_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[MetodoPago] ADD  CONSTRAINT [DF_MetodoPago_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Pago] ADD  CONSTRAINT [DF_Pago_Estado]  DEFAULT ('Completado') FOR [Estado]
GO
ALTER TABLE [dbo].[Pago] ADD  CONSTRAINT [DF_Pago_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Propietario] ADD  CONSTRAINT [DF_Propietario_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Raza] ADD  CONSTRAINT [DF_Raza_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[TipoUsuario] ADD  CONSTRAINT [DF_TipoUsuario_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Tratamiento] ADD  CONSTRAINT [DF_Tratamiento_Precio]  DEFAULT ((0)) FOR [Precio]
GO
ALTER TABLE [dbo].[Tratamiento] ADD  CONSTRAINT [DF_Tratamiento_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Usuario] ADD  CONSTRAINT [DF_Usuario_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Vacuna] ADD  CONSTRAINT [DF_Vacuna_Periodo]  DEFAULT ((12)) FOR [PeriodoMesesRecomendado]
GO
ALTER TABLE [dbo].[Vacuna] ADD  CONSTRAINT [DF_Vacuna_Activo]  DEFAULT ((1)) FOR [Activo]
GO
ALTER TABLE [dbo].[Vacuna] ADD  CONSTRAINT [DF_Vacuna_Precio]  DEFAULT ((0)) FOR [Precio]
GO
ALTER TABLE [dbo].[AplicacionVacuna]  WITH CHECK ADD  CONSTRAINT [FK_AplicacionVacuna_Consulta] FOREIGN KEY([IdConsulta])
REFERENCES [dbo].[Consulta] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AplicacionVacuna] CHECK CONSTRAINT [FK_AplicacionVacuna_Consulta]
GO
ALTER TABLE [dbo].[AplicacionVacuna]  WITH CHECK ADD  CONSTRAINT [FK_AplicacionVacuna_Vacuna] FOREIGN KEY([IdVacuna])
REFERENCES [dbo].[Vacuna] ([Id])
GO
ALTER TABLE [dbo].[AplicacionVacuna] CHECK CONSTRAINT [FK_AplicacionVacuna_Vacuna]
GO
ALTER TABLE [dbo].[Consulta]  WITH CHECK ADD  CONSTRAINT [FK_Consulta_Mascota] FOREIGN KEY([IdMascota])
REFERENCES [dbo].[Mascota] ([Id])
GO
ALTER TABLE [dbo].[Consulta] CHECK CONSTRAINT [FK_Consulta_Mascota]
GO
ALTER TABLE [dbo].[Consulta]  WITH CHECK ADD  CONSTRAINT [FK_Consulta_Usuario] FOREIGN KEY([IdUsuario])
REFERENCES [dbo].[Usuario] ([Id])
GO
ALTER TABLE [dbo].[Consulta] CHECK CONSTRAINT [FK_Consulta_Usuario]
GO
ALTER TABLE [dbo].[DetalleConsulta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleConsulta_Consulta] FOREIGN KEY([IdConsulta])
REFERENCES [dbo].[Consulta] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[DetalleConsulta] CHECK CONSTRAINT [FK_DetalleConsulta_Consulta]
GO
ALTER TABLE [dbo].[DetalleConsulta]  WITH CHECK ADD  CONSTRAINT [FK_DetalleConsulta_Tratamiento] FOREIGN KEY([IdTratamiento])
REFERENCES [dbo].[Tratamiento] ([Id])
GO
ALTER TABLE [dbo].[DetalleConsulta] CHECK CONSTRAINT [FK_DetalleConsulta_Tratamiento]
GO
ALTER TABLE [dbo].[Mascota]  WITH CHECK ADD  CONSTRAINT [FK_Mascota_Propietario] FOREIGN KEY([IdPropietario])
REFERENCES [dbo].[Propietario] ([Id])
GO
ALTER TABLE [dbo].[Mascota] CHECK CONSTRAINT [FK_Mascota_Propietario]
GO
ALTER TABLE [dbo].[Mascota]  WITH CHECK ADD  CONSTRAINT [FK_Mascota_Raza] FOREIGN KEY([IdRaza])
REFERENCES [dbo].[Raza] ([Id])
GO
ALTER TABLE [dbo].[Mascota] CHECK CONSTRAINT [FK_Mascota_Raza]
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_Consulta] FOREIGN KEY([IdConsulta])
REFERENCES [dbo].[Consulta] ([Id])
GO
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_Consulta]
GO
ALTER TABLE [dbo].[Pago]  WITH CHECK ADD  CONSTRAINT [FK_Pago_MetodoPago] FOREIGN KEY([IdMetodoPago])
REFERENCES [dbo].[MetodoPago] ([Id])
GO
ALTER TABLE [dbo].[Pago] CHECK CONSTRAINT [FK_Pago_MetodoPago]
GO
ALTER TABLE [dbo].[Raza]  WITH CHECK ADD  CONSTRAINT [FK_Raza_Especie] FOREIGN KEY([IdEspecie])
REFERENCES [dbo].[Especie] ([Id])
GO
ALTER TABLE [dbo].[Raza] CHECK CONSTRAINT [FK_Raza_Especie]
GO
ALTER TABLE [dbo].[Usuario]  WITH CHECK ADD  CONSTRAINT [FK_Usuario_TipoUsuario] FOREIGN KEY([IdTipoUsuario])
REFERENCES [dbo].[TipoUsuario] ([Id])
GO
ALTER TABLE [dbo].[Usuario] CHECK CONSTRAINT [FK_Usuario_TipoUsuario]
GO
ALTER TABLE [dbo].[Vacuna]  WITH CHECK ADD  CONSTRAINT [FK_Vacuna_Especie] FOREIGN KEY([IdEspecie])
REFERENCES [dbo].[Especie] ([Id])
GO
ALTER TABLE [dbo].[Vacuna] CHECK CONSTRAINT [FK_Vacuna_Especie]
GO
USE [master]
GO
ALTER DATABASE [VeterinariaDb] SET  READ_WRITE 
GO
