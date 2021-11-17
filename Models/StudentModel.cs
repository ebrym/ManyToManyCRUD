using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaInterview.Api.Models
{
    public class StudentModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
    }
    public class StudentRequestModel : StudentModel
    {
        public int StudentId { get; set; }
        public List<int> CourseId { get; set; }
    }
    public class StudentResponseModel : StudentModel
    {
        public int Id { get; set; }
        public List<CourseModel> Course { get; set; }
    }
}
