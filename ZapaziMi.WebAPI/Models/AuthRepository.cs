using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApplicationppp.Models;

namespace WebApplicationppp
{
    public class AuthRepository : IDisposable
    {
        private AuthContext _ctx;

        private UserManager<IdentityUser> _userManager;

        private RoleManager<IdentityRole> _roleManager;

        public AuthRepository()
        {
            _ctx = new AuthContext();
            _userManager = new UserManager<IdentityUser>(new UserStore<IdentityUser>(_ctx));
            _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_ctx));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel, string roleName)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName,
                Email = userModel.UserName
            };

            var result = await _userManager.CreateAsync(user, userModel.Password);

            if (result == IdentityResult.Success)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    role = new IdentityRole(roleName);
                    var roleresult = await _roleManager.CreateAsync(role);
                }

                var userFromDb = await _userManager.FindByNameAsync(user.UserName);

                if (userFromDb != null)
                {
                    await _userManager.AddToRoleAsync(userFromDb.Id, roleName);
                }
            }

            return result;
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            IdentityUser user = await _userManager.FindAsync(userName, password);

            return user;
        }

        public async Task<List<string>> GetRolesForUser(string userId)
        {
            var roles = await _userManager.GetRolesAsync(userId) as List<string>;

            return roles;
        }

        public async Task<List<string>> GetRolesForUserByName(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user.Id) as List<string>;

                return roles;
            }
            else
            {
                return null;
            }
        }

        public async Task<IdentityResult> ChangeUserPassword(string username, string oldPass, string newPass)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                IdentityResult result = await _userManager.ChangePasswordAsync(user.Id, oldPass, newPass);

                return result;
            }
            else
            {
                return null;
            }
        }

        public List<UserDTO> GetAllUsers(string roleId)
        {
            var context = new AuthContext();
            var db = new DiplomnaEntities();

            var usersInRole = _roleManager.FindById(roleId).Users.Select(x => x.UserId).ToList();
            var roleName = _roleManager.FindById(roleId).Name;

            var usersForReturn = context.Users.Where(x => usersInRole.Contains(x.Id)).ToList().Select(x => new UserDTO
            {
                Id = x.Id,
                Email = x.Email,
                Role = roleName,
                SalonsCount = db.Salons.Count(y => y.CreateBy.ToLower() == x.Email.ToLower())
            }).ToList();

            return usersForReturn;

        }

        public List<IdentityRole> GetAllRoles()
        {
            return _roleManager.Roles.Where(x => x.Name != "Admin").ToList();
        }

        public void Dispose()
        {
            _ctx.Dispose();
            _userManager.Dispose();
            _roleManager.Dispose();
        }
    }
}