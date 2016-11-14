
using System.Collections.Generic;
using System.Threading.Tasks;
using GlobalPollenProject.App.Models;
using GlobalPollenProject.App.Validation;

namespace GlobalPollenProject.App.Interfaces
{
    public interface IUserService : IAppService
    {
        Task<AppServiceResult> Login(LoginDetails user);
        Task<AppServiceResult> Logout();
        Task<AppServiceResult<AppUser>> RegisterForAccount(NewAppUser user);
        Task<AppServiceResult<AppUser>> GetCurrentUser();
        Task<AppServiceResult> RequestValidationEmail(string userId, string code);
        AppServiceResult RequestPasswordReset(AppUser user);
        AppServiceResult UpdatePublicProfile(AppUser user, PublicProfile profile); // TODO remove PublicProfile, use AppUser instead?
        
        // Clubs
        AppServiceResult CreateClub();
        AppServiceResult JoinClub();
        AppServiceResult LeaveClub();
        AppServiceResult<List<Club>> ListClubsByScore(int count);

        // General User Tasks
        AppServiceResult<List<AppUser>> ListUsersByScore(int count);
    }
}