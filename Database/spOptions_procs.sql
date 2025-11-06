-- SQL Server stored procedures to manage Choices (Options) for Questions
-- Provides: spOptions_GetByQuestion, spOption_New, spOption_Update, spOption_Delete

/* Drop + Create spOptions_GetByQuestion */
IF OBJECT_ID('dbo.spOptions_GetByQuestion', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spOptions_GetByQuestion;
GO

CREATE PROCEDURE dbo.spOptions_GetByQuestion
    @QuestionID INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ChoiceID, QuestionID, ChoiceText, IsCorrect
    FROM Choices
    WHERE QuestionID = @QuestionID
    ORDER BY ChoiceID;
END
GO

/* Drop + Create spOption_New (inserts an option; if marked correct, clears others) */
IF OBJECT_ID('dbo.spOption_New', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spOption_New;
GO

CREATE PROCEDURE dbo.spOption_New
    @QuestionID INT,
    @ChoiceText NVARCHAR(500),
    @IsCorrect BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        IF @IsCorrect = 1
        BEGIN
            UPDATE Choices
            SET IsCorrect = 0
            WHERE QuestionID = @QuestionID;
        END

        INSERT INTO Choices (QuestionID, ChoiceText, IsCorrect)
        VALUES (@QuestionID, @ChoiceText, @IsCorrect);

        DECLARE @NewID INT = SCOPE_IDENTITY();

        COMMIT TRANSACTION;

        SELECT @NewID AS ChoiceID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

/* Drop + Create spOption_Update (updates text and IsCorrect; atomically clears other correct flags if needed) */
IF OBJECT_ID('dbo.spOption_Update', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spOption_Update;
GO

CREATE PROCEDURE dbo.spOption_Update
    @ChoiceID INT,
    @ChoiceText NVARCHAR(500),
    @IsCorrect BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @QuestionID INT;
        SELECT @QuestionID = QuestionID FROM Choices WHERE ChoiceID = @ChoiceID;
        IF @QuestionID IS NULL
        BEGIN
            RAISERROR('Choice not found', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        IF @IsCorrect = 1
        BEGIN
            UPDATE Choices SET IsCorrect = 0 WHERE QuestionID = @QuestionID;
        END

        UPDATE Choices
        SET ChoiceText = @ChoiceText,
            IsCorrect = @IsCorrect
        WHERE ChoiceID = @ChoiceID;

        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO

/* Drop + Create spOption_Delete (deletes a choice) */
IF OBJECT_ID('dbo.spOption_Delete', 'P') IS NOT NULL
    DROP PROCEDURE dbo.spOption_Delete;
GO

CREATE PROCEDURE dbo.spOption_Delete
    @ChoiceID INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRANSACTION;

        DECLARE @QuestionID INT;
        SELECT @QuestionID = QuestionID FROM Choices WHERE ChoiceID = @ChoiceID;
        IF @QuestionID IS NULL
        BEGIN
            RAISERROR('Choice not found', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END

        DELETE FROM Choices WHERE ChoiceID = @ChoiceID;

        COMMIT TRANSACTION;
        SELECT 1 AS Success;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        THROW;
    END CATCH
END
GO