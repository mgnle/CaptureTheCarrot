using System.Collections.Generic;
using UnityEngine;

public class FitnessEvaluator {
	
	float nearSlider;
	float avoidSlider;
	float fireSlider;
	float mudSlider;
	
	/* Calculates the fitness of the neural network based on the distance
	 * from the carrot and whether an enemy is in firing range.
	 * For distance, take the average of all distances and inverse it.
	 * For firing, ...not really sure. */
	public float Evaluate(List<int> carrotDistance, List<int> enemyDistance, List<int> mudDistance, int firing) {
		float nearSum = 0;
		float avoidSum = 0;
		float mudSum = 0;
		float nearFitness = 0;
		float avoidFitness = 0;
		float mudFitness = 0;
		float fireFitness = 0;
		
		// Fitness for approaching a carrot
		for (int i = 0; i < carrotDistance.Count; i++) {
			nearSum += carrotDistance[i]; 
		}
		if (nearSum != 0 && carrotDistance.Count != 0)
			nearFitness = 1f/(nearSum/carrotDistance.Count);
		nearFitness = nearFitness * nearSlider;
		//Debug.Log ("Near Fitness: " + nearFitness);
		
		// Fitness for avoiding enemies
		for (int i = 0; i < enemyDistance.Count; i++) {
			avoidSum += enemyDistance[i];
		}
		if (avoidSum != 0 && enemyDistance.Count != 0)
			avoidFitness = 1f/(avoidSum/enemyDistance.Count);
		avoidFitness = avoidFitness * avoidSlider;
		//Debug.Log ("Avoid Fitness: " + avoidFitness);
		
		// Fitness for avoiding mud pits
		for (int i = 0; i < mudDistance.Count; i++) {
			mudSum += mudDistance[i];
		}
		if (mudSum != 0 && mudDistance.Count != 0)
			avoidFitness = 1f/(mudSum/mudDistance.Count);
		mudFitness = mudFitness * mudSlider;
		//Debug.Log ("Mud Fitness: " + mudFitness);

		// Fitness for firing
		fireFitness = 1 - (1 / firing);
		fireFitness = fireFitness * fireSlider;
		//Debug.Log ("Firing Fitness: " + fireFitness);
			
			
		return nearFitness + avoidFitness + fireFitness;
	}
	
	public void setSliders(float near, float avoid, float mud, float fire) {
		nearSlider = near;
		avoidSlider = avoid;
		mudSlider = mud;
		fireSlider = fire;
    }
	
}
