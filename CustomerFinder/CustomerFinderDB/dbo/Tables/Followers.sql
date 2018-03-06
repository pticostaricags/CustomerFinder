CREATE TABLE [dbo].[Followers] (
    [FollowersId]           BIGINT             IDENTITY (1, 1) NOT NULL,
    [UserFollowed]          NVARCHAR (50)      NOT NULL,
    [UserFollowing]         NVARCHAR (50)      NOT NULL,
    [LastTimeSeenFollowing] DATETIMEOFFSET (7) NOT NULL,
    [SessionId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [PK_FOLLOWERS] PRIMARY KEY CLUSTERED ([FollowersId] ASC)
);


GO

CREATE INDEX [IX_Followers_CrossFollowers] ON [dbo].[Followers] ([UserFollowed], [UserFollowing]) INCLUDE ([FollowersId], [LastTimeSeenFollowing], [SessionId])
