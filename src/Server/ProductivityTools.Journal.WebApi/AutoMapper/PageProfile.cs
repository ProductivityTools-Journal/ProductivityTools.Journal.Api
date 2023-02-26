using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductivityTools.Meetings.WebApi.AutoMapper
{
    public class PageProfile :Profile
    {
        public PageProfile()
        {
            CreateMap<ProductivityTools.Meetings.Database.Objects.Page, ProductivityTools.Meetings.CoreObjects.Page>()
                .ReverseMap();
            //CreateMap<ProductivityTools.Meetings.Database.Objects.JournalItemNotes, ProductivityTools.Meetings.CoreObjects.JournalItemNotes>()
            //    .ReverseMap();
        }
    }
}
