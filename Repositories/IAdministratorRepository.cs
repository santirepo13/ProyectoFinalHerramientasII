using System.Collections.Generic;
using CodeQuest.Models;

namespace CodeQuest.Repositories
{
    /// <summary>
    /// Interfaz que define las operaciones de acceso a datos para administradores
    /// Implementa el pilar de Abstracción de POO
    /// </summary>
    public interface IAdministratorRepository
    {
        /// <summary>
        /// Crea un nuevo administrador en la base de datos
        /// </summary>
        /// <param name="username">Nombre de usuario del administrador</param>
        /// <param name="password">Contraseña del administrador</param>
        /// <returns>ID del administrador creado</returns>
        /// <exception cref="System.Exception">Se lanza cuando no se puede crear el administrador</exception>
        int CreateAdministrator(string username, string password);

        /// <summary>
        /// Verifica si un administrador existe en la base de datos
        /// </summary>
        /// <param name="username">Nombre de usuario a verificar</param>
        /// <returns>True si el administrador existe, False en caso contrario</returns>
        bool AdministratorExists(string username);

        /// <summary>
        /// Obtiene el ID de un administrador por su nombre
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>ID del administrador o 0 si no existe</returns>
        int GetAdministratorId(string username);

        /// <summary>
        /// Obtiene un administrador completo por su ID
        /// </summary>
        /// <param name="adminId">ID del administrador</param>
        /// <returns>Objeto Administrator con toda la información o null si no existe</returns>
        Administrator GetAdministratorById(int adminId);

        /// <summary>
        /// Autentica un administrador por username y password
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <param name="password">Contraseña</param>
        /// <returns>Objeto Administrator si la autenticación es exitosa, null en caso contrario</returns>
        Administrator Authenticate(string username, string password);

        /// <summary>
        /// Actualiza la información de un administrador
        /// </summary>
        /// <param name="administrator">Administrador con la información actualizada</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdateAdministrator(Administrator administrator);

        /// <summary>
        /// Elimina un administrador de la base de datos
        /// </summary>
        /// <param name="adminId">ID del administrador a eliminar</param>
        /// <returns>True si se eliminó correctamente</returns>
        bool DeleteAdministrator(int adminId);

        /// <summary>
        /// Obtiene todos los administradores
        /// </summary>
        /// <returns>Lista de todos los administradores</returns>
        List<Administrator> GetAllAdministrators();

        /// <summary>
        /// Actualiza la contraseña de un administrador
        /// </summary>
        /// <param name="adminId">ID del administrador</param>
        /// <param name="newPassword">Nueva contraseña</param>
        /// <returns>True si se actualizó correctamente</returns>
        bool UpdatePassword(int adminId, string newPassword);

        /// <summary>
        /// Obtiene un administrador por su nombre de usuario
        /// </summary>
        /// <param name="username">Nombre de usuario</param>
        /// <returns>Objeto Administrator o null si no existe</returns>
        Administrator GetAdministratorByUsername(string username);
    }
}