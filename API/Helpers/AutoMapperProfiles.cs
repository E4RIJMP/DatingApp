using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<AppUser, MemberDto>()
            .ForMember(destination => destination.Age, options =>
                options.MapFrom(source => source.DateOfBirth.CalculateAge()))
            .ForMember(destination => destination.PhotoUrl, options =>
                options.MapFrom(source => source.Photos.FirstOrDefault(photo => photo.IsMain)!.Url));
        CreateMap<Photo, PhotoDto>();
        CreateMap<MemberUpdateDto, AppUser>();
        CreateMap<RegisterDto, AppUser>();
        CreateMap<string, DateOnly>().ConvertUsing(source => DateOnly.Parse(source));
        CreateMap<Message, MessageDto>()
            .ForMember(destination => destination.SenderPhotoUrl, options =>
                options.MapFrom(source => source.Sender.Photos.FirstOrDefault(photo => photo.IsMain)!.Url))
            .ForMember(destination => destination.RecipientPhotoUrl, options =>
                options.MapFrom(source => source.Recipient.Photos.FirstOrDefault(photo => photo.IsMain)!.Url));
    }
}
