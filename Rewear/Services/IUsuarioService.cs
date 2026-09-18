using Rewear.Models;

namespace Rewear.Services
{
    public interface IUsuarioService
    {
        Task<List<Usuario>> ObtenerTodosAsync();
        Task<Usuario?> ObtenerPorIdAsync(int id);
        Task<Usuario?> ObtenerParaEditarAsync(int id);

        Task<Usuario?> CrearAsync(
            Usuario usuario,
            IFormFile? fotoPerfil);

        Task<bool> ActualizarAsync(
            int id,
            Usuario usuario,
            IFormFile? fotoPerfil);

        Task<bool> EliminarAsync(int id);

        Task<bool> ExisteAsync(int id);

        Task<bool> CambiarContrasenaAsync(
            int id,
            string nuevaContrasena);
    }
}