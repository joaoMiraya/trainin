using API.Shared.Constants;

namespace API.Application.Interfaces;
public interface INotificationContext
{
    IReadOnlyList<Notification> Notifications { get; }
    bool HasNotifications { get; }
    bool HasErrors { get; }
    bool HasWarnings { get; }
    bool IsValid { get; }

    void AddNotification(Notification notification);
    void AddNotification(string message, NotificationSystemType type, string? property = null, string? code = null);
    void AddNotifications(IEnumerable<Notification> notifications);
    void AddError(string message, string? property = null, string? code = null);
    void AddWarning(string message, string? property = null, string? code = null);
    void AddInfo(string message, string? property = null, string? code = null);
    void AddSuccess(string message, string? property = null, string? code = null);
    void AddValidation(string message, string? property = null, string? code = null);
    void Clear();

    IEnumerable<Notification> GetByType(NotificationSystemType type);
    IEnumerable<Notification> GetByProperty(string property);
}