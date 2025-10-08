namespace API.Shared.Constants;

public enum FriendshipStatus
{
    Pending,
    Accepted,
    Rejected,
    Blocked
}

public enum NotificationType
{
    Like,
    Comment,
    FriendRequest,
    FriendAccepted,
    Mention,
    Other
}

public enum ReportTargetType
{
    Post,
    Comment,
    Reply
}

public enum ImageType
{
    Profile = 0,
    Post = 1
}