using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;

namespace CodeQuest
{
    public class Question
    {
        public int QuestionID { get; set; }
        public string Text { get; set; }
        public int Difficulty { get; set; }
        public List<Choice> Choices { get; set; } = new List<Choice>();
    }

    public class Choice
    {
        public int ChoiceID { get; set; }
        public string ChoiceText { get; set; }
        public bool IsCorrect { get; set; }
    }

    public class RoundResult
    {
        public int Score { get; set; }
        public int XpEarned { get; set; }
        public int Correctas { get; set; }
        public int TiempoTotalSegundos { get; set; }
    }

    public static class GameHelper
    {
        public static int CreateRound(int userId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spRounds_New", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserID", userId);
                    
                    var result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        return SafeConverter.ToInt32(result);
                    }
                    throw new Exception("No se pudo crear la ronda");
                }
            }
        }

        public static List<Question> GetQuestionsForRound(int difficulty)
        {
            var questions = new List<Question>();
            
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                
                // Get 3 random questions of the specified difficulty
                string query = @"
                    SELECT TOP 3 QuestionID, Text, Difficulty 
                    FROM Questions 
                    WHERE Difficulty = @difficulty 
                    ORDER BY NEWID()";
                
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@difficulty", difficulty);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            questions.Add(new Question
                            {
                                QuestionID = SafeConverter.ToInt32(reader["QuestionID"]),
                                Text = reader.GetString("Text"),
                                Difficulty = SafeConverter.ToInt32(reader["Difficulty"])
                            });
                        }
                    }
                }
                
                // Get choices for each question
                foreach (var question in questions)
                {
                    question.Choices = GetChoicesForQuestion(question.QuestionID, connection);
                }
            }
            
            return questions;
        }

        private static List<Choice> GetChoicesForQuestion(int questionId, SqlConnection connection)
        {
            var choices = new List<Choice>();
            
            string query = "SELECT ChoiceID, ChoiceText, IsCorrect FROM Choices WHERE QuestionID = @questionId ORDER BY NEWID()";
            
            using (var command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@questionId", questionId);
                
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        choices.Add(new Choice
                        {
                            ChoiceID = SafeConverter.ToInt32(reader["ChoiceID"]),
                            ChoiceText = reader.GetString("ChoiceText"),
                            IsCorrect = reader.GetBoolean("IsCorrect")
                        });
                    }
                }
            }
            
            return choices;
        }

        public static void SubmitAnswer(int roundId, int questionId, int choiceId, int timeSpentSec)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spRounds_Answer", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoundID", roundId);
                    command.Parameters.AddWithValue("@QuestionID", questionId);
                    command.Parameters.AddWithValue("@ChoiceID", choiceId);
                    command.Parameters.AddWithValue("@TimeSpentSec", timeSpentSec);
                    
                    command.ExecuteNonQuery();
                }
            }
        }

        public static RoundResult CloseRound(int roundId)
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spRounds_Close", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@RoundID", roundId);
                    
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new RoundResult
                            {
                                Score = SafeConverter.ToInt32(reader["Score"]),
                                XpEarned = SafeConverter.ToInt32(reader["XpEarned"]),
                                Correctas = SafeConverter.ToInt32(reader["Correctas"]),
                                TiempoTotalSegundos = SafeConverter.ToInt32(reader["TiempoTotalSegundos"])
                            };
                        }
                    }
                }
            }
            
            return null;
        }

        public static DataTable GetTopRanking()
        {
            using (var connection = DatabaseHelper.GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("spUsers_TopRanking", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    
                    var adapter = new SqlDataAdapter(command);
                    var dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    
                    return dataTable;
                }
            }
        }
    }
}