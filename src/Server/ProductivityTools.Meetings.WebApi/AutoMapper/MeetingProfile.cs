using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityTools.Meetings.WebApi.AutoMapper
{
    public class MeetingProfile :Profile
    {
        public MeetingProfile()
        {
            CreateMap<ProductivityTools.Meetings.Database.Objects.JournalItem, ProductivityTools.Meetings.CoreObjects.Meeting>()
                .ReverseMap();
        }
    }
}
