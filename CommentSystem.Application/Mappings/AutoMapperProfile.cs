using AutoMapper;
using CommentSystem.Application.DTOs;
using CommentSystem.Domain.Entities;

namespace CommentSystem.Application.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateCommentDto, Comment>();
        CreateMap<Comment, PublicCommentDto>();

        CreateMap<Comment, AdminCommentDto>()
            .ForMember(dest => dest.Status,
                opt =>
                    opt.MapFrom(src => src.Status.ToString()));

        CreateMap<Comment, UserCommentDto>()
            .ForMember(dest => dest.Status,
                opt =>
                    opt.MapFrom(src => src.Status.ToString()));
    }
}