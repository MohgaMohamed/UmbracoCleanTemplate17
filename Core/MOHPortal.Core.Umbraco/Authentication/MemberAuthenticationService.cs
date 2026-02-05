//using DocumentFormat.OpenXml.Office2010.Excel;
//using Microsoft.Extensions.Options;
//using MOHPortal.Core.Umbraco.Localization;
//using MOHPortal.Core.Umbraco.Model.Gen;
//using MOHPortal.Core.Umbraco.PageServices;
//using Umbraco.Cms.Core.Configuration.Models;
//using Umbraco.Cms.Core.Models;
//using Umbraco.Cms.Core.Models.PublishedContent;
//using Umbraco.Cms.Core.Security;
//using Umbraco.Cms.Core.Services;
//using Umbraco.Cms.Web.Common.Security;

//namespace MOHPortal.Core.Umbraco.Authentication
//{
//    internal class MemberAuthenticationService : IMemberAuthenticationService
//    {
//        private readonly LocalizationWrapper _localization;
//        private readonly MemberPasswordConfigurationSettings _memberPasswordSettings;
//        private readonly IMemberManager _memberManager;
//        private readonly IMemberService _memberService;
//        private readonly IUmbracoPageService _pageService;
//        private readonly IMemberSignInManager _memberSignInManager;

//        public MemberAuthenticationService(LocalizationWrapper localization, IOptions<MemberPasswordConfigurationSettings> memberPasswordSettings, IMemberManager memberManager, IUmbracoPageService pageService, IMemberService memberService, IMemberSignInManager memberSignInManager)
//        {
//            _localization = localization;
//            _memberPasswordSettings = memberPasswordSettings.Value;
//            _memberManager = memberManager;
//            _pageService = pageService;
//            _memberService = memberService;
//            _memberSignInManager = memberSignInManager;
//        }

//        public IEnumerable<string> ChallengePasswordStrength(string? password)
//        {
//            if(string.IsNullOrWhiteSpace(password))
//            {
//                yield return _localization.ValidationRequired;
//                yield break;
//            }

//            Dictionary<Func<string, bool>, string> rules = new(){
//                { (x) => x.Length < _memberPasswordSettings.RequiredLength, _localization.MemberAtLeastEightChar },
//                { (x) => !x.Any(char.IsUpper), _localization.MemberAtLeast1UpperCase },
//                { (x) => !x.Any(char.IsDigit), _localization.MemberAtLeast1Numeric },
//                { (x) => x.All(char.IsLetterOrDigit), _localization.MemberSpecialChar },
//            };

//            foreach (KeyValuePair<Func<string, bool>, string> rule in rules)
//            {
//                if(rule.Key.Invoke(password))
//                {
//                    yield return rule.Value;
//                }
//            }
//        }

//        public bool ExistsByEmail(string email) => _memberService.GetByEmail(email) is not null;
//        public async Task<string?> GeneratePasswordResetToken(string email)
//        {
//            MemberIdentityUser? member = await _memberManager.FindByEmailAsync(email);
//            if (member is null)
//            {
//                return default;
//            }

//            return await _memberManager.GeneratePasswordResetTokenAsync(member);
//        }


//        public Task<MemberIdentityUser?> GetAuthenticatedMember() => _memberManager.GetCurrentMemberAsync();
//        public async Task<TPublishedMemberModel?> GetAuthenticatedMemberAs<TPublishedMemberModel>() where TPublishedMemberModel : class, IPublishedContent
//        {
//            MemberIdentityUser? member = await GetAuthenticatedMember();
//            if(member is null)
//            {
//                return default;
//            }

//            return _memberManager.AsPublishedMember(member)?.SafeCast<TPublishedMemberModel>();
//        }

//        public async Task<IMember?> GetAuthenticatedMemberModel()
//        {
//            MemberIdentityUser? member = await GetAuthenticatedMember();
//            return _memberService.GetByKey(member?.Key ?? default);
//        }

//        public string GetLoginPageUrl()
//        {
//            Login? pageurl = _pageService.GetContentPageAs<UserManagementContainer>(UserManagementContainer.ModelTypeAlias)?
//               .FirstChild<Login>();

//            return pageurl is null
//                ? _pageService.GetHomeUrl()
//                : _pageService.Encoded(pageurl.Url(mode: UrlMode.Absolute));
//        }

//        public TPublishedMemberModel? GetMemberAs<TPublishedMemberModel>(MemberIdentityUser member) where TPublishedMemberModel : class, IPublishedContent => _memberManager.AsPublishedMember(member)?.SafeCast<TPublishedMemberModel>();
//        public async Task<TPublishedMemberModel?> GetMemberAs<TPublishedMemberModel>(IMember member) where TPublishedMemberModel : class, IPublishedContent
//        {
//            MemberIdentityUser? identityMember = await _memberManager.FindByIdAsync(member.Id.ToString());
//            if(identityMember == null)
//            {
//                return null;
//            }

//            return GetMemberAs<TPublishedMemberModel>(identityMember);
//        }

//        public async Task<TPublishedMemberModel?> GetMemberAs<TPublishedMemberModel>(int id) where TPublishedMemberModel : class, IPublishedContent
//        {
//            MemberIdentityUser? member = await _memberManager.FindByIdAsync(id.ToString());
//            if (member == null) 
//            {
//                return null;
//            }
//            return GetMemberAs<TPublishedMemberModel>(member);
//        }

//        public async Task<TPublishedMemberModel?> GetMemberAs<TPublishedMemberModel>(Guid key) where TPublishedMemberModel : class, IPublishedContent
//        {
//            IMember? member = _memberService.GetByKey(key);
//            if (member == null)
//            {
//                return null;
//            }

//            return await GetMemberAs<TPublishedMemberModel>(member);
//        }

//        public async Task<bool> IsAuthenticated()
//        {
//            return await (_memberManager.GetCurrentMemberAsync()) is not null;
//        }

//        public bool IsMemberVerified(string email)
//        {
//            bool isVerified = default;
//            if (string.IsNullOrWhiteSpace(email))
//            {
//                return isVerified;
//            }

//            IMember? member = _memberService.GetByEmail(email);
//            if (member != null)
//            {
//                isVerified = member.GetValue<bool>(nameof(Citizen.Verification));
//            }

//            return isVerified;
//        }

//        public Task SignOutMember() => _memberSignInManager.SignOutAsync();
//    }
//}
