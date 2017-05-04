using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TeamsTfsBridge.Model;

namespace TeamsTfsBridge.Controllers
{
    [Route("[controller]")]
    public class PullRequestController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TfsMessage payload)
        {
            var url = Environment.GetEnvironmentVariable("TeamsChannelWebHookUrl");

            HttpClient client = new HttpClient();

            // Unfortunately, doesn't work properly in Teams UI (seems to be a bug, https://microsoftteams.uservoice.com/forums/555103-public/suggestions/16951963-fix-webhook-api)
            var potentialAction = new PotentialAction
            {
                Context = "http://schema.org",
                Type = "ViewAction",
                Name = "View in TFS",
                Target = new string[] { $"[{payload.resource._links.web.href}]" }
            };

            var card = new TeamCard
            {
                Title = $"{payload.message.markdown}",
                Text = $"{payload.detailedMessage.markdown}{Environment.NewLine}[Open in TFS]({payload.resource._links.web})",
                PotentialAction = new PotentialAction[] { potentialAction },
            };

            var json = JsonConvert.SerializeObject(card);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var result = await client.PostAsync(url, content);

            if (result.IsSuccessStatusCode)
            {
                return this.Ok();
            }
            else
            {
                return this.BadRequest(result.ReasonPhrase);
            }
        }
    }
}
