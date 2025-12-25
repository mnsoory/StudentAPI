using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.API.Attributes;
using StudentAPI.Application.DTOs;
using StudentAPI.Application.Features.Students.Commands;
using StudentAPI.Application.Features.Students.Queries;
using StudentAPI.Domain.Entities;
using StudentAPI.Domain.Enums;
using System.Threading.Tasks;

namespace StudentAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public StudentsController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet(Name = "AllStudents")]
        [CheckPermission(Permission.GetStudent)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllStudents()
        {
            var query = new GetStudentsQuery();

            var students = await _mediator.Send(query);

            return Ok(students);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("Passed", Name = "PassedStudents")]
        [CheckPermission(Permission.GetStudent)]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetPassedStudents()
        {
            var query = new GetPassedStudentsQuery();
            var passedStudents = await _mediator.Send(query);

            return Ok(passedStudents);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("AverageGrade", Name = "AverageGrade")]
        [CheckPermission(Permission.GetStudent)]
        public async Task<ActionResult<double>> GetAverageGrade()
        {
            var query = new GetAverageGradeQuery();
            double? averageGrade = await _mediator.Send(query);

            if(averageGrade == null)
            {
                return Ok(0);
            }

            return Ok(averageGrade);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}", Name = "StudentById")]
        [CheckPermission(Permission.GetStudent)]
        public async Task<ActionResult<StudentDto>> GetStudentById(int id)
        {
            if(id < 1)
                return BadRequest($"Not Accepted Id: {id}");

            var query = new GetStudentQuery { Id = id };
            var student = await _mediator.Send(query);

            if (student == null)
                return NotFound($"No student with Id: {id} was found");

            return Ok(student);
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost(Name = "AddStudent")]
        [CheckPermission(Permission.AddStudent)]
        public async Task<ActionResult<StudentDto>> AddStudent([FromBody] CreateStudentCommand command)
        {

            var newStudent = await _mediator.Send(command);
            var studentDto = _mapper.Map<StudentDto>(newStudent);

            return CreatedAtRoute("StudentById", new { id = newStudent.Id }, studentDto);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{Id}")]
        [CheckPermission(Permission.UpdateStudent)]
        public async Task<IActionResult> UpdateStudent(int Id, [FromBody] UpdateStudentCommand command)
        {
            if (Id != command.Id)
            {
                return BadRequest($"The ID: {Id} in the route must match the ID: {command.Id} in the request body.");
            }

            bool isUpdated = await _mediator.Send(command);

            if (!isUpdated)
                return NotFound($"No student with Id: {Id} was found");

            return NoContent();
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete("{id}", Name = "DeleteStudent")]
        [CheckPermission(Permission.DeleteStudent)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var command = new DeleteStudentCommand { Id = id };

            bool isDeleted = await _mediator.Send(command);

            if(!isDeleted)
                return NotFound($"No student with Id: {command.Id} was found");

            return NoContent();
        }
    }
}
