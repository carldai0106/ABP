using AutoMapper;
using CMS.Application.User.Dto;
using CMS.Domain.User;

namespace CMS.Application
{
    internal static class CustomDtoMapper
    {
        private static volatile bool _mappedBefore;
        private static readonly object SyncObj = new object();

        public static void CreateMappings()
        {
            lock (SyncObj)
            {
                if (_mappedBefore)
                {
                    return;
                }

                CreateMappingsInternal();

                _mappedBefore = true;
            }
        }

        private static void CreateMappingsInternal()
        {
            //Mapper.Initialize(cfg =>
            //{
            //    cfg.CreateMap<UserEntity, CreateUserDto>()
            //        .ForMember(dto => dto.Password, options => options.Ignore())
            //        .ReverseMap()
            //        .ForMember(user => user.Password, options => options.Ignore());

            //    cfg.CreateMap<UserEntity, UserEditDto>()
            //        .ForMember(dto => dto.Password, options => options.Ignore())
            //        .ReverseMap()
            //        .ForMember(user => user.Password, options => options.Ignore())
            //        .ForMember(user => user.UserRoles, options => options.Ignore());
            //});

            Mapper.CreateMap<UserEntity, CreateUserDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());

            Mapper.CreateMap<UserEntity, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore())
                .ForMember(user => user.UserRoles, options => options.Ignore());
        }
    }
}