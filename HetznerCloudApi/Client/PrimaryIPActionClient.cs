using HetznerCloudApi.Object.Action;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HetznerCloudApi.Client
{
    public class PrimaryIPActionClient
    {
        private readonly string _token;

        public PrimaryIPActionClient(string token)
        {
            _token = token;
        }


        public async Task<Action> AssignToResource(long primaryIPId, string Assignee_Type, long Assignee_Id)
        {
            var tmp = new
            {
                assignee_type = Assignee_Type,
                assignee_id = Assignee_Id
            };


            return (await Core.SendPostRequest<SimpleActionResponse>(_token, $"/primary_ips/{primaryIPId}/actions/assign", tmp)).Action;

        }

        public async Task<Action> UnassignFromResource(long primaryIPId)
        {
            return (await Core.SendPostRequest<SimpleActionResponse>(_token, $"/primary_ips/{primaryIPId}/actions/unassign")).Action;

        }
    }
}
