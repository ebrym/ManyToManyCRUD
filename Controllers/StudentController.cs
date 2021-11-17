using AndelaInterview.Api.Interface;
using AndelaInterview.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaInterview.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class StudentController : ControllerBase
    {
        
        private readonly ILogger<StudentController> _logger;
        private readonly IStudent _student;

        public StudentController(ILogger<StudentController> logger, IStudent student)
        {
            _logger = logger;
            _student = student;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _student.GetStudents();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] StudentRequestModel student)
        {
            var result = await _student.Add(student);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetStudentById(int Id)
        {
            var result = await _student.GetById(Id);

            return Ok(result);

        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _student.Delete(Id);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] StudentRequestModel student)
        {
            var result = await _student.Update(student);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
