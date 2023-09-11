namespace ApiCamisetas.Controllers
{
    using ApiCamisetas.Models;
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json;

    [Route("api/[controller]")]
    [ApiController]
    public class CamisetasController : ControllerBase
    {
        private readonly string _binPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Json", "Camisetas.json");
        private async Task<List<Camiseta>> GetCamisetasListAsync()
        {
            if (System.IO.File.Exists(_binPath))
            {
                var json = await System.IO.File.ReadAllTextAsync(_binPath);
                return JsonConvert.DeserializeObject<List<Camiseta>>(json) ?? new List<Camiseta>();
            }
            return new List<Camiseta>();
        }

        [HttpPost]
        private async Task SaveCamisetasListAsync(List<Camiseta> camisetas)
        {
            var directory = Path.GetDirectoryName(_binPath);
            if (directory == null)
            {
                throw new InvalidOperationException("El directorio no puede ser nulo.");
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonConvert.SerializeObject(camisetas);
            await System.IO.File.WriteAllTextAsync(_binPath, json);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Camiseta>>> GetCamisetas()
        {
            var camisetas = await GetCamisetasListAsync();
            if (!camisetas.Any() && !System.IO.File.Exists(_binPath))
            {
                return new BadRequestObjectResult("El archivo Camisetas.bin no se encuentra creado, es necesario crearlo antes de poder usarlo.");
            }
            return new OkObjectResult(camisetas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Camiseta>> GetCamiseta(int id)
        {
            if (!System.IO.File.Exists(_binPath))
            {
                return new BadRequestObjectResult("El archivo Camisetas.bin no se encuentra creado, es necesario crearlo antes de poder usarlo.");
            }

            var camisetas = await GetCamisetasListAsync();
            var camiseta = camisetas.FirstOrDefault(c => c.ID == id);
            if (camiseta != null)
            {
                return new OkObjectResult(camiseta);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Camiseta>> PostCamiseta(Camiseta camiseta)
        {
            var camisetas = await GetCamisetasListAsync();
            camisetas.Add(camiseta);
            await SaveCamisetasListAsync(camisetas);

            return CreatedAtAction("GetCamiseta", new { id = camiseta.ID }, camiseta);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCamiseta(int id, Camiseta camiseta)
        {
            if (id != camiseta.ID)
            {
                return BadRequest();
            }

            if (!System.IO.File.Exists(_binPath))
            {
                return new BadRequestObjectResult("El archivo Camisetas.bin no se encuentra creado, es necesario crearlo antes de poder usarlo.");
            }

            var camisetas = await GetCamisetasListAsync();
            var existingCamiseta = camisetas.FirstOrDefault(c => c.ID == id);

            if (existingCamiseta == null)
            {
                return NotFound();
            }

            camisetas.Remove(existingCamiseta);
            camisetas.Add(camiseta);
            await SaveCamisetasListAsync(camisetas);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCamiseta(int id)
        {
            if (!System.IO.File.Exists(_binPath))
            {
                return new BadRequestObjectResult("El archivo Camisetas.bin no se encuentra creado, es necesario crearlo antes de poder usarlo.");
            }

            var camisetas = await GetCamisetasListAsync();
            var camiseta = camisetas.FirstOrDefault(c => c.ID == id);

            if (camiseta == null)
            {
                return NotFound();
            }

            camisetas.Remove(camiseta);
            await SaveCamisetasListAsync(camisetas);

            return NoContent();
        }


    }
}
