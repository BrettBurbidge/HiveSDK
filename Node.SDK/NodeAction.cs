using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Node.SDK
{
    public class NodeAction
    {
        public Guid ActionID { get; set; }
        public string Name { get; set; }
        public string Payload { get; set; }
        public ActionStatus Status { get; set; }

        public enum ActionStatus
        {
            WaitingToBePickedUp,
            WaitingForCompletion,
            Finished
        }
    }
}
