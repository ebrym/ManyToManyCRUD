using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AndelaInterview.Api.Models
{
    public class CourseModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
    }
    public class CourseRequestModel : CourseModel
    {
        public int CourseId { get; set; }
        public List<int> StudentId { get; set; }
    }
    public class CourseResponseModel : CourseModel
    {
        public int Id { get; set; }
        public List<StudentModel> Student { get; set; }
    }
}
