namespace  FayoumGovPortal.Core.Umbraco.TemplateConstants
{
    public static class SharedUmbracoPartialViews
    {
        internal const string UmbracoPartialComponentsPath = "Umbraco";
        public const string Breadcrumbs = $"{UmbracoPartialComponentsPath}/_{nameof(Breadcrumbs)}";
        public const string Paginator = $"{UmbracoPartialComponentsPath}/_{nameof(Paginator)}";
        public const string SeoMetaData = $"{UmbracoPartialComponentsPath}/_{nameof(SeoMetaData)}";
        public const string LanguageSwitcher = $"{UmbracoPartialComponentsPath}/_{nameof(LanguageSwitcher)}";
        public const string GlobalSearchForm = $"{UmbracoPartialComponentsPath}/_{nameof(GlobalSearchForm)}";
        public const string GreCaptchaView = $"{UmbracoPartialComponentsPath}/_{nameof(GreCaptchaView)}";
    }
}
