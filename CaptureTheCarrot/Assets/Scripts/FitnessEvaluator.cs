using System.Collections.Generic;
using UnityEngine;

public class FitnessEvaluator {
	
	float nearSlider;
	float fireSlider;
	
	/* Calculates the fitness of the neural network based on the distance
	 * from the carrot and whether an enemy is in firing range.
	 * For distance, take the average of all distances and inverse it.
	 * For firing, ...not really sure. */
	public float Evaluate(List<int> distance, int firing) {
		float nearSum = 0;
		float nearFitness = 0;
		float avoidFitness = 0;
		float fireFitness = 0;
				
		// Fitness for approaching a carrot
		for (int i = 0; i < distance.Count; i++) {
			nearSum += distance[i]; 
		}
		if (nearSum != 0 && distance.Count != 0)
			nearFitness = 1f/(nearSum/distance.Count);
		nearFitness = nearFitness * nearSlider;
		//Debug.Log ("Near Fitness: " + nearFitness);
		
		// Fitness for avoiding enemies
		
		

		// Fitness for firing
		fireFitness = 1 - (1 / firing);
		fireFitness = fireFitness * fireSlider;
		//Debug.Log ("Firing Fitness: " + fireFitness);
			
			
		return nearFitness + fireFitness;
	}
	
	public void setSliders(float near, float fire) {
		nearSlider = near;
		fireSlider = fire;
    }
	
}
