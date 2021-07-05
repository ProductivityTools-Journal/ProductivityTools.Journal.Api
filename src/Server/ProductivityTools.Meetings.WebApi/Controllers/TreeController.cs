using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductivityTools.Meetings.CoreObjects;
using ProductivityTools.Meetings.Services;
using ProducvitityTools.Meetings.Queries;

namespace ProductivityTools.Meetings.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TreeController : ControllerBase
    {
        readonly IMapper Mapper;
        readonly  ITreeService TreeServices;

        public TreeController(ITreeService treeService, IMapper mapper)
        {
            this.TreeServices = treeService;
            this.Mapper = mapper;
        }

        [HttpPost]
        [Route(Consts.TreeControlerGet)]
        public List<TreeNode> GetTree()
        {
            var result = TreeServices.GetTree();
            return result;
        }

        [HttpPost]
        [Authorize]
        [Route(Consts.TreeControlerNewNode)]
        public void AddTreeNode(NewTreeNodeRequest request)
        {
            this.TreeServices.AddTreeNode(request.ParentId, request.Name);
        }

        public int RemoveTreeNodeWithAllItems(RemoveTreeNodeRequest removeTreeNodeRequest)
        {
            int removed = this.TreeServices.RemoveTreeNodeWithAllItems(removeTreeNodeRequest.TreeId);
            return removed;
        }
    }
}