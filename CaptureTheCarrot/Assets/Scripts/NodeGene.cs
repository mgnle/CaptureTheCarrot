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
				
				public NodeGene (NodeGene toCopy)
				{
					this.nodeID = toCopy.nodeID;
					this.type = toCopy.type;					
				}
				
				public override bool Equals(System.Object obj)
				{
					NodeGene n = (NodeGene) obj;
					if(this.nodeID == n.nodeID && this.type.Equals(n.type))
					{
						return true;
					}
					return false;
				}
		}
}

