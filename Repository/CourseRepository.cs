using AndelaInterview.Api.Domain;
using AndelaInterview.Api.Entity;
using AndelaInterview.Api.Interface;
using AndelaInterview.Api.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaInterview.Api.Repository
{
    public class CourseRepository : ICourse
    {
        private readonly DataContext _context;
        public CourseRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCourse(CourseRequestModel course)
        {

            var _course = new Course
            {
                Code = course.Code,
                Name = course.Name
            };

            var studentCourses = new List<StudentCourse>();

            if (course.StudentId != null)
            {
                foreach (var item in course.StudentId)
                {
                    var student = await _context.Students.FirstOrDefaultAsync(x => x.Id==item);

                    if (student != null)
                    {
                        var studentCourse = new StudentCourse
                        {
                            Student = student,
                            Course = _course
                        };
                        studentCourses.Add(studentCourse);
                    }

                }

                _course.StudentCourses = studentCourses;
            }
            

            await _context.Courses.AddAsync(_course);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CourseResponseModel>> GetCourses()
        {

            var query = await _context.Courses
                  .Select(a => new CourseResponseModel
                  {
                      Id = a.Id,
                      Code = a.Code,
                      Name = a.Name,
                      Student = _context.StudentCourses
                                  .Where(x=> x.CourseId == a.Id)
                                  .Select(s => new StudentModel
                                  {
                                      Name =  s.Student.Name,
                                      Address = s.Student.Address,
                                      Phone = s.Student.Phone,
                                  })
                                  .ToList(),
                  }).ToListAsync();



            return query;
        }
        public async Task<CourseResponseModel> GetCourse(int Id)
        {

            var query = await _context.Courses
                  .Where(x => x.Id == Id)
                  .Select(a => new CourseResponseModel
                  {
                      Id = a.Id,
                      Code = a.Code,
                      Name = a.Name,
                      Student = _context.StudentCourses
                                  .Where(x => x.CourseId == a.Id)
                                  .Select(s => new StudentModel
                                  {
                                      Name = s.Student.Name,
                                      Address = s.Student.Address,
                                      Phone = s.Student.Phone,
                                  })
                                  .ToList(),
                  }).FirstOrDefaultAsync();



            return query;
        }
        public async Task<bool> DeleteCourse(int Id)
        {
            var course = await _context.Courses
                        .Where(x => x.Id == Id)
                        .Include(sc=> sc.StudentCourses)
                        .FirstOrDefaultAsync();
            if(course != null)
            {
                _context.Remove(course);
                _context.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> Update(CourseRequestModel course)
        {
            var _course = await _context.Courses
                        .Where(x => x.Id == course.CourseId)
                        .FirstOrDefaultAsync();

            _course.Name = course.Name;
            _course.Code = course.Code;


            if (_course != null)
            {
                // student not mapped to course
                var studentCourses = new List<StudentCourse>();           


                    foreach (var item in course.StudentId)
                    {
                        var student = await _context.Students.FirstOrDefaultAsync(x => x.Id == item);
                        var studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(x => x.CourseId == course.CourseId && x.StudentId == item);

                        if (student != null && studentCourse == null)
                        {
                            var newStudentCourse = new StudentCourse
                            {
                                Student = student,
                                Course = _course
                            };
                            studentCourses.Add(newStudentCourse);
                        }

                    }
                    _course.StudentCourses = studentCourses;            
            }
               // _context.Remove(course);
            _context.SaveChanges();

            return true;
        }

    }
}
