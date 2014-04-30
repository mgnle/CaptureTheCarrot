using System.Collections.Generic;
using UnityEngine;


namespace AssemblyCSharp
{
		public class Species
		{
				private List<BunnyControl> members;
		
				public Species ()
				{
					members = new List<BunnyControl>();
				}
				
				public void Add(BunnyControl bunny)
				{
					members.Add(bunny);
				}
				
				public void Remove(BunnyControl bunny)
				{
					members.Remove(bunny);
				}
				
				public float GetAverageFitness()
				{
					float averageFitness = 0;
					float sum = 0;
					foreach(BunnyControl bunny in members)
					{
						sum += bunny.brain.Evaluate();
					}
					if(members.Count > 0) averageFitness = sum/members.Count;
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

