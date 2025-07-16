using System;
using System.Collections.Generic;

namespace UserManagement.PL.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public UserViewModel() { 
            Id = Guid.NewGuid().ToString();
        }
    }
}
