using CleanArchitecture.WebUI.Models.Configuration;
using CleanArchitecture.WebUI.Services;
using System;


namespace CleanArchitecture.WebUI.Models;

public class CookieBannerViewModel : ICookieBannerViewModel
{
    private const string CookieConsentPath = "cookieConsent";
    private const string CookieDetailsPath = CookieConsentPath + "/details";

    private readonly ICookieBannerConfiguration _configuration;
    private readonly IUserContext _userContext;
    private readonly IUrlHelper _urlHelper;

    public CookieBannerViewModel(
        ICookieBannerConfiguration configuration, 
        IUserContext userContext,
        IUrlHelper? urlHelper = null)
    {
        if (configuration == null) throw new ArgumentNullException("configuration");
        if (userContext == null) throw new ArgumentNullException("userContext");

        _urlHelper = urlHelper ?? new UrlHelper();

        _configuration = configuration;
        _userContext = userContext;
    }

    public string CookieConsentUrl => _userContext?.HashedAccountId == null 
        ? _urlHelper.GetPath(_configuration.ManageFamilyHubBaseUrl, CookieConsentPath)
        : _urlHelper.GetPath(_userContext, _configuration.ManageFamilyHubBaseUrl, CookieConsentPath);
    
    public string CookieDetailsUrl => _userContext?.HashedAccountId == null
        ? _urlHelper.GetPath(_configuration.ManageFamilyHubBaseUrl, CookieDetailsPath)
        : _urlHelper.GetPath(_userContext, _configuration.ManageFamilyHubBaseUrl, CookieDetailsPath);
}
