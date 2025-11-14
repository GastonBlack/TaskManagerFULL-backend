using ToDoAppAPI.Data;
using ToDoAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ToDoAppAPI.Services
{
    public class TaskService
    {
        private readonly AppDbContext _context; // Se guarda el contexto de la DB.

        public TaskService(AppDbContext context) // Se recibe por inyeccion.
        {
            _context = context;
        }

        // ========================================
        // GET ALL TASKS.
        // ========================================
        public async Task<IEnumerable<ToDoTask>> GetAllAsync()
        {
            return await _context.Tasks.ToListAsync();
        }

        // ========================================
        // GET TASK BY ID
        // ========================================
        public async Task<ToDoTask> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id); // Es un SELECT * FROM Tasks where Id = ...
        }

        // ========================================
        // CREATE A NEW TASK.
        // ========================================
        public async Task<ToDoTask> CreateAsync(ToDoTask task) // Recibe una tarea como parametro.
        {
            _context.Tasks.Add(task); // INSERT a _context(DB).
            await _context.SaveChangesAsync(); // Ejecuta el SQL.
            return task;
        }

        // ========================================
        // UPDATE TASK.
        // ========================================
        public async Task<ToDoTask?> UpdateAsync(ToDoTask updated)
        {
            var existing = await _context.Tasks.FindAsync(updated.Id);
            if (existing == null)
                return null;

            existing.Content = updated.Content;
            existing.Deadline = updated.Deadline;
            existing.IsCompleted = updated.IsCompleted;

            await _context.SaveChangesAsync();
            return existing;
        }

        // ========================================
        // DELETE TASK.
        // ========================================
        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        // ========================================
        // DELETE ALL TASKS.
        // ========================================
        public async Task DeleteAllTasksAsync()
        {
            _context.Tasks.RemoveRange(_context.Tasks);
            await _context.SaveChangesAsync();
        }

        // ========================================
        // MARK TASK AS COMPLETED.
        // ========================================
        public async Task<ToDoTask?> ToggleAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return null;

            task.IsCompleted = !task.IsCompleted;
            await _context.SaveChangesAsync();
            return task;
        }

        // ========================================
        // LOAD WELCOME NOTE.
        // ========================================
        public async Task<ToDoTask?> LoadWelcomeAsync()
        {
            // SI ya hay tareas no se agrega nada.
            if (await _context.Tasks.AnyAsync())
                return null;

            var welcomeTask = new ToDoTask
            {
                Content = "Bienvenido a TaskManager, crea tu primera tarea!",
                Deadline = null,
                IsCompleted = false
            };

            _context.Tasks.Add(welcomeTask);
            await _context.SaveChangesAsync();
            return welcomeTask;
        }

        // ========================================
        // LOAD TEST NOTES.
        // ========================================
        public async Task<IEnumerable<ToDoTask>> LoadTestTasksAsync()
        {
            // Limpiar todas las tareas existentes.
            _context.Tasks.RemoveRange(_context.Tasks);
            await _context.SaveChangesAsync();

            var testTasks = new List<ToDoTask>
            {
                new ToDoTask("Comprar pan y leche antes de las 18:00", new DateTime(2025, 11, 7), false),
                new ToDoTask("Llamar al banco para consultar sobre la tarjeta de crédito y preguntar si se puede extender el límite actual.", new DateTime(2025, 11, 9), false),
                new ToDoTask("Limpiar la casa y ordenar el escritorio. Aprovechar para revisar papeles viejos, tirar lo que no sirva y dejar todo listo para el fin de semana.", new DateTime(2025, 11, 10), false),
                new ToDoTask("Enviar el regalo de cumpleaños a Ana. Revisar que el paquete esté bien embalado, incluir una nota escrita a mano y confirmar la dirección con ella antes de mandarlo por correo.", null, true),
                new ToDoTask("Ver la película pendiente del fin de semana", new DateTime(2025, 11, 12), false),
                new ToDoTask("Pagar el recibo de luz y agua antes del jueves. Revisar también si el gas está al día.", null, false),
                new ToDoTask("Terminar el libro 'El nombre del viento'. Leer al menos dos capítulos antes de dormir y anotar frases interesantes en el cuaderno nuevo que compré.", new DateTime(2025, 11, 14), false),
                new ToDoTask("Actualizar el currículum y revisar ofertas laborales.", new DateTime(2025, 11, 15), false),
                new ToDoTask("Comprar comida para el gato y arena para el arenero. Recordar probar la nueva marca que recomendó el veterinario la última vez.", null, false),
                new ToDoTask("Llevar el auto al mecánico para revisión.", new DateTime(2025, 11, 17), true),
                new ToDoTask("Preparar la mochila para el viaje del fin de semana.", new DateTime(2025, 11, 18), false),
                new ToDoTask("Mandar correo a Pedro para coordinar la cena del viernes. Confirmar lugar, hora y si va a traer vino o postre. Ver si puedo invitar también a Clara.", new DateTime(2025, 11, 19), false),
                new ToDoTask("Lavar la ropa blanca y planchar la camisa azul que quiero usar el sábado. Si sobra tiempo, organizar el armario y separar la ropa que ya no uso para donar.", new DateTime(2025, 11, 20), true),
                new ToDoTask("Anotar ideas para el nuevo proyecto personal. Estuve pensando en hacer un blog donde pueda subir fotos de los lugares que visito, junto con pequeños textos sobre cómo me siento en cada lugar. Tal vez usar una plantilla minimalista en WordPress.", new DateTime(2025, 11, 21), false),
                new ToDoTask("Comprar flores para el cumpleaños de mamá. Buscar un ramo con lirios o margaritas.", new DateTime(2025, 11, 22), false),
                new ToDoTask("Sacar turno con el dentista para control anual. El último fue hace casi un año y ya toca. Preguntar si hay disponibilidad los martes después de las 17:00.", new DateTime(2025, 11, 23), false),
                new ToDoTask("Revisar las fotos viejas y hacer una copia de seguridad en el disco externo. Organizar las carpetas por año y borrar duplicados. Capaz subir algunas al drive para no perderlas si el disco se rompe.", null, true),
                new ToDoTask("Organizar una cena con amigos. Elegir una fecha donde todos puedan, preparar una lista de comidas fáciles (tacos o pizza casera) y hacer una lista de reproducción con música tranquila. Tal vez comprar velas para ambientar.", new DateTime(2025, 11, 25), false),
                new ToDoTask("Aprender a cocinar algo nuevo esta semana. Capaz intentar hacer lasaña desde cero o un curry tailandés.", new DateTime(2025, 11, 26), false),
                new ToDoTask("Tomar un día libre para descansar y desconectar del teléfono. Salir a caminar por la rambla, leer algo sin interrupciones, y dejar el celular en modo avión al menos por una mañana. La idea es descansar de verdad, no solo cambiar de pantalla.", new DateTime(2025, 11, 27), false)
            };
            _context.Tasks.AddRange(testTasks);
            await _context.SaveChangesAsync();

            return testTasks;
        }
    }
}