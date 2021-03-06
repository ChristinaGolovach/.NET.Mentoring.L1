﻿USE [master]
IF EXISTS (SELECT * FROM sys.databases where name='Observation')
DROP DATABASE Observation
GO

CREATE DATABASE [Observation]
COLLATE SQL_Latin1_General_CP1251_CI_AS; 
GO

USE [Observation]
GO

CREATE TABLE [dbo].[Coordinates](
	[Id] INT IDENTITY (1,1) NOT NULL,
	[X] FLOAT(53) NOT NULL,
	[Y] FLOAT(53) NOT NULL,
	CONSTRAINT PK_Coordinates PRIMARY KEY ([Id]))
GO

CREATE TABLE [dbo].[EnergyObservations](
	[Id] INT IDENTITY (1,1) NOT NULL,
	[CoordinateId] INT NOT NULL,
	[EstimatedValue] FLOAT(53) NOT NULL,
	[ObservationTime] DATETIME NOT NULL,	
	CONSTRAINT PK_EnergyObservations PRIMARY KEY ([Id]),
	CONSTRAINT FK_EnergyObservations_Coordinates_CoordinateId FOREIGN KEY ([CoordinateId]) REFERENCES [Coordinates]([Id]))
GO