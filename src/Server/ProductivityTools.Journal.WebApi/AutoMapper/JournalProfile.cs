using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityTools.Meetings.WebApi.AutoMapper
{
    public class JournalProfile :Profile
    {
        public JournalProfile()
        {
            CreateMap<ProductivityTools.Meetings.Database.Objects.Page, ProductivityTools.Meetings.CoreObjects.JournalItem>()
                .ReverseMap();
            CreateMap<ProductivityTools.Meetings.Database.Objects.JournalItemNotes, ProductivityTools.Meetings.CoreObjects.JournalItemNotes>()
                .ReverseMap();
        }
    }
}
