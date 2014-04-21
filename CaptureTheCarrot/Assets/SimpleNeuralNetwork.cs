using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
		public class SimpleNeuralNetwork : INeuralNetwork
		{
				public const double DEFAULT_WEIGHT = 0.5;
		
				private static int nodeID;	
				private static int innovationNum;

				readonly int _inputCount;
				readonly int _outputCount;
				public float[] _inputArray;
				public float[] _outputArray;
				
				private List<NodeGene> _nodeGenes;
				private List<ConnectionGene> _connectionGenes;

				public SimpleNeuralNetwork (int inputCount, int outputCount)
				{ 
						this._inputCount = inputCount;
						this._outputCount = outputCount;

						this._inputArray = new float[inputCount];
						this._outputArray = new float[outputCount];
						
						nodeID = 0;
						innovationNum = 0;
						
						// Initial node list size = Inputs + Ouputs
						this._nodeGenes = new List<NodeGene>();
			
						// Initial connection list size = Inputs * Outputs
						this._connectionGenes = new List<ConnectionGene>();
												
						// Create the input nodes
						for(int i=0; i<inputCount; i++)
						{
							this._nodeGenes.Add(new NodeGene(nodeID, NodeType.Input));
							nodeID++;
						}
						
						// Create the output nodes
						for(int i=0; i<outputCount; i++)
						{
							this._nodeGenes.Add(new NodeGene(nodeID, NodeType.Output));								
							nodeID++;
						}
						
						// Create the connections - 1 for each input to output pair of nodes
						for(int i=0; i<inputCount; i++)
						{
							int fromNode = i;
							for(int j=0; j<outputCount; j++)
							{
								int toNode = j+inputCount;
								this._connectionGenes.Add(new ConnectionGene(innovationNum, fromNode, toNode, DEFAULT_WEIGHT));
								innovationNum++;
							}										
						}
				}
				
				public void addConnection()
				{
					// TODO: add connections to mutate the network
				}
				
				public void addNode()
				{
					// TODO: add nodes to mutate the network
				}
				
				public int InputCount {
						get { return _inputCount; }
				}
				
				public int OutputCount {
						get { return _outputCount; }
				}

				public float[] InputSignalArray {
						get { return _inputArray; }
						set { _inputArray = value; }
				}
				
				public float[] OutputSignalArray {
						get { return _outputArray; }
				}

				public void Activate ()
				{					
					// For each output node calculate the output value				
					for (int j =0; j < this._outputCount; j++){
						double value = 0;
						
						// Calculate the value based on the weights of the connections to that output node
						for (int i =0; i < this._inputCount; i++){
							value += this._inputArray[i]*this._connectionGenes[i+j].weight;
						}
						this._outputArray[j] = (float)value;
					}
				}
		}
}

