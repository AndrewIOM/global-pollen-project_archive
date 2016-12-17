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
        Task<AppServiceResult<PublicProfile>> GetPublicProfile(string userId);
        Task<AppServiceResult> RequestValidationEmail(string userId, string code);
        AppServiceResult RequestPasswordReset(AppUser user);
        AppServiceResult UpdatePublicProfile(AppUser user, PublicProfile profile); // TODO remove PublicProfile, use AppUser instead?
        
        // Clubs
        AppServiceResult<Club> CreateClub(AddClub club);
        AppServiceResult JoinClub();
        AppServiceResult LeaveClub();
        PagedAppServiceResult<Club> ListClubsByScore(int count);

        // General User Tasks
        PagedAppServiceResult<AppUser> ListUsersByScore(int count);
    }
}