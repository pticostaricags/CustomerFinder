CREATE TABLE [dbo].[UnFollowers] (
    [UnFollowersId]           BIGINT             IDENTITY (1, 1) NOT NULL,
    [UserUnFollowed]          NVARCHAR (50)      NOT NULL,
    [UserUnFollowing]         NVARCHAR (50)      NOT NULL,
    [LastTimeSeenUnFollowing] DATETIMEOFFSET (7) NOT NULL,
    [SessionId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [PK_UNFOLLOWERS] PRIMARY KEY CLUSTERED ([UnFollowersId] ASC)
);