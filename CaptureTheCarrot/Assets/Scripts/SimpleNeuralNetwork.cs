using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
		public class SimpleNeuralNetwork : INeuralNetwork
		{
				private Random gen = new Random();
		
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
								double randomWeight = gen.Next(-5, 5)/100.0;
								this._connectionGenes.Add(new ConnectionGene(innovationNum, fromNode, toNode, randomWeight));
								innovationNum++;
							}										
						}
				}
				
				public SimpleNeuralNetwork (SimpleNeuralNetwork parent1, SimpleNeuralNetwork parent2)
				{
					List<NodeGene> nodes1 = parent1.GetNodes();
					List<NodeGene> nodes2 = parent2.GetNodes();
					List<ConnectionGene> connections1 = parent1.GetConnections();
					List<ConnectionGene> connections2 = parent2.GetConnections();
					
					List<NodeGene> nodeIntersection = new List<NodeGene>();
					List<NodeGene> node1Disjoint = new List<NodeGene>();
					List<NodeGene> node2Disjoint = new List<NodeGene>();
					foreach(NodeGene n1 in nodes1)
					{
						bool found = false;
						foreach(NodeGene n2 in nodes2)
						{
							if(n1.Equals(n2))
							{
								nodeIntersection.Add(new NodeGene(n1));
								found = true;
							}
						}
						if(!found)
						{
							node1Disjoint.Add(new NodeGene(n1));
						}
					}
					foreach(NodeGene n2 in nodes2)
					{
						bool found = false;
						foreach(NodeGene n1 in nodes1)
						{
							if(n1.Equals(n2))
							{
								found = true;
							}
						}
						if(!found)
						{
							node2Disjoint.Add(new NodeGene(n2));
						}
					}
					
					// Find the intersection of connections, as well as the disjoints
					List<ConnectionGene> connectionsIntersection = new List<ConnectionGene>();
					List<ConnectionGene> connections1Disjoint = new List<ConnectionGene>();
					List<ConnectionGene> connections2Disjoint = new List<ConnectionGene>();
					foreach(ConnectionGene c1 in connections1)
					{
						bool found = false;
						foreach(ConnectionGene c2 in connections2)
						{
							if(c1.Equals(c2))
							{
								connectionsIntersection.Add(new ConnectionGene(c1));
								found = true;
							}
						}
						if(!found)
						{
							connections1Disjoint.Add(new ConnectionGene(c1));
						}
						
					}
					foreach(ConnectionGene c2 in connections2)
					{
						bool found = false;
						foreach(ConnectionGene c1 in connections1)
						{
							if(c1.Equals(c2))
							{
								found = true;
							}
						}
						if(!found)
						{
							connections2Disjoint.Add(new ConnectionGene(c2));
						}						
					}
					
					// Make this neural network's connections the intersection + the disjoint according to the fitness function
					_nodeGenes = nodeIntersection;
					_connectionGenes = connectionsIntersection;
										
					int eval1 = parent1.Evaluate();
					int eval2 = parent2.Evaluate();
					if(eval1 > eval2)
					{
						foreach(NodeGene n1 in node1Disjoint)
						{
							_nodeGenes.Add(n1);
						}
						foreach(ConnectionGene c1 in connections1Disjoint)
						{
							_connectionGenes.Add(c1);
						}
					}
					else if(eval2 > eval1)
					{
						foreach(NodeGene n2 in node2Disjoint)
						{
							_nodeGenes.Add(n2);
						}
						foreach(ConnectionGene c2 in connections2Disjoint)
						{
							_connectionGenes.Add(c2);
						}
					}
					else
					{
						// If they are equal -- add the disjoint nodes from both sets
						foreach(NodeGene n1 in node1Disjoint)
						{
							_nodeGenes.Add(n1);
						}
						foreach(NodeGene n2 in node2Disjoint)
						{
							_nodeGenes.Add(n2);
						}
						foreach(ConnectionGene c1 in connections1Disjoint)
						{
							_connectionGenes.Add(c1);
						}
						foreach(ConnectionGene c2 in connections2Disjoint)
						{
							_connectionGenes.Add(c2);
						}
					}
					
					// Figure out the input count and output count
					foreach(NodeGene n in _nodeGenes)
					{
						if(n.type == NodeType.Input)
						{
							this._inputCount++;
						}
						if(n.type == NodeType.Output)
						{
							this._outputCount++;
						}
					}
					
					this._inputArray = new float[this._inputCount];
					this._outputArray = new float[this._outputCount];					
				}
				
				public void changeWeights()
				{
					
				}
				
				private void addConnection()
				{
					// TODO: add connections to mutate the network
				}
				
				private void addNode()
				{
					// TODO: add nodes to mutate the network
				}
				
				public List<NodeGene> GetNodes()
				{
					return _nodeGenes;
				}
				
				public List<ConnectionGene> GetConnections()
				{
					return _connectionGenes;
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
					for (int j =0; j < this._outputCount; j++)
					{
						double value = 0;
						
						// Calculate the value based on the weights of the connections to that output node
						for (int i =0; i < this._inputCount; i++){
							value += this._inputArray[i]*this._connectionGenes[i+j].weight;
						}
						this._outputArray[j] = (float)value;
					}
				}
				
				public double[] getWeights()
				{
					double[] weights = new double[_inputCount*_outputCount];
					for(int i=0; i<weights.Length; i++)
					{
						weights[i] = this._connectionGenes[i].weight;
					}
					return weights;
				}
				
				// Return a number representing how good the neural network is
				public int Evaluate() 
				{
					return gen.Next();
				}
		}
}

