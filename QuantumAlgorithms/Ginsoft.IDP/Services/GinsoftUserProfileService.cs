using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Ginsoft.IDP.Services
{
    public class GinsoftUserProfileService : IProfileService
    {
        public GinsoftUserProfileService(IGinsoftUserRepository ginsoftUserRepository) {
            GinsoftUserRepository = ginsoftUserRepository;
        }

        public IGinsoftUserRepository GinsoftUserRepository { get; }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var claimsForUser = GinsoftUserRepository.GetUserClaimsBySubjectId(subjectId);

            context.IssuedClaims = claimsForUser.Select(claim => new Claim(claim.ClaimType, claim.ClaimValue)).ToList();
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            context.IsActive = GinsoftUserRepository.IsUserActive(subjectId);

            return Task.CompletedTask;
        }
    }
}
