using System;

namespace CodeQuest.Models
{
    /// <summary>
    /// Clase que representa un administrador del sistema
    /// Implementa el pilar de Encapsulamiento de POO con propiedades privadas y validaciones
    /// </summary>
    public class Administrator
    {
        private string _username;
        private string _password;

        /// <summary>
        /// ID único del administrador
        /// </summary>
        public int AdminID { get; set; }

        /// <summary>
        /// Nombre de usuario del administrador con validaciones de encapsulamiento
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando el username es inválido</exception>
        public string Username 
        { 
            get => _username;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("El nombre de usuario no puede estar vacío");
                if (value.Length < 3)
                    throw new ArgumentException("El nombre de usuario debe tener al menos 3 caracteres");
                if (value.Length > 50)
                    throw new ArgumentException("El nombre de usuario no puede exceder 50 caracteres");
                 
                _username = value.Trim();
            }
        }

        /// <summary>
        /// Contraseña del administrador con validación
        /// </summary>
        /// <exception cref="ArgumentException">Se lanza cuando la contraseña es inválida</exception>
        public string Password 
        { 
            get => _password;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("La contraseña no puede estar vacía");
                if (value.Length < 4)
                    throw new ArgumentException("La contraseña debe tener al menos 4 caracteres");
                if (value.Length > 100)
                    throw new ArgumentException("La contraseña no puede exceder 100 caracteres");
                 
                _password = value;
            }
        }

        /// <summary>
        /// Fecha de creación del administrador
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Administrator()
        {
            _username = string.Empty;
            _password = string.Empty;
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Constructor con parámetros
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        public Administrator(string username, string password)
        {
            Username = username; 
            Password = password; 
            CreatedAt = DateTime.Now;
        }

        /// <summary>
        /// Verifica si la contraseña proporcionada coincide con la almacenada
        /// </summary>
        /// <param name="passwordToCheck">Contraseña a verificar</param>
        /// <returns>True si las contraseñas coinciden</returns>
        public bool VerifyPassword(string passwordToCheck)
        {
            return Password == passwordToCheck;
        }

        /// <summary>
        /// Actualiza la contraseña del administrador
        /// </summary>
        /// <param name="newPassword">Nueva contraseña</param>
        /// <exception cref="ArgumentException">Se lanza cuando la nueva contraseña es inválida</exception>
        public void UpdatePassword(string newPassword)
        {
            Password = newPassword; 
        }
    }
}