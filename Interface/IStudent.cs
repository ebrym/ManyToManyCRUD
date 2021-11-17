using AndelaInterview.Api.Entity;
using AndelaInterview.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaInterview.Api.Interface
{
   public interface IStudent
    {
        Task<bool> Add(StudentRequestModel student);
        Task<IEnumerable<StudentResponseModel>> GetStudents();
        Task<StudentResponseModel> GetById(int Id);
        Task<bool> Delete(int Id);
        Task<bool> Update(StudentRequestModel student);
    }
}
