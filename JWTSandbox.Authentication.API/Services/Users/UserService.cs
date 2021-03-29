using JWTSandbox.Authentication.API.Services.Audiences;
using JWTSandbox.Authentication.API.Services.Models;
using System.Collections.Generic;
using System.Linq;

namespace JWTSandbox.Authentication.API.Services.Users
{
    public interface IUserService
    {
        bool UserExist(string loginName, string pass, string audienceId);
        User FindUserByUserId(int userId);
        User FindUserByLoginName(string loginName);
    }

    public class UserService : IUserService
    {
        private List<User> _userList = new List<User>();

        public UserService()
        {
            //fetch this from the db
            _userList.Add(new User {
                        Id = 1,
                        FirstName = "Test1",
                        LastName = "Test1",
                        Password = "Test1",
                        Username = "Test1",
                        AudienceId = AudienceService.AUDIENCE_1_ID
                    });
        }

        public User FindUserByLoginName(string loginName)
        {
            return _userList.FirstOrDefault(x => x.Username == loginName);
        }

        public User FindUserByUserId(int userId)
        {
            return _userList.FirstOrDefault(x => x.Id == userId);
        }

        public bool UserExist(string loginName, string pass, string audienceId)
        {
            return _userList.Any(x => x.Username == loginName && x.Password == pass && x.AudienceId == audienceId);
        }
    }
}
