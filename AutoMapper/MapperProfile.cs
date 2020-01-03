using AutoMapper;
using Garage2.ViewModels;
using Garage2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Garage2.AutoMapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<Member, MemberEditViewModel>().ReverseMap();
            CreateMap<Member, MemberRegisterViewModel>().ReverseMap();
            CreateMap<Member, MemberDetailsViewModel>();

            

        }

    }
}
