﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductivityTools.Meetings.Services.AutoMapper
{
    public class TreeProfile : Profile
    {
        public TreeProfile()
        {
            CreateMap<ProductivityTools.Meetings.Database.Objects.Journal, ProductivityTools.Meetings.CoreObjects.TreeNode>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.JournalId))
                .ReverseMap();
        }
    }
}
