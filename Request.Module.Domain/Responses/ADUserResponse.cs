using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Module.Domain.Responses
{
    public class ADUserResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public Guid? ManagerId { get; set; }
        public ADUserResponse Manager { get; set; }
    }
}
