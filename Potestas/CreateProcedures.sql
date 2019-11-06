--------------BEGIN Insert_FlasObservation---------------

IF (OBJECT_ID('Insert_FlasObservation') IS NOT NULL)
  DROP PROCEDURE Insert_FlasObservation
GO

CREATE PROCEDURE [dbo].[Insert_FlasObservation]
	@X FLOAT(53),
	@Y FLOAT(53),
	--@DurationMs INT,
	--@Intensity FLOAT(53),
	@EstimatedValue FLOAT(53),
	@ObservationTime DATETIME
AS

DECLARE @CoordinateId INT = 0

BEGIN TRY
	BEGIN TRAN

	--SELECT @CoordinateId = @CoordinateId
	--FROM Coordinates WHERE X = @X AND Y = @Y

	--IF(@CoordinateId = 0)
	--BEGIN
	--	INSERT INTO Coordinates (X, Y)
	--	VALUES (@X, @Y)

	--	SELECT @CoordinateId = SCOPE_IDENTITY();
	--END

	INSERT INTO Coordinates (X, Y)
	VALUES (@X, @Y)

	SELECT @CoordinateId = SCOPE_IDENTITY();
	   
	INSERT INTO FlashObservations(CoordinateId, EstimatedValue, ObservationTime)
	VALUES (@CoordinateId, @EstimatedValue, @ObservationTime)

	COMMIT TRAN
END TRY

BEGIN CATCH
	ROLLBACK TRAN
END CATCH
GO
--------------END Insert_FlasObservation---------------

--------------BEGIN  GetAverageEnergy------------------
IF (OBJECT_ID('Select_Average_Energy') IS NOT NULL)
  DROP PROCEDURE Select_Average_Energy
GO

CREATE PROCEDURE [dbo].[Select_Average_Energy]
AS
	SELECT AVG(EstimatedValue)
	FROM FlashObservations
GO


IF (OBJECT_ID('Select_Average_Energy_Between_Dates') IS NOT NULL)
  DROP PROCEDURE Select_Average_Energy_Between_Dates
GO

CREATE PROCEDURE [dbo].[Select_Average_Energy_Between_Dates]
	@StartFrom DATETIME,
	@EndBy DATETIME
AS
	SELECT AVG(EstimatedValue)
	FROM FlashObservations
	WHERE FlashObservations.ObservationTime >= @StartFrom AND FlashObservations.ObservationTime <= @EndBy
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
	FROM FlashObservations
	JOIN Coordinates ON Coordinates.Id = FlashObservations.CoordinateId
	WHERE X > @TopLeftX AND 
		  X < @BottomRightX AND 
		  Y > @TopLeftY AND
		  Y < @BottomRightY
GO
--------------END  GetAverageEnergy--------------------

--------------BEGIN  GetDistributionByCoordinates------
--------------END  GetDistributionByCoordinates--------


--------------BEGIN  GetMaxEnergy----------------------
IF (OBJECT_ID('Select_Max_Energy') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy]
AS
	SELECT MAX(EstimatedValue)
	FROM FlashObservations
GO

IF (OBJECT_ID('Select_Max_Energy_Between_Dates') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy_Between_Dates
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy_Between_Dates]
	@StartFrom DATETIME,
	@EndBy DATETIME
AS
	SELECT MAX(EstimatedValue)
	FROM FlashObservations
	WHERE FlashObservations.ObservationTime >= @StartFrom AND FlashObservations.ObservationTime <= @EndBy
GO


IF (OBJECT_ID('Select_Max_Energy_Between_Coordinates') IS NOT NULL)
  DROP PROCEDURE Select_Max_Energy_Between_Coordinates
GO

CREATE PROCEDURE [dbo].[Select_Max_Energy_Between_Coordinates]
	@TopLeftX FLOAT(53),
	@TopLeftY FLOAT(53),
	@BottomRightX FLOAT(53),
	@BottomRightY FLOAT(53)
AS
	SELECT MAX(EstimatedValue)
	FROM FlashObservations
	JOIN Coordinates ON Coordinates.Id = FlashObservations.CoordinateId
	WHERE X > @TopLeftX AND 
		  X < @BottomRightX AND 
		  Y > @TopLeftY AND
		  Y < @BottomRightY
GO
--------------END  GetMaxEnergy------------------------

--------------BEGIN  GetMaxEnergyPosition--------------
--------------d=sqrt(x2−x1)^2+(y2−y1)^2 --where 

--IF (OBJECT_ID('Select_Max_Energy_Position') IS NOT NULL)
--  DROP PROCEDURE Select_Max_Energy_Position
--GO

--CREATE PROCEDURE [dbo].[Select_Max_Energy_Position]
--AS
--	SELECT Coordinates.Id, X, Y
--	FROM FlashObservations
--	JOIN Coordinates ON Coordinates.Id = FlashObservations.CoordinateId
--	--WHERE 
--GO
--------------END  GetMaxEnergyPosition----------------

--------------BEGIN  GetMaxEnergyTime------------------
--------------END  GetMaxEnergyTime--------------------