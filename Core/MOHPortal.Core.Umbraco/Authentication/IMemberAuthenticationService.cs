//using Umbraco.Cms.Core.Models;
//using Umbraco.Cms.Core.Models.PublishedContent;
//using Umbraco.Cms.Core.Security;

//namespace  FayoumGovPortal.Core.Umbraco.Authentication
//{
//    public interface IMemberAuthenticationService
//    {
//        public Task<bool> IsAuthenticated();
//        public bool ExistsByEmail(string email);
        
//        public IEnumerable<string> ChallengePasswordStrength(string? password);
        
//        public TPublishedMemberModel? GetMemberAs<TPublishedMemberModel>(MemberIdentityUser member)
//            where TPublishedMemberModel : class, IPublishedContent;
//        public Task<TPublishedMemberModel?> GetMemberAs<TPublishedMemberModel>(IMember member)
//            where TPublishedMemberModel : class, IPublishedContent;

//        public Task<TPublishedMemberModel?> GetMemberAs<TPublishedMemberModel>(int id)
//            where TPublishedMemberModel : class, IPublishedContent;
//        public Task<TPublishedMemberModel?> GetMemberAs<TPublishedMemberModel>(Guid key)
//            where TPublishedMemberModel : class, IPublishedContent;


//        public Task<MemberIdentityUser?> GetAuthenticatedMember();
//        public Task<TPublishedMemberModel?> GetAuthenticatedMemberAs<TPublishedMemberModel>()
//            where TPublishedMemberModel : class, IPublishedContent;
//        public Task<IMember?> GetAuthenticatedMemberModel();

//        public Task<string?> GeneratePasswordResetToken(string email);
//        public Task SignOutMember();
//        public string GetLoginPageUrl();
//        public bool IsMemberVerified(string email);


//    }
//}
