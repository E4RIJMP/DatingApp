using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class LikesRepository(DataContext context, IMapper mapper) : ILikesRepository
{
    public void AddLike(UserLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
        return await context.Likes
            .Where(like => like.SourceUserId == currentUserId)
            .Select(like => like.TargetUserId)
            .ToListAsync();
    }

    public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
    {
        return await context.Likes
            .FindAsync(sourceUserId, targetUserId);
    }

    public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
    {
        var likes = context.Likes.AsQueryable();
        IQueryable<MemberDto> query;

        switch (likesParams.Predicate)
        {
            case "liked":
                query = likes
                    .Where(like => like.SourceUserId == likesParams.UserId)
                    .Select(like => like.TargetUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                break;
            case "likedBy":
                query = likes
                    .Where(like => like.TargetUserId == likesParams.UserId)
                    .Select(like => like.SourceUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                break;
            default:
                var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);

                query = likes
                    .Where(like => like.TargetUserId == likesParams.UserId && likeIds.Contains(like.SourceUserId))
                    .Select(like => like.SourceUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                break;
        }

        return await PagedList<MemberDto>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);
    }
}
