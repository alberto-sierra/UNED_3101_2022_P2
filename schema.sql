CREATE DATABASE citas;

USE citas

-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- citas.dbo.Consultorio definition

-- Drop table

-- DROP TABLE citas.dbo.Consultorio;

CREATE TABLE citas.dbo.Consultorio (
	Id int IDENTITY(1,1) NOT NULL,
	Numero int NOT NULL,
	CONSTRAINT PK__Consulto__3214EC07D6B6ABFC PRIMARY KEY (Id)
);


-- citas.dbo.Especialidad definition

-- Drop table

-- DROP TABLE citas.dbo.Especialidad;

CREATE TABLE citas.dbo.Especialidad (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK__Especial__3214EC073577DBBB PRIMARY KEY (Id)
);


-- citas.dbo.Paciente definition

-- Drop table

-- DROP TABLE citas.dbo.Paciente;

CREATE TABLE citas.dbo.Paciente (
	Id bigint IDENTITY(1,1) NOT NULL,
	Identificacion varchar(9) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Telefono varchar(10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	NombreCompleto varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK__Paciente__3214EC0744820A76 PRIMARY KEY (Id),
	CONSTRAINT UQ__Paciente__D6F931E57485B694 UNIQUE (Identificacion)
);


-- citas.dbo.Equipo definition

-- Drop table

-- DROP TABLE citas.dbo.Equipo;

CREATE TABLE citas.dbo.Equipo (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre varchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	IdEspecialidad int NOT NULL,
	Activo varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Serie varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Descripcion varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	FechaCompra datetime NULL,
	CONSTRAINT PK__Equipo__3214EC079D6312D0 PRIMARY KEY (Id),
	CONSTRAINT FK__Equipo__IdEspeci__571DF1D5 FOREIGN KEY (IdEspecialidad) REFERENCES citas.dbo.Especialidad(Id)
);


-- citas.dbo.Especialista definition

-- Drop table

-- DROP TABLE citas.dbo.Especialista;

CREATE TABLE citas.dbo.Especialista (
	Id int IDENTITY(1,1) NOT NULL,
	Nombre varchar(100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Identificacion varchar(9) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PrecioConsulta money NOT NULL,
	IdEspecialidad int NOT NULL,
	CONSTRAINT PK__Especial__3214EC0720ABD1DE PRIMARY KEY (Id),
	CONSTRAINT UQ__Especial__D6F931E5DC511E7C UNIQUE (Identificacion),
	CONSTRAINT FK__Especiali__IdEsp__5165187F FOREIGN KEY (IdEspecialidad) REFERENCES citas.dbo.Especialidad(Id)
);


-- citas.dbo.ReservaConsultorio definition

-- Drop table

-- DROP TABLE citas.dbo.ReservaConsultorio;

CREATE TABLE citas.dbo.ReservaConsultorio (
	Id int IDENTITY(1,1) NOT NULL,
	IdEspecialista int NOT NULL,
	HoraInicio time NOT NULL,
	HoraFinal time NOT NULL,
	DiaSemana tinyint NOT NULL,
	Disponible bit NOT NULL,
	IdEquipo int NOT NULL,
	IdConsultorio int NOT NULL,
	CONSTRAINT PK__ReservaC__3214EC0711734D00 PRIMARY KEY (Id),
	CONSTRAINT FK__ReservaCo__IdCon__01142BA1 FOREIGN KEY (IdConsultorio) REFERENCES citas.dbo.Consultorio(Id),
	CONSTRAINT FK__ReservaCo__IdEqu__6E01572D FOREIGN KEY (IdEquipo) REFERENCES citas.dbo.Equipo(Id),
	CONSTRAINT FK__ReservaCo__IdEsp__5DCAEF64 FOREIGN KEY (IdEspecialista) REFERENCES citas.dbo.Especialista(Id)
);


-- citas.dbo.Cita definition

-- Drop table

-- DROP TABLE citas.dbo.Cita;

CREATE TABLE citas.dbo.Cita (
	Id int IDENTITY(1,1) NOT NULL,
	IdPaciente bigint NOT NULL,
	IdReserva int NOT NULL,
	PrecioConsulta money NOT NULL,
	Fecha datetime NOT NULL,
	CONSTRAINT PK__Cita__3214EC07A4361294 PRIMARY KEY (Id),
	CONSTRAINT FK__Cita__IdPaciente__619B8048 FOREIGN KEY (IdPaciente) REFERENCES citas.dbo.Paciente(Id),
	CONSTRAINT FK__Cita__IdReserva__628FA481 FOREIGN KEY (IdReserva) REFERENCES citas.dbo.ReservaConsultorio(Id)
);
