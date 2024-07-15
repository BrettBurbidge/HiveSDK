using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HiveServer.SDK
{
    public class NodeAction
    {
        public NodeAction(string name, string payload)
        {
            ActionID = Guid.NewGuid();
            Name = name;
            Payload = payload;
            Status = ActionStatus.WaitingToBePickedUp;
        }

        public Guid ActionID { get; set; }
        public string Name { get; set; }
        public string Payload { get; set; }
        public ActionStatus Status { get; set; }

        // Enum should be defined outside the class for clarity and serialization
        public enum ActionStatus
        {
            WaitingToBePickedUp,
            WaitingForCompletion,
            Finished
        }
    }

}
