using System.Collections.Generic;

namespace UserManagement.PL.ViewModels
{
    public class UserRolesViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public List<RoleSelectionViewModel> Roles { get; set; } = new List<RoleSelectionViewModel>();

    }
}
