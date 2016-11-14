using GlobalPollenProject.App.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GlobalPollenProject.WebUI.Services
{
    public class NetCoreUserResolverService : IUserResolverService
    {
        private readonly IHttpContextAccessor _context;
        public NetCoreUserResolverService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetCurrentUserName()
        {
            return _context.HttpContext.User?.Identity?.Name;
        }
    }
}