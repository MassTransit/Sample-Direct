namespace DirectClient
{
    using System.Diagnostics;


    public class NodeOptions
    {
        public NodeOptions()
        {
            NodeId = Process.GetCurrentProcess().Id.ToString();
        }

        public string NodeId { get; }
    }
}