using CleanArchitecture.WebUI.Infrastructure.Configuration;
using CleanArchitecture.WebUI.Models;
using CleanArchitecture.WebUI.Models.Configuration;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

namespace CleanArchitecture.WebUI.Extensions;

public static class HtmlHelperExtensions
{
    public static IHeaderViewModel GetHeaderViewModel(this IHtmlHelper html, bool hideMenu = false)
    {
        var externalLinks = (html.ViewContext.HttpContext.RequestServices.GetService(typeof(IOptions<ExternalLinksConfiguration>)) as IOptions<ExternalLinksConfiguration>)?.Value;
        var authConfig = (html.ViewContext.HttpContext.RequestServices.GetService(typeof(IOptions<IdentityServerOptions>)) as IOptions<IdentityServerOptions>)?.Value;
        var requestRoot = html.ViewContext.HttpContext.Request.GetRequestUrlRoot();
        var requestPath = html.ViewContext.HttpContext.Request.Path;
        //var commitmentsSiteUrl = new Uri(externalLinks?.CommitmentsSiteUrl);
        var hashedAccountId = html.ViewContext.RouteData.Values["accountId"]?.ToString();

#pragma warning disable CS8601 // Possible null reference assignment.
        var headerModel = new HeaderViewModel(new HeaderConfiguration
        {
            //EmployerCommitmentsBaseUrl = $"{commitmentsSiteUrl.Scheme}://{commitmentsSiteUrl.Host}/commitments",
            //EmployerCommitmentsV2BaseUrl = $"{commitmentsSiteUrl.Scheme}://{commitmentsSiteUrl.Host}",
            //EmployerFinanceBaseUrl = externalLinks?.ManageApprenticeshipSiteUrl,
            //ManageApprenticeshipsBaseUrl = externalLinks?.ManageApprenticeshipSiteUrl,
            AuthenticationAuthorityUrl = authConfig?.BaseAddress,
            ClientId = authConfig?.ClientId,
            //EmployerRecruitBaseUrl = externalLinks?.EmployerRecruitmentSiteUrl,
            SignOutUrl = hashedAccountId == null ? new Uri($"{requestRoot}/signout/") : new Uri($"{requestRoot}/{hashedAccountId}/signout/"),
            ChangeEmailReturnUrl = new Uri($"{requestRoot}{requestPath}"),
            ChangePasswordReturnUrl = new Uri($"{requestRoot}{requestPath}")
        },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["accountId"]?.ToString()
            });
#pragma warning restore CS8601 // Possible null reference assignment.

        headerModel.SelectMenu("Finance");

        if (html.ViewBag.HideNav is bool && html.ViewBag.HideNav)
        {
            headerModel.HideMenu();
        }

        return headerModel;
    }

    public static IFooterViewModel GetFooterViewModel(this IHtmlHelper html)
    {
        var externalLinks =
            (html.ViewContext.HttpContext.RequestServices.GetService(typeof(IOptions<ExternalLinksConfiguration>))
                as IOptions<ExternalLinksConfiguration>)?.Value;

#pragma warning disable CS8601 // Possible null reference assignment.
        return new FooterViewModel(new FooterConfiguration
        {
            ManageApprenticeshipsBaseUrl = externalLinks?.ManageApprenticeshipSiteUrl
        },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["accountId"]?.ToString()
            });
#pragma warning restore CS8601 // Possible null reference assignment.
    }

    public static ICookieBannerViewModel GetCookieBannerViewModel(this IHtmlHelper html)
    {
        var externalLinks =
            (html.ViewContext.HttpContext.RequestServices.GetService(typeof(IOptions<ExternalLinksConfiguration>))
                as IOptions<ExternalLinksConfiguration>)?.Value;

#pragma warning disable CS8601 // Possible null reference assignment.
        return new CookieBannerViewModel(new CookieBannerConfiguration
        {
            ManageFamilyHubBaseUrl = externalLinks?.ManageApprenticeshipSiteUrl
        },
            new UserContext
            {
                User = html.ViewContext.HttpContext.User,
                HashedAccountId = html.ViewContext.RouteData.Values["accountId"]?.ToString()
            });
#pragma warning restore CS8601 // Possible null reference assignment.
    }
}
