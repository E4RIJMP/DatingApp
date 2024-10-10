using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        return await context.Users
            .Where(user => user.NormalizedUserName == username.ToUpper())
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        var query = context.Users.AsQueryable();

        query = query.Where(user => !string.IsNullOrWhiteSpace(userParams.CurrentUsername)
            && user.NormalizedUserName != userParams.CurrentUsername.ToUpper());

        if (!string.IsNullOrWhiteSpace(userParams.Gender))
        {
            query = query.Where(user => user.Gender == userParams.Gender);
        }

        var maxDateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        query = query.Where(user => user.DateOfBirth <= maxDateOfBirth);

        query = userParams.OrderBy switch
        {
            "created" => query.OrderByDescending(user => user.Created),
            _ => query.OrderByDescending(user => user.LastActive)
        };

        if (userParams.MaxAge.HasValue)
        {
            var minDateOfBirth = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge.Value - 1));
            query = query.Where(user => user.DateOfBirth >= minDateOfBirth);
        }

        return await PagedList<MemberDto>.CreateAsync(
            query.ProjectTo<MemberDto>(mapper.ConfigurationProvider),
            userParams.PageNumber,
            userParams.PageSize
        );
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
        .Include(user => user.Photos)
        .SingleOrDefaultAsync(user => user.NormalizedUserName == username.ToUpper());
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        return await context.Users
        .Include(user => user.Photos)
        .ToListAsync();
    }

    public void update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified;
    }
}
