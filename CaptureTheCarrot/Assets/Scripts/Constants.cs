using System;
namespace AssemblyCSharp
{
		public class Constants
		{
				public const int INPUTS = 6+1;	// 1 bias node
				public const int OUTPUTS = 3;
        
				public const int NUM_BUNNIES = 30;
		
				public const int TIME_ALIVE_THRESHOLD = 25;
		
				public const double PROBABILITY_MUTATE_WEIGHT = 1;
				public const int AMOUNT_MUTATE_WEIGHT = 5;	// In hundredths
				public const double PROBABILITY_ADD_NODE = 1;
				public const double PROBABILITY_ADD_CONNECTION = 1;
		}
}

