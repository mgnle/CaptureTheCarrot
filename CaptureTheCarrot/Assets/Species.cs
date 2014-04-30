using System.Collections.Generic;
namespace AssemblyCSharp
{
		public class Species
		{
				private List<BunnyControl> members;
				private float averageFitness;
		
				public Species ()
				{
					members = new List<BunnyControl>();
					averageFitness = 0;
				}
				
				public void Add(BunnyControl bunny)
				{
					members.Add(bunny);
					recalculateAverageFitness();
				}
				
				public void Remove(BunnyControl bunny)
				{
					members.Remove(bunny);
					recalculateAverageFitness();
				}
				
				private void recalculateAverageFitness()
				{
					float sum = 0;
					foreach(BunnyControl bunny in members)
					{
						sum += bunny.brain.Evaluate();
					}
					if(members.Count > 0) averageFitness = sum/members.Count;
				}
				
				public float GetAverageFitness()
				{
					return averageFitness;
				}
								
				public List<BunnyControl> GetMembers()
				{
					return members;
				}
				
				public BunnyControl LowestFitnessIndividualInSpecies()
				{
					float minFitness = float.MaxValue;
					BunnyControl minBunny = members[0];
					foreach (BunnyControl bunny in members)
					{
						float fitness = bunny.brain.Evaluate();
						if (fitness < minFitness)
						{
							minBunny = bunny;
							minFitness = fitness;
						}
					}
					return minBunny;
				}
				
				public Dictionary<BunnyControl, float> CalculateAdjustedFitness()
				{
					Dictionary<BunnyControl, float> bunnyFitnessMap = new Dictionary<BunnyControl, float>();
					foreach (BunnyControl bunny in members)
					{
						float fitness = bunny.brain.Evaluate();
						float adjusted = fitness/(float)members.Count;
						bunnyFitnessMap.Add(bunny, adjusted);
					}
					return bunnyFitnessMap;
				}
				
				public void ChooseParents(out BunnyControl bestBunny, out BunnyControl secondBestBunny)
				{
					bestBunny = members[0];
					secondBestBunny = members[0];
					
					foreach(BunnyControl bunny in members) {
						float bestBunnyEval = bestBunny.brain.Evaluate();
						float secondBestBunnyEval = secondBestBunny.brain.Evaluate();						
						float bunnyEval = bunny.brain.Evaluate();
						
						if(bunnyEval > bestBunnyEval) {
							bestBunny = bunny;
			            }
			            else if(bunnyEval > secondBestBunnyEval) {
			                secondBestBunny = bunny;
			            }
			        }
		        }				
        
    	}
}

