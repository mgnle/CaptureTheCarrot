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
						
						// Initial node list size = Inputs + Ouputs + 1 Hidden
						this._nodeGenes = new List<NodeGene>();
			
						// Initial connection list size = Inputs + Outputs
						this._connectionGenes = new List<ConnectionGene>();
						
						// Create the hidden node
						int hiddenId = nodeID;
						this._nodeGenes.Add(new NodeGene(nodeID, NodeType.Hidden));
						
						// Create the input nodes and connections from the inputs to the hidden node
						for(int i=0; i<inputCount; i++)
						{
							this._nodeGenes.Add(new NodeGene(nodeID, NodeType.Input));
							this._connectionGenes.Add(new ConnectionGene(innovationNum, nodeID, hiddenId, DEFAULT_WEIGHT));							
							nodeID++;
							innovationNum++;
						}
						
						// Create the output nodes and connections from the hidden node to the outputs
						for(int i=0; i<outputCount; i++)
						{
							this._nodeGenes.Add(new NodeGene(nodeID, NodeType.Output));								
							this._connectionGenes.Add(new ConnectionGene(innovationNum, hiddenId, nodeID, DEFAULT_WEIGHT));
							nodeID++;
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
					double value = 0;
					for (int i =0; i < this._inputCount; i++){
						value += this._inputArray[i]*this._connectionGenes[i].weight;
					}
										
					for (int i =0; i < this._outputCount; i++){
						this._outputArray[i] = (float)(value*this._connectionGenes[this._inputCount+i].weight);
					}
				}
		}
}

