using Microsoft.AspNetCore.Mvc;
using Rewear.Models;
using Rewear.Services;

namespace Rewear.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();

            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService.ObtenerPorIdAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,Nombres,Apellidos,Correo,Contrasena,Telefono")]
            Usuario usuario,
            IFormFile? fotoPerfil)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var resultado = await _usuarioService
                .CrearAsync(usuario, fotoPerfil);

            if (resultado == null)
            {
                ModelState.AddModelError(
                    "fotoPerfil",
                    "La imagen no es válida. Solo se permiten JPG, JPEG, PNG o WEBP y un tamaño máximo de 5 MB.");

                return View(usuario);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService
                .ObtenerParaEditarAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,Nombres,Apellidos,Correo,Telefono")]
            Usuario usuario,
            IFormFile? fotoPerfil)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            ModelState.Remove(nameof(Usuario.Contrasena));

            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            var resultado = await _usuarioService
                .ActualizarAsync(id, usuario, fotoPerfil);

            if (!resultado)
            {
                if (!await _usuarioService.ExisteAsync(id))
                {
                    return NotFound();
                }

                ModelState.AddModelError(
                    "fotoPerfil",
                    "La imagen no es válida. Solo se permiten JPG, JPEG, PNG o WEBP y un tamaño máximo de 5 MB.");

                return View(usuario);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService
                .ObtenerPorIdAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resultado = await _usuarioService
                .EliminarAsync(id);

            if (!resultado)
            {
                return NotFound();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/CambiarContrasena/5
        public async Task<IActionResult> CambiarContrasena(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _usuarioService
                .ObtenerPorIdAsync(id.Value);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/CambiarContrasena/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarContrasena(
            int id,
            string nuevaContrasena)
        {
            if (string.IsNullOrWhiteSpace(nuevaContrasena))
            {
                ModelState.AddModelError(
                    "nuevaContrasena",
                    "La nueva contraseña es obligatoria.");

                var usuarioError =
                    await _usuarioService.ObtenerPorIdAsync(id);

                if (usuarioError == null)
                {
                    return NotFound();
                }

                return View(usuarioError);
            }

            if (nuevaContrasena.Length < 4)
            {
                ModelState.AddModelError(
                    "nuevaContrasena",
                    "La contraseña debe tener al menos 4 caracteres.");

                var usuarioError =
                    await _usuarioService.ObtenerPorIdAsync(id);

                if (usuarioError == null)
                {
                    return NotFound();
                }

                return View(usuarioError);
            }

            var resultado = await _usuarioService
                .CambiarContrasenaAsync(id, nuevaContrasena);

            if (!resultado)
            {
                return NotFound();
            }

            return RedirectToAction(
                nameof(Edit),
                new { id });
        }
    }
}