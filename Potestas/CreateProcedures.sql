--------------BEGIN Insert_EnergyObservation---------------

IF (OBJECT_ID('Insert_EnergyObservation') IS NOT NULL)
  DROP PROCEDURE Insert_EnergyObservation
GO

CREATE PROCEDURE [dbo].[Insert_EnergyObservation]
	@X FLOAT(53),
	@Y FLOAT(53),
	@EstimatedValue FLOAT(53),
	@ObservationTime DATETIME
AS

DECLARE @CoordinateId INT = 0

BEGIN TRY
	BEGIN TRAN

	SELECT @CoordinateId = @CoordinateId
	FROM Coordinates WHERE X = @X AND Y = @Y

	IF(@CoordinateId = 0)
	BEGIN
		INSERT INTO Coordinates (X, Y)
		VALUES (@X, @Y)

		SELECT @CoordinateId = SCOPE_IDENTITY();
	END
	   
	INSERT INTO EnergyObservations(CoordinateId, EstimatedValue, ObservationTime)
	VALUES (@CoordinateId, @EstimatedValue, @ObservationTime)

	COMMIT TRAN
END TRY

BEGIN CATCH
	ROLLBACK TRAN
END CATCH
GO
--------------END Insert_EnergyObservation---------------

--------------BEGIN  GetAverageEnergy------------------
IF (OBJECT_ID('Select_Average_Energy') IS NOT NULL)
  DROP PROCEDURE Select_Average_Energy
GO

CREATE PROCEDURE [dbo].[Select_Average_Energy]
AS
	SELECT AVG(EstimatedValue)
	FROM EnergyObservations
GO


IF (OBJECT_ID('Select_Average_Energy_Between_Dates') IS NOT NULL)
  DROP PROCEDURE Select_Average_Energy_Between_Dates
GO

CREATE PROCEDURE [dbo].[Select_Average_Energy_Between_Dates]
	@StartFrom DATETIME,
	@EndBy DATETIME
AS
	SELECT AVG(EstimatedValue)
	FROM EnergyObservations
	WHERE EnergyObservations.ObservationTime >= @StartFrom AND EnergyObservations.ObservationTime <= @EndBy
GO


IF (OBJECT_ID('Select_Average_Energy_Between_Coordinates') IS NOT NULL)
  DROP PROCEDURE Select_Average_Energy_Between_Coordinates
GO

CREATE PROCEDURE [dbo].[Select_Average_Energy_Between_Coordinates]
	@TopLeftX FLOAT(53),
	@TopLeftY FLOAT(53),
	@BottomRightX FLOAT(53),
	@BottomRightY FLOAT(53)
AS
	SELECT AVG(EstimatedValue)
	FROM EnergyObservations
	JOIN Coordinates ON Coordinates.Id = EnergyObservations.CoordinateId
	WHERE X > @TopLeftX AND 
		  X < @BottomRightX AND 
		  Y > @TopLeftY AND
		  Y < @BottomRightY
GO
--------------END  GetAverageEnergy--------------------

--------------BEGIN GetDistributionByEnergyValue-------
IF (OBJECT_ID('Select_Distribution_By_EnergyValue') IS NOT NULL)
  DROP PROCEDURE Select_Distribution_By_EnergyValue
GO

CREATE PROCEDURE [dbo].[Select_Distribution_By_EnergyValue]
AS
	SELECT EstimatedValue, COUNT(Id) as 'Count'
  FROM EnergyObservations
  GROUP BY EstimatedValue
GO
--------------END GetDistributionByEnergyValue---------

--------------BEGIN GetDistributionByCoordinates-------
IF (OBJECT_ID('Select_Distribution_By_Coordinates') IS NOT NULL)
  DROP PROCEDURE Select_Distribution_By_Coordinates
GO

CREATE PROCEDURE [dbo].[Select_Distribution_By_Coordinates]
AS
	SELECT Coordinates.Id, Coordinates.X,Coordinates.Y, COUNT(EnergyObservations.Id) AS 'Count'
  FROM EnergyObservations
  JOIN Coordinates ON Coordinates.Id = EnergyObservations.CoordinateId
  GROUP BY Coordinates.Id, Coordinates.X,Coordinates.Y
GO
--------------END GetDistributionByCoordinates---------

--------------BEGIN GetDistributionByObservationTime---
IF (OBJECT_ID('Select_Distribution_By_ObservationTime') IS NOT NULL)
  DROP PROCEDURE Select_Distribution_By_ObservationTime
GO

CREATE PROCEDURE [dbo].[Select_Distribution_By_ObservationTime]
AS
	SELECT ObservationTime, COUNT(Id) AS 'Count'
  FROM EnergyObservations
  GROUP BY ObservationTime
GO
--------------END GetDistributionByObservationTime-----

--------------BEGIN  GetMaxEnergy----------------------
IF (OBJECT_ID('Select_Max_Energy') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy]
AS
	SELECT MAX(EstimatedValue)
	FROM EnergyObservations
GO

IF (OBJECT_ID('Select_Max_Energy_By_Date') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy_By_Date
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy_By_Date]
	@Date DATETIME
AS
	SELECT MAX(EstimatedValue)
	FROM EnergyObservations
	WHERE EnergyObservations.ObservationTime = @Date
GO


IF (OBJECT_ID('Select_Max_Energy_By_Coordinate') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy_By_Coordinate
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy_By_Coordinate]
	@X FLOAT(53),
	@Y FLOAT(53)
AS
	SELECT MAX(EstimatedValue)
	FROM EnergyObservations
	JOIN Coordinates ON Coordinates.Id = EnergyObservations.CoordinateId
	WHERE X = @X AND Y = @Y
GO
--------------END  GetMaxEnergy------------------------

--------------BEGIN  GetMaxEnergyPosition--------------
IF (OBJECT_ID('Select_Max_Energy_Position') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy_Position
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy_Position]
AS
	SELECT Coordinates.Id, X, Y
	FROM Coordinates
	WHERE Coordinates.Id = (SELECT TOP 1 EnergyObservations.CoordinateId 
                          FROM EnergyObservations 
                          ORDER BY EnergyObservations.EstimatedValue DESC)
GO
--------------END  GetMaxEnergyPosition----------------

--------------BEGIN  GetMaxEnergyTime------------------
IF (OBJECT_ID('Select_Max_Energy_Time') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy_Time
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy_Time]
AS
	SELECT TOP 1 EnergyObservations.ObservationTime
	FROM EnergyObservations
  ORDER BY EnergyObservations.EstimatedValue DESC
GO
--------------END  GetMaxEnergyTime--------------------

--------------BEGIN GetMinEnergy-----------------------
IF (OBJECT_ID('Select_Min_Energy') IS NOT NULL)
  DROP PROCEDURE Select_Min_Energy
GO

CREATE PROCEDURE [dbo].[Select_Min_Energy]
AS
	SELECT MIN(EstimatedValue)
	FROM EnergyObservations
GO


IF (OBJECT_ID('Select_Min_Energy_By_Coordinate') IS NOT NULL)
  DROP PROCEDURE Select_Min_Energy_By_Coordinate
GO

CREATE PROCEDURE [dbo].[Select_Min_Energy_By_Coordinate]
	@X FLOAT(53),
	@Y FLOAT(53)
AS
	SELECT MIN(EstimatedValue)
	FROM EnergyObservations
	JOIN Coordinates ON Coordinates.Id = EnergyObservations.CoordinateId
	WHERE X = @X AND Y = @Y
GO

IF (OBJECT_ID('Select_Min_Energy_By_Date') IS NOT NULL)
  DROP PROCEDURE Select_Min_Energy_By_Date
GO

CREATE PROCEDURE [dbo].[Select_Min_Energy_By_Date]
	@Date DATETIME
AS
	SELECT MIN(EstimatedValue)
	FROM EnergyObservations
	WHERE EnergyObservations.ObservationTime = @Date
GO
--------------END GetMinEnergy-------------------------

--------------BEGIN  GetMinEnergyPosition--------------
IF (OBJECT_ID('Select_Min_Energy_Position') IS NOT NULL)
  DROP PROCEDURE Select_Min_Energy_Position
GO

CREATE PROCEDURE [dbo].[Select_Min_Energy_Position]
AS
	SELECT Coordinates.Id, X, Y
	FROM Coordinates
	WHERE Coordinates.Id = (SELECT TOP 1 EnergyObservations.CoordinateId 
                          FROM EnergyObservations 
                          ORDER BY EnergyObservations.EstimatedValue)
GO
--------------END  GetMinEnergyPosition----------------

--------------BEGIN  GetMinEnergyTime------------------
IF (OBJECT_ID('Select_Min_Energy_Time') IS NOT NULL)
  DROP PROCEDURE Select_Min_Energy_Time
GO

CREATE PROCEDURE [dbo].[Select_Min_Energy_Time]
AS
	SELECT TOP 1 EnergyObservations.ObservationTime
	FROM EnergyObservations
  ORDER BY EnergyObservations.EstimatedValue
GO
--------------END  GetMaxEnergyTime--------------------
