using System.Collections;
using System.Net.NetworkInformation;
using HiveServer.SDK;

namespace HiveServer
{

    /// <summary>
    /// Fake Action Queue no state management yet. 
    /// </summary>
    public static class ActionQueue
    {
        public static List<NodeAction> _actionQueue = new List<NodeAction>();

        public static void AddFakeActions()
        {
            AddAction(new NodeAction("Green Energy Check", "{}"));
            AddAction(new NodeAction("Green PowerPay Point Award", "{}"));
            AddAction(new NodeAction("Green NFT Distribution", "{}"));
            AddAction(new NodeAction("Green Action 1", "{}"));
            AddAction(new NodeAction("Green Action 2", "{}"));
            AddAction(new NodeAction("Green Action 3", "{}"));
            AddAction(new NodeAction("Green Action 4", "{}"));
            AddAction(new NodeAction("Green Action 5", "{}"));
            AddAction(new NodeAction("Green Action 6", "{}"));
            AddAction(new NodeAction("Green Action 7", "{}"));
            AddAction(new NodeAction("Green Action 8", "{}"));
            AddAction(new NodeAction("Green Action 9", "{}"));
            AddAction(new NodeAction("Green Action 10", "{}"));


        }

        public static void AddAction(NodeAction action)
        {
            _actionQueue.Add(action);
        }

        public static NodeAction? GetActionToConsume()
        {
            foreach (var action in _actionQueue)
            {
                if (action.Status == NodeAction.ActionStatus.WaitingToBePickedUp)
                {
                    action.Status = NodeAction.ActionStatus.WaitingForCompletion;
                    return action;
                }
            }
            return null;
        }

        public static bool RemoveActionFromQueue(Guid nodeActionID)
        {
            _actionQueue.RemoveAt(_actionQueue.FindIndex(x => x.ActionID == nodeActionID));
            return true;
        }
    }
}
