using AndelaInterview.Api.Entity;
using AndelaInterview.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaInterview.Api.Interface
{
   public  interface ICourse
    {
        Task<bool> AddCourse(CourseRequestModel course);
        Task<IEnumerable<CourseResponseModel>> GetCourses();
        Task<CourseResponseModel> GetCourse(int Id);
        Task<bool> DeleteCourse(int Id);
        Task<bool> Update(CourseRequestModel course);
    }
}
