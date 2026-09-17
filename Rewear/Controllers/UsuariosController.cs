using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rewear.Data;
using Rewear.Models;

namespace Rewear.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Usuarios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Usuarios.ToListAsync());
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
            if (usuario == null)
            {
                return NotFound(); //this one to Business Layer
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
            [Bind("Id,Nombres,Apellidos,Correo,Contrasena,Telefono")] Usuario usuario,
            IFormFile? fotoPerfil)
        {
            if (ModelState.IsValid)
            {
                // Asignar automáticamente los datos del sistema
                usuario.FechaRegistro = DateTime.Now;
                usuario.Estado = true;

                // Si se seleccionó una foto
                if (fotoPerfil != null && fotoPerfil.Length > 0)
                {
                    // Formatos permitidos
                    var extensionesPermitidas = new[] { ".jpg", ".jpeg", ".png", ".webp" };

                    string extension = Path.GetExtension(fotoPerfil.FileName)
                        .ToLowerInvariant();

                    // Validar extensión
                    if (!extensionesPermitidas.Contains(extension))
                    {
                        ModelState.AddModelError(
                            "fotoPerfil",
                            "Solo se permiten imágenes JPG, JPEG, PNG o WEBP."
                        );

                        return View(usuario);
                    }

                    // Validar tamaño máximo: 5 MB
                    const long tamanoMaximo = 5 * 1024 * 1024;

                    if (fotoPerfil.Length > tamanoMaximo)
                    {
                        ModelState.AddModelError(
                            "fotoPerfil",
                            "La imagen no puede superar los 5 MB."
                        );

                        return View(usuario);
                    }

                    // Carpeta donde se guardarán las fotos
                    string carpeta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        "perfiles"
                    );

                    // Crear la carpeta si no existe
                    if (!Directory.Exists(carpeta))
                    {
                        Directory.CreateDirectory(carpeta);
                    }

                    // Generar un nombre único
                    string nombreArchivo = Guid.NewGuid().ToString() + extension;

                    string rutaArchivo = Path.Combine(carpeta, nombreArchivo);

                    // Guardar la imagen
                    using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                    {
                        await fotoPerfil.CopyToAsync(stream);
                    }

                    // Guardar solamente la ruta en la BD
                    usuario.FotoPerfil = "/uploads/perfiles/" + nombreArchivo;
                }

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }


        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
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
            [Bind("Id,Nombres,Apellidos,Correo,Telefono")] Usuario usuario,
            IFormFile? fotoPerfil)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            // La contraseña se modifica en una pantalla separada.
            // Por eso no debe validarse en este formulario.
            ModelState.Remove(nameof(Usuario.Contrasena));

            if (ModelState.IsValid)
            {
                // Buscar el usuario original en la base de datos
                var usuarioExistente = await _context.Usuarios.FindAsync(id);

                if (usuarioExistente == null)
                {
                    return NotFound();
                }

                // ==========================================
                // ACTUALIZAR DATOS DEL USUARIO
                // ==========================================

                usuarioExistente.Nombres = usuario.Nombres;
                usuarioExistente.Apellidos = usuario.Apellidos;
                usuarioExistente.Correo = usuario.Correo;
                usuarioExistente.Telefono = usuario.Telefono;

                // La contraseña NO se modifica aquí.
                // Se conserva la que ya existe en la base de datos.


                // ==========================================
                // ACTUALIZAR FOTO DE PERFIL
                // ==========================================

                if (fotoPerfil != null && fotoPerfil.Length > 0)
                {
                    // Extensiones permitidas
                    var extensionesPermitidas = new[]
                    {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

                    string extension = Path.GetExtension(fotoPerfil.FileName)
                        .ToLowerInvariant();

                    // Validar formato
                    if (!extensionesPermitidas.Contains(extension))
                    {
                        ModelState.AddModelError(
                            "fotoPerfil",
                            "Solo se permiten imágenes JPG, JPEG, PNG o WEBP."
                        );

                        return View(usuario);
                    }

                    // Validar tamaño máximo: 5 MB
                    const long tamanoMaximo = 5 * 1024 * 1024;

                    if (fotoPerfil.Length > tamanoMaximo)
                    {
                        ModelState.AddModelError(
                            "fotoPerfil",
                            "La imagen no puede superar los 5 MB."
                        );

                        return View(usuario);
                    }

                    // ==========================================
                    // CREAR CARPETA
                    // ==========================================

                    string carpeta = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "wwwroot",
                        "uploads",
                        "perfiles"
                    );

                    if (!Directory.Exists(carpeta))
                    {
                        Directory.CreateDirectory(carpeta);
                    }

                    // ==========================================
                    // GENERAR NOMBRE ÚNICO
                    // ==========================================

                    string nombreArchivo =
                        Guid.NewGuid().ToString() + extension;

                    string rutaArchivo =
                        Path.Combine(carpeta, nombreArchivo);

                    // ==========================================
                    // GUARDAR NUEVA FOTO
                    // ==========================================

                    using (var stream = new FileStream(
                        rutaArchivo,
                        FileMode.Create))
                    {
                        await fotoPerfil.CopyToAsync(stream);
                    }

                    // ==========================================
                    // ELIMINAR FOTO ANTERIOR
                    // ==========================================

                    if (!string.IsNullOrEmpty(usuarioExistente.FotoPerfil))
                    {
                        string rutaFotoAnterior = Path.Combine(
                            Directory.GetCurrentDirectory(),
                            "wwwroot",
                            usuarioExistente.FotoPerfil.TrimStart('/')
                        );

                        if (System.IO.File.Exists(rutaFotoAnterior))
                        {
                            System.IO.File.Delete(rutaFotoAnterior);
                        }
                    }

                    // ==========================================
                    // GUARDAR RUTA DE LA NUEVA FOTO
                    // ==========================================

                    usuarioExistente.FotoPerfil =
                        "/uploads/perfiles/" + nombreArchivo;
                }

                // ==========================================
                // GUARDAR CAMBIOS
                // ==========================================

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await UsuarioExiste(usuarioExistente.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(usuario);
        }



        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
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
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UsuarioExiste(int id)
        {
            return await _context.Usuarios.AnyAsync(e => e.Id == id);
        }

        // GET: Usuarios/CambiarContrasena/5
        public async Task<IActionResult> CambiarContrasena(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

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
                    "La nueva contraseña es obligatoria."
                );

                var usuarioError = await _context.Usuarios.FindAsync(id);

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
                    "La contraseña debe tener al menos 4 caracteres."
                );

                var usuarioError = await _context.Usuarios.FindAsync(id);

                if (usuarioError == null)
                {
                    return NotFound();
                }

                return View(usuarioError);
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Contrasena = nuevaContrasena;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Edit), new { id = usuario.Id });
        }
    }
}
