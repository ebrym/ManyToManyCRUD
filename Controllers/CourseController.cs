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
    public class CourseController : ControllerBase
    {
        
        private readonly ILogger<CourseController> _logger;
        private readonly ICourse _course;

        public CourseController(ILogger<CourseController> logger, ICourse course)
        {
            _logger = logger;
            _course = course;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _course.GetCourses();
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetById(int Id)
        {
            var result = await _course.GetCourse(Id);
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CourseRequestModel course)
        {
            var result = await _course.AddCourse(course);
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
        public async Task<IActionResult> Put([FromBody] CourseRequestModel course)
        {
            var result = await _course.Update(course);
            if (result)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest();
            }
        }


        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var result = await _course.DeleteCourse(Id);
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
