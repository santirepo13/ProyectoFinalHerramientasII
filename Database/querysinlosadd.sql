create database CodeQuest

use CodeQuest

-- Tabla de usuarios
CREATE TABLE Users (
  UserID      INT IDENTITY(1,1) PRIMARY KEY,
  Username    VARCHAR(50) NOT NULL UNIQUE,
  Xp          INT NOT NULL DEFAULT 0,
  Level       INT NOT NULL DEFAULT 1,
  CreatedAt   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME()
);

-- Preguntas 
CREATE TABLE Questions (
  QuestionID  INT IDENTITY(1,1) PRIMARY KEY,
  Text        VARCHAR(1000) NOT NULL,
  Difficulty  TINYINT NOT NULL DEFAULT 1
);
--Respuestas
CREATE TABLE Choices (
  ChoiceID    INT IDENTITY(1,1) PRIMARY KEY,
  QuestionID  INT NOT NULL,
  ChoiceText  VARCHAR(500) NOT NULL,
  IsCorrect   BIT NOT NULL DEFAULT 0,
  CONSTRAINT FK_Choices_Questions
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
      ON DELETE NO ACTION
);


--Rondas
CREATE TABLE Rounds (
  RoundID     INT IDENTITY(1,1) PRIMARY KEY,
  UserID      INT NOT NULL,
  StartedAt   DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
  CompletedAt DATETIME2 NULL,
  Score       INT NOT NULL DEFAULT 0,
  XpEarned    INT NOT NULL DEFAULT 0,
  DurationSec INT NULL,
  CONSTRAINT FK_Rounds_Users
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
      ON UPDATE CASCADE ON DELETE CASCADE
);

--RespuestasporRonda
CREATE TABLE RoundAnswers (
  RoundAnswerID INT IDENTITY(1,1) PRIMARY KEY,
  RoundID       INT NOT NULL,
  QuestionID    INT NOT NULL,
  ChoiceID      INT NOT NULL,
  IsCorrect     BIT NOT NULL,
  AnsweredAt    DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
  TimeSpentSec  INT NOT NULL DEFAULT(0),
  CONSTRAINT UQ_Round_Question UNIQUE (RoundID, QuestionID),
  CONSTRAINT FK_RoundAnswers_Rounds
    FOREIGN KEY (RoundID) REFERENCES Rounds(RoundID)
      ON DELETE CASCADE,
  CONSTRAINT FK_RoundAnswers_Questions
    FOREIGN KEY (QuestionID) REFERENCES Questions(QuestionID)
      ON DELETE NO ACTION,
  CONSTRAINT FK_RoundAnswers_Choices
    FOREIGN KEY (ChoiceID) REFERENCES Choices(ChoiceID)
      ON DELETE NO ACTION
);

--indices
CREATE INDEX IX_Choices_Question ON Choices(QuestionID);
CREATE INDEX IX_Rounds_User ON Rounds(UserID);
CREATE INDEX IX_RoundAnswers_Round ON RoundAnswers(RoundID);
CREATE INDEX IX_RoundAnswers_Question ON RoundAnswers(QuestionID);

--
-- PROCEDIMIENTOS ALMACENADOS
--

--Crear Usuario

Create or Alter Procedure spUser_New
@username varchar (50)
as
BEGIN
    set NOCOUNT on;
    insert into Users(Username) values (@username);
    select SCOPE_IDENTITY() as UserID
    END
    GO


--Crear Pregunta
Create or Alter Procedure spQuestion_New
@Text varchar (1000),
@Difficulty tinyint = 1
As
BEGIN 
    set NOCOUNT on; 
    Insert Into Questions(Text, Difficulty) values (@Text, @Difficulty);
    Select SCOPE_IDENTITY() as QuestionID;
    END
    GO


-- Crear opción
CREATE OR ALTER PROCEDURE spOption_new
  @QuestionID INT,
  @ChoiceText VARCHAR(500),
  @IsCorrect BIT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO Choices (QuestionID, ChoiceText, IsCorrect)
  VALUES (@QuestionID, @ChoiceText, @IsCorrect);
  SELECT SCOPE_IDENTITY() AS ChoiceID;
END
GO

-- Crear ronda para usuario
CREATE OR ALTER PROCEDURE spRounds_New
  @UserID INT
AS
BEGIN
  SET NOCOUNT ON;
  INSERT INTO Rounds (UserID) VALUES (@UserID);
  SELECT SCOPE_IDENTITY() AS RoundID;
END
GO

-- Obtener 3 preguntas aleatorias
CREATE OR ALTER PROCEDURE spQuestions_Take3
AS
BEGIN
  SET NOCOUNT ON;
  SELECT TOP 3 QuestionID
  FROM Questions
  ORDER BY NEWID();
END
GO

-- Registrar respuesta a una pregunta de una ronda
CREATE OR ALTER PROCEDURE spRounds_Answer
  @RoundID INT,
  @QuestionID INT,
  @ChoiceID INT,
  @TimeSpentSec INT
AS
BEGIN
  SET NOCOUNT ON;
  IF NOT EXISTS (SELECT 1 FROM Choices WHERE ChoiceID=@ChoiceID AND QuestionID=@QuestionID)
  BEGIN
    RAISERROR(N'La opción no pertenece a la pregunta indicada.', 16, 1);
    RETURN;
  END;
  INSERT INTO RoundAnswers (RoundID, QuestionID, ChoiceID, IsCorrect, TimeSpentSec)
  SELECT @RoundID, @QuestionID, @ChoiceID, c.IsCorrect, @TimeSpentSec
  FROM Choices c
  WHERE c.ChoiceID = @ChoiceID;
  SELECT SCOPE_IDENTITY() AS RoundAnswerID;
END
GO



-- Cerrar ronda: calcula XP por aciertos y tiempo
CREATE OR ALTER PROCEDURE spRounds_Close
  @RoundID INT
AS
BEGIN
  SET NOCOUNT ON;
  BEGIN TRY
    BEGIN TRAN;
    DECLARE @total INT = (SELECT COUNT(*) FROM RoundAnswers WHERE RoundID=@RoundID);
    IF (@total <> 3)
    BEGIN
      RAISERROR(N'La ronda debe tener exactamente 3 respuestas (actual: %d).', 16, 1, @total);
      ROLLBACK TRAN;
      RETURN;
    END;
    DECLARE @duracion INT = DATEDIFF(SECOND, 
      (SELECT StartedAt FROM Rounds WHERE RoundID=@RoundID),
      SYSUTCDATETIME());

    DECLARE @correctas INT = (
      SELECT COUNT(*) FROM RoundAnswers WHERE RoundID=@RoundID AND IsCorrect=1
    );

    DECLARE @tiempo_total INT = (
      SELECT SUM(TimeSpentSec) FROM RoundAnswers WHERE RoundID=@RoundID
    );

    DECLARE @score INT = @correctas * 10;
    DECLARE @base_xp INT = @score;
    DECLARE @bonus_xp INT = CASE 
      WHEN @tiempo_total <= 30 THEN 20
      WHEN @tiempo_total <= 60 THEN 10
      ELSE 0
      END;
    DECLARE @xp INT = @base_xp + @bonus_xp;

    UPDATE Rounds
    SET CompletedAt = SYSUTCDATETIME(),
        Score = @score,
        XpEarned = @xp,
        DurationSec = @duracion
    WHERE RoundID = @RoundID;

    UPDATE u
      SET u.Xp = u.Xp + @xp,
          u.Level = 1 + (u.Xp / 100)
    FROM Users u
    JOIN Rounds r ON r.UserID = u.UserID
    WHERE r.RoundID = @RoundID;

    COMMIT TRAN;
    SELECT @score AS Score, @xp AS XpEarned, @correctas AS Correctas, @tiempo_total AS TiempoTotalSegundos;
  END TRY
  BEGIN CATCH
    IF XACT_STATE() <> 0 ROLLBACK TRAN;
    DECLARE @msg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(@msg, 16, 1);
  END CATCH
END
GO


-- TOP 10 mejores jugadores
CREATE OR ALTER PROCEDURE spUsers_TopRanking
AS
BEGIN
  SET NOCOUNT ON;
  SELECT TOP 10 
    u.Username, 
    u.Xp, 
    u.Level, 
    COUNT(r.RoundID) AS RondasJugadas, 
    AVG(CAST(r.Score AS FLOAT)) AS ScorePromedio,
    MAX(r.CompletedAt) AS UltimaRonda
  FROM Users u
  LEFT JOIN Rounds r ON r.UserID = u.UserID
  GROUP BY u.UserID, u.Username, u.Xp, u.Level
  ORDER BY u.Xp DESC, ScorePromedio DESC, RondasJugadas DESC;
END
GO


-- Historial de rondas por usuario
CREATE OR ALTER PROCEDURE spRounds_History
  @UserID INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT r.RoundID, r.Score, r.XpEarned, r.DurationSec, r.StartedAt, r.CompletedAt
  FROM Rounds r
  WHERE r.UserID = @UserID
  ORDER BY r.StartedAt DESC;
END
GO

-- Detalle de preguntas y respuestas de una ronda
CREATE OR ALTER PROCEDURE spRounds_Details
  @RoundID INT
AS
BEGIN
  SET NOCOUNT ON;
  SELECT 
      ra.RoundAnswerID,
      q.Text AS Pregunta,
      c.ChoiceText AS Respuesta,
      ra.IsCorrect,
      ra.TimeSpentSec,
      ra.AnsweredAt
  FROM RoundAnswers ra
  INNER JOIN Questions q ON q.QuestionID = ra.QuestionID
  INNER JOIN Choices c ON c.ChoiceID = ra.ChoiceID
  WHERE ra.RoundID = @RoundID
  ORDER BY ra.AnsweredAt ASC;
END
GO