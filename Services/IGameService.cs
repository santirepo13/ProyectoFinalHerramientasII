using System.Collections.Generic;
using System.Data;
using CodeQuest.Models;

namespace CodeQuest.Services
{
    public interface IGameService
    {
        // User operations
        int CreateUser(string username);
        bool UserExists(string username);
        int GetUserId(string username);
        User GetUserById(int userId);
        /// <summary>
        /// Obtiene todos los usuarios (para administración)
        /// </summary>
        /// <returns>Lista de usuarios</returns>
        List<User> GetAllUsers();

        // Question management (admin)
        /// <summary>
        /// Obtiene todas las preguntas
        /// </summary>
        List<Question> GetAllQuestions();

        /// <summary>
        /// Obtiene una pregunta por su ID
        /// </summary>
        Question GetQuestionById(int questionId);

        /// <summary>
        /// Crea una nueva pregunta
        /// </summary>
        int CreateQuestion(Question question);

        /// <summary>
        /// Crea una nueva opción para una pregunta
        /// </summary>
        int CreateChoice(int questionId, Choice choice);

        /// <summary>
        /// Actualiza una pregunta existente
        /// </summary>
        bool UpdateQuestion(Question question);

        /// <summary>
        /// Elimina una pregunta y sus opciones
        /// </summary>
        bool DeleteQuestion(int questionId);
        // Game operations
        int StartNewRound(int userId);
        List<Question> GetQuestionsForRound(int difficulty);
        void SubmitAnswer(int roundId, int questionId, int choiceId, int timeSpentSec);
        RoundResult CompleteRound(int roundId);
        
        // Statistics
        DataTable GetTopRanking();
        User GetUserStats(int userId);

        // Admin operations for ranking
        /// <summary>
        /// Actualiza el nombre de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="newUsername">Nuevo nombre</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateUsername(int userId, string newUsername);

        /// <summary>
        /// Elimina completamente un usuario del ranking
        /// </summary>
        /// <param name="userId">ID del usuario a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteUserFromRanking(int userId);

        /// <summary>
        /// Resetea el XP de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>True si se reseteó correctamente</returns>
        bool ResetUserXP(int userId);

        /// <summary>
        /// Obtiene las opciones de una pregunta específica
        /// </summary>
        /// <param name="questionId">ID de la pregunta</param>
        /// <returns>Lista de opciones</returns>
        List<Choice> GetChoicesForQuestion(int questionId);

        /// <summary>
        /// Actualiza una opción existente
        /// </summary>
        /// <param name="choiceId">ID de la opción</param>
        /// <param name="choice">Opción con datos actualizados</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateChoice(int choiceId, Choice choice);

        /// <summary>
        /// Elimina una opción
        /// </summary>
        /// <param name="choiceId">ID de la opción a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteChoice(int choiceId);
    }
}