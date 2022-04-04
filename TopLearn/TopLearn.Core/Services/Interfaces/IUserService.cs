using System;
using System.Collections.Generic;
using TopLearn.Core.DTOs;
using System.Text;
using TopLearn.DataLayer.Entities.User;
using TopLearn.DataLayer.Entities.Wallet;

namespace TopLearn.Core.Services.Interfaces
{
    public interface IUserService
    {
        bool IsExistUserName(string userName);
        bool IsExistEmail(string email);
        int AddUser(User user);
        User LoginUser(LoginViewModel login);
        bool ActiveAccount(string activeCode);
        User GetUserByEmail(string email);
        User GetUserByUserName(string userName);
        User GetUserById(int userId);
        int GetUserIdByUserName(string userName);
        User GetUserByActiveCode(string activeCode);
        void UpdateUser(User user);

        #region User Panel

        InformationUserViewModel GetUserInformation(string userName);
        InformationUserViewModel GetUserInformation(int userId);
        SideBarDataForUserPanelViewModel GetDataForSideBar(string userName);
        EditUserProfileViewModel GetDataForEditProfile(string userName);
        void EditProfile(string userName, EditUserProfileViewModel profile);
        bool CompareOldPassword(string userName, string oldPassword);
        void ChangeUserPassword(string userName, string newPassword);


        #endregion

        #region Wallet

        int BalanceUserWallet(string userName);
        List<WalletViewModel> GetWalletUser(string userName);
        void ChargeWallet(string userName, int amount, string description, bool isPay = false);
        void AddWallet(Wallet wallet);

        #endregion

        #region Admin Panel

        UsersForAdminViewModel GetAllUsers(int pageId = 1, string filterUserName = "", string filterEmail = "");
        UsersForAdminViewModel GetDeleteUsers(int pageId = 1, string filterUserName = "", string filterEmail = "");
        int AddUserFromAdmin(CreateUserViewModel user);
        EditUserViewModel GetUserForShowInEditMode(int userId);
        void EditUserFromAdmin(EditUserViewModel editUser);
        void DeleteUser(int userId);

        #endregion
    }
}
