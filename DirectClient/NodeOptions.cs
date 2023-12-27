namespace DirectClient
{
    using System;


    public class NodeOptions
    {
        public NodeOptions()
        {
            NodeId = Random.Shared.Next(500000).ToString();
        }

        public string NodeId { get; }
    }
}