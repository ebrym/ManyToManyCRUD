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
    public class StudentRepository : IStudent
    {
        private readonly DataContext _context;
        public StudentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> Add(StudentRequestModel student)
        {

            var _student = new Student
            {
                Address = student.Address,
                Name = student.Name,
                Phone = student.Phone
            };

            var studentCourses = new List<StudentCourse>();

            if (student.CourseId != null)
            {
                foreach (var item in student.CourseId)
                {
                    var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == item);

                    if (course != null)
                    {
                        var studentCourse = new StudentCourse
                        {
                            Course = course,
                            Student = _student
                        };
                        studentCourses.Add(studentCourse);
                    }
                }

                _student.StudentCourses = studentCourses;
            }
            

            await _context.Students.AddAsync(_student);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<StudentResponseModel>> GetStudents()
        {
            var query = await _context.Students
                  .Select(a => new StudentResponseModel
                  {
                      Id= a.Id,
                      Name = a.Name,
                      Address = a.Address,
                      Phone = a.Phone,
                      Course = a.StudentCourses.Where(x=> x.StudentId == a.Id)
                                  .Select(c => new CourseModel
                                  {
                                      Code = c.Course.Code,
                                      Name = c.Course.Name,
                                  })
                                  .ToList(),
                  }).ToListAsync();

            return query;
        }
        public async Task<StudentResponseModel> GetById(int Id)
        {

            var query = await _context.Students
                  .Where(x => x.Id == Id)
                  .Select(s => new StudentResponseModel
                  {
                      Name = s.Name,
                      Address = s.Address,
                      Phone = s.Phone,
                      Course = _context.StudentCourses
                                  .Where(x => x.StudentId == s.Id)
                                  .Select(c => new CourseModel
                                  {
                                      Code = c.Course.Code,
                                      Name = c.Course.Name,
                                  })
                                  .ToList(),
                  }).FirstOrDefaultAsync();



            return query;
        }
        public async Task<bool> Delete(int Id)
        {
            var student = await _context.Students
                        .Where(x => x.Id == Id)
                        .Include(sc => sc.StudentCourses)
                        .FirstOrDefaultAsync();

            _context.Remove(student);
            _context.SaveChanges();

            return true;
        }

        public async Task<bool> Update(StudentRequestModel student)
        {
            var _student = await _context.Students
                        .Where(x => x.Id == student.StudentId)
                        .FirstOrDefaultAsync();

            _student.Name = student.Name;
            _student.Address = student.Address;
            _student.Phone = student.Phone;


            if (_student != null)
            {
                // student not mapped to course
                var studentCourses = new List<StudentCourse>();


                foreach (var item in student.CourseId)
                {
                    var course = await _context.Courses.FirstOrDefaultAsync(x => x.Id == item);
                    var studentCourse = await _context.StudentCourses.FirstOrDefaultAsync(x => x.StudentId == student.StudentId && x.CourseId == item);

                    if (course != null && studentCourse == null)
                    {
                        var newStudentCourse = new StudentCourse
                        {
                            Student = _student,
                            Course = course
                        };
                        studentCourses.Add(newStudentCourse);
                    }

                }
                _student.StudentCourses = studentCourses;
            }
            // _context.Remove(course);
            _context.SaveChanges();

            return true;
        }
    }
}
