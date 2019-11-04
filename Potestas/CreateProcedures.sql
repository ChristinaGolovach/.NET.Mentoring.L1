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

	SELECT @CoordinateId = @CoordinateId
	FROM Coordinates WHERE X = @X AND Y = @Y

	IF(@CoordinateId = 0)
	BEGIN
		INSERT INTO Coordinates (X, Y)
		VALUES (@X, @Y)

		SELECT @CoordinateId = SCOPE_IDENTITY();
	END
	   
	INSERT INTO FlashObservations(CoordinateId, EstimatedValue, ObservationTime)
	VALUES (@CoordinateId, @EstimatedValue, @ObservationTime)

	COMMIT TRAN
END TRY

BEGIN CATCH
	ROLLBACK TRAN
END CATCH

--------------END Insert_FlasObservation---------------
