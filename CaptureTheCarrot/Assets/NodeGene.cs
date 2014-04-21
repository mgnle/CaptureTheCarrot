using System;
namespace AssemblyCSharp
{
		public enum NodeType
		{
			Input,
			Output,
			Hidden
		}

		public class NodeGene
		{
				public int nodeID;
				public NodeType type;

				public NodeGene (int id, NodeType type)
				{
					this.nodeID = id;
					this.type = type;					
				}
		}
}

