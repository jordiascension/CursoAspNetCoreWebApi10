using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using School.Application.Contracts;
using School.Models;

using static School.Apis.Contracts.StudentDtos;

namespace School.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IUnitOfWork _uow;

        public StudentController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<StudentDto>>> GetAll(CancellationToken ct)
        {
            //https://localhost:7245/api/student
            var repo = _uow.Repository<Student>();
            var students = await repo.GetAllAsync(ct);

            var result = students
                .Select(s => new StudentDto(s.Id, s.FirstName, s.LastName, s.DateOfBirth))
                .ToList();

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<StudentDto>> GetById(int id, CancellationToken ct)
        {
            //https://localhost:7245/api/student/1
            var repo = _uow.Repository<Student>();
            var student = await repo.GetByIdAsync(id, ct);

            if (student is null) return NotFound();

            return Ok(new StudentDto(student.Id, student.FirstName, student.LastName, student.DateOfBirth));
        }

        /*
        ASP.NET Core usa System.Text.Json, que solo garantiza 
        convertir fechas en formato ISO-8601.
        ISO-8601 usa este orden:
        YYYY-MM-DD
        Ejemplo:
        Año → 2002
        Mes → 05
        Día → 15
        Esto es universal y sin ambigüedad. 
        El formato del Student en json debería de ser así:
        {
           "firstName": "John",
           "lastName": "Doe",
           "dateOfBirth": "2003-05-01"
        }
        ✔️ 400 si fecha inválida
        ✔️ 201 si se crea
        */
        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create([FromBody] StudentCreateDto dto,
                                                        CancellationToken ct)
        {
            // Validación básica: DOB no puede ser futura
            if (dto.DateOfBirth.Date > DateTime.UtcNow.Date)
                return BadRequest("DateOfBirth cannot be in the future.");

            var repo = _uow.Repository<Student>();

            var student = new Student
            {
                FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? null : dto.FirstName.Trim(),
                LastName = string.IsNullOrWhiteSpace(dto.LastName) ? null : dto.LastName.Trim(),
                DateOfBirth = dto.DateOfBirth.Date
            };

            await repo.AddAsync(student, ct);
            await _uow.SaveChangesAsync(ct);

            var outDto = new StudentDto(student.Id, student.FirstName, student.LastName, student.DateOfBirth);

            return CreatedAtAction(nameof(GetById), new { id = student.Id }, outDto);
        }

        /*
         * Puede devolver:
        ✔️ 404 si no existe
        ✔️ 204 si se actualiza
        ✔️ 400 si fecha inválida
         */
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] StudentUpdateDto dto, CancellationToken ct)
        {
            if (dto.DateOfBirth.Date > DateTime.UtcNow.Date)
                return BadRequest("DateOfBirth cannot be in the future.");

            var repo = _uow.Repository<Student>();
            var student = await repo.GetByIdAsync(id, ct);

            if (student is null) return NotFound();

            student.FirstName = string.IsNullOrWhiteSpace(dto.FirstName) ? null : dto.FirstName.Trim();
            student.LastName = string.IsNullOrWhiteSpace(dto.LastName) ? null : dto.LastName.Trim();
            student.DateOfBirth = dto.DateOfBirth.Date;

            repo.Update(student);
            await _uow.SaveChangesAsync(ct);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var repo = _uow.Repository<Student>();
            var student = await repo.GetByIdAsync(id, ct);

            if (student is null) return NotFound();

            repo.Remove(student);
            await _uow.SaveChangesAsync(ct);

            return NoContent();
        }
    }
}
