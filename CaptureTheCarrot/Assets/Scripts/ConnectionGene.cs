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
				
				public ConnectionGene (ConnectionGene toCopy)
				{
					this.innovationNumber = toCopy.innovationNumber;
					this.nodeIn = toCopy.nodeIn;
					this.nodeOut = toCopy.nodeOut;
					this.weight = toCopy.weight;
					this.enabled = toCopy.enabled;
				}
				
				public override bool Equals(System.Object obj)
				{
					ConnectionGene c = (ConnectionGene) obj;
					if (this.innovationNumber == c.innovationNumber &&
						this.nodeIn == c.nodeIn && this.nodeOut == c.nodeOut &&
						this.weight == c.weight && this.enabled == c.enabled)
					{
						return true;
					}
					return false;
				}
		}
}

