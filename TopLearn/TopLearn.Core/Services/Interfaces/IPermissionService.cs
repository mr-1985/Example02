using System;
using System.Collections.Generic;
using System.Text;
using TopLearn.DataLayer.Entities.Permissions;
using TopLearn.DataLayer.Entities.User;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IPermissionService
    {
        #region Role

        List<Role> GetRoles();
        void AddRolesToUser(List<int> roleIds, int userId);
        int AddRole(Role role);
        Role GetRoleById(int roleId);
        void DeleteRole(Role role);
        void UpdateRole(Role role);
        void EditRolesUser(int userId, List<int> rolesId);
        List<Permission> GetAllPermissions();
        void AddPermission(int roleId, List<int> permissions);
        List<int> PermissionsRole(int roleId);
        void EditRolePermission(int roleId, List<int> permissions);
        bool CheckPermission(int permissionId, string userName);

        #endregion
    }
}
