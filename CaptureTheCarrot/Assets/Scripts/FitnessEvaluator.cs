using System.Collections.Generic;
using UnityEngine;

public class FitnessEvaluator {
	
	/* Calculates the fitness of the neural network based on the distance
	 * from the carrot and whether an enemy is in firing range.
	 * For distance, take the average of all distances and inverse it.
	 * For firing, ...not really sure. */
	public float Evaluate(List<int> distance, List<int> firing) {
		int sum = 0;
		float nearFitness = 0;
		float fireFitness = 0;
				
		for (int i = 0; i < distance.Count; i++) {
			sum += distance[i];
		}
		
		if (sum != 0 && distance.Count != 0) {
			nearFitness = 1/(sum/distance.Count);
		}
		nearFitness = nearFitness;//*userInputScale;
			
		// Fitness for firing
		/*if (CalculateOnTargetSensor() == 1)
			firingCount++;
		firingFitness = 1 - (1 / firingCount);
		firingFitness = firingFitness;//*userInputScale;
		//Debug.Log ("Firing Fitness: " + firingFitness);*/
			
			
		return nearFitness + fireFitness;
	}
	
}
