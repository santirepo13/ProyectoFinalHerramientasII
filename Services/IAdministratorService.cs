using System.Collections.Generic;
using CodeQuest.Models;

namespace CodeQuest.Services
{
    /// <summary>
    /// Interfaz que define los servicios de administración del sistema
    /// Implementa el pilar de Abstracción de POO
    /// </summary>
    public interface IAdministratorService
    {
        /// <summary>
        /// Autentica un administrador en el sistema
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Administrador autenticado o null si falla</returns>
        Administrator Authenticate(string username, string password);

        /// <summary>
        /// Crea un nuevo administrador
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>ID del administrador creado</returns>
        int CreateAdministrator(string username, string password);

        /// <summary>
        /// Obtiene todos los administradores
        /// </summary>
        /// <returns>Lista de administradores</returns>
        List<Administrator> GetAllAdministrators();

        /// <summary>
        /// Obtiene un administrador por su ID
        /// </summary>
        /// <param name="adminId">ID del administrador</param>
        /// <returns>Administrador encontrado o null</returns>
        Administrator GetAdministratorById(int adminId);

        /// <summary>
        /// Obtiene un administrador por su nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Administrador encontrado o null</returns>
        Administrator GetAdministratorByUsername(string username);

        /// <summary>
        /// Actualiza la información de un administrador
        /// </summary>
        /// <param name="administrator">Administrador con datos actualizados</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateAdministrator(Administrator administrator);

        /// <summary>
        /// Actualiza la contraseña de un administrador
        /// </summary>
        /// <param name="adminId">ID del administrador</param>
        /// <param name="newPassword">Nueva contraseña</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdatePassword(int adminId, string newPassword);

        /// <summary>
        /// Elimina un administrador
        /// </summary>
        /// <param name="adminId">ID del administrador a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteAdministrator(int adminId);

        /// <summary>
        /// Verifica si un nombre de usuario ya existe
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si existe, False en caso contrario</returns>
        bool AdministratorExists(string username);
    }
}