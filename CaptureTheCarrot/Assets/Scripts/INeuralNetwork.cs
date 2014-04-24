using System;
namespace AssemblyCSharp
{
		public interface INeuralNetwork
		{
				/// <summary>
				/// Gets the number of inputs to the neural network. This is assumed to be fixed for the lifetime of the NeuralNetwork.
				/// </summary>
				int InputCount { get; }
				
				/// <summary>
				/// Gets the number of outputs from the neural network. This is assumed to be fixed for the lifetime of the NeuralNetwork.
				/// </summary>
				int OutputCount { get; }
				
				/// <summary>
				/// Gets an array of input values that feed into the neural network 
				/// </summary>
				float[] InputSignalArray { get; set; }

				/// <summary>
				/// Gets an array of output values that feed out from the neural network
				/// </summary>
				float[] OutputSignalArray { get; }
								
				/// <summary>
				/// Activate the neural network. This is a request for the network to accept its inputs and produce output signals
				/// ready for reading from OutputSignalArray.
				/// </summary>
				void Activate();
				
				double[] getWeights();
				
				int Evaluate();
		}
}

