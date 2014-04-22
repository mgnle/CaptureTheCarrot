using System;
namespace AssemblyCSharp
{
		public class ConnectionGene
		{
				public int innovationNumber;
				public int nodeIn;
				public int nodeOut;
				public double weight;
				public bool enabled;
				
				public ConnectionGene (int innovNum, int nodeIn, int nodeOut, double weight)
				{
					this.innovationNumber = innovNum;
					this.nodeIn = nodeIn;
					this.nodeOut = nodeOut;
					this.weight = weight;
					this.enabled = true;
				}
		}
}

