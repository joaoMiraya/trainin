
using API.Application.Interfaces;
using API.Shared.Constants;

public class NotificationContext : INotificationContext
{
    private readonly List<Notification> _notifications = new();

    public IReadOnlyList<Notification> Notifications => _notifications.AsReadOnly();
    public bool HasNotifications => _notifications.Count > 0;
    public bool HasErrors => _notifications.Any(x => x.Type == NotificationSystemType.Error);
    public bool HasWarnings => _notifications.Any(x => x.Type == NotificationSystemType.Warning);
    public bool IsValid => !HasErrors;

    public void AddNotification(Notification notification)
    {
        _notifications.Add(notification);
    }

    public void AddNotification(string message, NotificationSystemType type, string? property = null, string? code = null)
    {
        AddNotification(new Notification(message, type, property, code));
    }

    public void AddNotifications(IEnumerable<Notification> notifications)
    {
        _notifications.AddRange(notifications);
    }

    public void AddError(string message, string? property = null, string? code = null)
    {
        AddNotification(Notification.Error(message, property, code));
    }

    public void AddWarning(string message, string? property = null, string? code = null)
    {
        AddNotification(Notification.Warning(message, property, code));
    }

    public void AddInfo(string message, string? property = null, string? code = null)
    {
        AddNotification(Notification.Info(message, property, code));
    }

    public void AddSuccess(string message, string? property = null, string? code = null)
    {
        AddNotification(Notification.Success(message, property, code));
    }

    public void AddValidation(string message, string? property = null, string? code = null)
    {
        AddNotification(Notification.Validation(message, property, code));
    }

    public void Clear()
    {
        _notifications.Clear();
    }

    public IEnumerable<Notification> GetByType(NotificationSystemType type)
    {
        return _notifications.Where(x => x.Type == type);
    }

    public IEnumerable<Notification> GetByProperty(string property)
    {
        return _notifications.Where(x => x.Property == property);
    }
}

public class Notification
{
    public string Message { get; set; }
    public NotificationSystemType Type { get; set; }
    public string? Property { get; set; }
    public string? Code { get; set; }

    public Notification(string message, NotificationSystemType type, string? property = null, string? code = null)
    {
        Message = message;
        Type = type;
        Property = property;
        Code = code;
    }

    public static Notification Error(string message, string? property = null, string? code = null)
        => new(message, NotificationSystemType.Error, property, code);

    public static Notification Warning(string message, string? property = null, string? code = null)
        => new(message, NotificationSystemType.Warning, property, code);

    public static Notification Info(string message, string? property = null, string? code = null)
        => new(message, NotificationSystemType.Info, property, code);

    public static Notification Success(string message, string? property = null, string? code = null)
        => new(message, NotificationSystemType.Success, property, code);

    public static Notification Validation(string message, string? property = null, string? code = null)
        => new(message, NotificationSystemType.Validation, property, code);
}