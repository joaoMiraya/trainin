
using API.Application.DTOs;

namespace API.Application.Interfaces;

public interface ILocalesService
{
    List<object> GetLocalesByLanguage(string language);
    object GetLocales(string fileName);
}
