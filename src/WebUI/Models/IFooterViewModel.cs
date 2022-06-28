using CleanArchitecture.WebUI.Services;

namespace CleanArchitecture.WebUI.Models;

public interface IFooterViewModel : ILinkCollection, ILinkHelper
{
    bool UseLegacyStyles { get; }
}
