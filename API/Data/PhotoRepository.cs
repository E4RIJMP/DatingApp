using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class PhotoRepository(DataContext context) : IPhotoRepository
{
    public async Task<Photo?> GetPhotoById(int id)
    {
        return await context.Photos
        .IgnoreQueryFilters()
        .SingleOrDefaultAsync(photo => photo.Id == id);
    }
    public async Task<IEnumerable<PhotoForApprovalDto>> GetUnapprovedPhotos()
    {
        return await context.Photos
        .IgnoreQueryFilters()
        .Where(photo => photo.IsApproved == false)
        .Select(photo => new PhotoForApprovalDto
        {
            Id = photo.Id,
            Username = photo.AppUser.UserName,
            Url = photo.Url,
            IsApproved = photo.IsApproved
        }).ToListAsync();
    }
    public void RemovePhoto(Photo photo)
    {
        context.Photos.Remove(photo);
    }
}
