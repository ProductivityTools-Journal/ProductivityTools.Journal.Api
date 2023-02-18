using IdentityModel.Client;
using ProductivityTools.Meetings.CoreObjects;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ProductivityTools.Meetings.ClientCaller
{
    public class MeetingsClient
    {
        SimpleHttpPostClient.HttpPostClient HttpPostClient;
        string Secret;

        private string token;
        private string Token
        {
            get
            {
                if (string.IsNullOrEmpty(token))
                {//code to be rewritten, mising asyncs and others, but it needs to start working
                    ServicePointManager.ServerCertificateValidationCallback += (o, c, ch, er) => true;

                    var client = new HttpClient();

                    var disco = client.GetDiscoveryDocumentAsync("https://identityserver.productivitytools.tech:8010/").Result;
                    if (disco.IsError)
                    {
                        Console.WriteLine(disco.Error);
                    }

                    var tokenResponse = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                    {
                        Address = disco.TokenEndpoint,

                        ClientId = "WPFApplication",
                        ClientSecret = "secret",
                        Scope = "ProductivityTools.Meetings.API"
                    }).Result;

                    if (tokenResponse.IsError)
                    {
                        Console.WriteLine(tokenResponse.Error);

                    }

                    Console.WriteLine(tokenResponse.Json);

                    token = tokenResponse.AccessToken;

                    //// call api
                    //var apiClient = new HttpClient();
                    //apiClient.SetBearerToken(tokenResponse.AccessToken);

                    //var response = apiClient.GetAsync("http://localhost:7001/Identity/Paid").Result;
                    //if (!response.IsSuccessStatusCode)
                    //{
                    //    Console.WriteLine(response.StatusCode);
                    //}
                    //else
                    //{
                    //    var content = response.Content.ReadAsStringAsync().Result;
                    //    Console.WriteLine(content);
                    //}
                }
                return token;
            }
        }



        public MeetingsClient(string secret)
        {
            this.Secret = secret;
            this.HttpPostClient = new SimpleHttpPostClient.HttpPostClient(true);

            // this.HttpPostClient.SetBaseUrl("https://localhost:44366/api");//iis

            //this.HttpPostClient.SetBaseUrl("http://localhost:5002/api");//vs

            //this.HttpPostClient.SetBaseUrl("https://productivitytools.tech:443/api");
            this.HttpPostClient.SetBaseUrl("https://meetings.productivitytools.tech:8020/api");
            //this.HttpPostClient.SetBaseUrl("http://192.168.1.51:8081/api");
            this.HttpPostClient.HttpClient.SetBearerToken(Token);

        }

        public async Task<List<JournalItem>> GetMeetings(int? treeNodeId = null, bool drillDown = true)
        {
            var r = this.HttpPostClient.PostAsync<List<JournalItem>>(Consts.MeetingControllerName, Consts.ListName, new MeetingListRequest() { Id = treeNodeId, DrillDown = drillDown });
            return await r;
        }

        public async Task UpdateMeeting(JournalItem meeting)
        {
            await this.HttpPostClient.PostAsync<JournalItem>(Consts.MeetingControllerName, Consts.UpdateMeetingName, meeting);
        }

        public async Task<int> SaveMeeting(JournalItem meeting)
        {
            return await this.HttpPostClient.PostAsync<int>(Consts.MeetingControllerName, Consts.AddMeetingName, meeting);
        }

        public async Task DeleteMeeting(JournalId meetingId)
        {
            await this.HttpPostClient.PostAsync<object>(Consts.MeetingControllerName, Consts.DeletePageName, meetingId);
        }

        public async Task<List<TreeNode>> GetTree()
        {
            var r = await this.HttpPostClient.PostAsync<List<TreeNode>>(Consts.TreeControllerName, Consts.TreeControlerGet);
            return r;
        }

        public async Task<object> NewTreeNode(int parentTreeId, string name)
        {
            var r = await this.HttpPostClient.PostAsync<object>(Consts.TreeControllerName, Consts.TreeControlerNewNode, new NewTreeNodeRequest(parentTreeId, name));
            return r;
        }

    }
}
