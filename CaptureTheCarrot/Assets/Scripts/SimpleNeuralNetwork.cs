using UnityEngine;
using System;
using System.Collections.Generic;

namespace AssemblyCSharp
{
	public class SimpleNeuralNetwork : INeuralNetwork
	{
			private System.Random gen = new System.Random();
	
			private static int nodeID;	
			private static int innovationNum;

			readonly int _inputCount;
			readonly int _outputCount;
			public float[] _inputArray;
			public float[] _outputArray;
			
			private List<NodeGene> _nodeGenes;
			private List<ConnectionGene> _connectionGenes;
    
			private FitnessEvaluator fitEval;
			private List<int> distance;
			private int firing;
        
			private Dictionary<int, List<ConnectionGene>> _adjacencyList;

			public SimpleNeuralNetwork (int inputCount, int outputCount)
			{ 
					fitEval = new FitnessEvaluator();
    
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
											
					this._adjacencyList = new Dictionary<int, List<ConnectionGene>>();
											
					// Create the input nodes
					for(int i=0; i<inputCount; i++)
					{
						NodeGene toAdd = new NodeGene(nodeID, NodeType.Input);
						this._nodeGenes.Add(toAdd);	
						
						// Instantiate adjacency list for this node
						List<ConnectionGene> newList = new List<ConnectionGene>();
						this._adjacencyList.Add(toAdd.nodeID, newList);
						
						nodeID++;
					}
					
					// Create the output nodes
					for(int i=0; i<outputCount; i++)
					{
						NodeGene toAdd = new NodeGene(nodeID, NodeType.Output);
						this._nodeGenes.Add(toAdd);	
						
						// Instantiate adjacency list for this node
						List<ConnectionGene> newList = new List<ConnectionGene>();
						this._adjacencyList.Add(toAdd.nodeID, newList);
													
						nodeID++;
					}
					
					// Create the connections - 1 for each input to output pair of nodes
					for(int i=0; i<inputCount; i++)
					{
						int fromNode = i;
						for(int j=0; j<outputCount; j++)
						{
							int toNode = j+inputCount;
							double randomWeight = ((double)gen.Next(-100,100))/100.0;
							ConnectionGene toAdd = new ConnectionGene(innovationNum, fromNode, toNode, randomWeight);
							this._connectionGenes.Add(toAdd);
							
							// Add to the adjacency list for this node
							List<ConnectionGene> list = this._adjacencyList[toNode];
							list.Add(toAdd);
			
							innovationNum++;
						}
					}
			}
			
			public SimpleNeuralNetwork(SimpleNeuralNetwork parent1, SimpleNeuralNetwork parent2)
			{	
				fitEval = new FitnessEvaluator();
		
				_adjacencyList = new Dictionary<int, List<ConnectionGene>>();
				
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
							this._adjacencyList.Add(n1.nodeID, new List<ConnectionGene>());
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
							ConnectionGene toAdd = new ConnectionGene(c1);
							connectionsIntersection.Add(toAdd);
							this._adjacencyList[c1.nodeOut].Add(toAdd);
					
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
									
				float eval1 = parent1.Evaluate();
				float eval2 = parent2.Evaluate();
				if(eval1 > eval2)
				{
					foreach(NodeGene n1 in node1Disjoint)
					{
						_nodeGenes.Add(n1);
						
						_adjacencyList.Add(n1.nodeID, new List<ConnectionGene>());
					}
					foreach(ConnectionGene c1 in connections1Disjoint)
					{
						_connectionGenes.Add(c1);
						
						_adjacencyList[c1.nodeOut].Add(c1);
					}
				}
				else if(eval2 > eval1)
				{
					foreach(NodeGene n2 in node2Disjoint)
					{
						_nodeGenes.Add(n2);
						
						this._adjacencyList.Add(n2.nodeID, new List<ConnectionGene>());
					}
					foreach(ConnectionGene c2 in connections2Disjoint)
					{
						_connectionGenes.Add(c2);
						
						_adjacencyList[c2.nodeOut].Add(c2);
					}
				}
				else
				{
					// If they are equal -- add the disjoint nodes from both sets
					foreach(NodeGene n1 in node1Disjoint)
					{
						_nodeGenes.Add(n1);
						
						_adjacencyList.Add(n1.nodeID, new List<ConnectionGene>());
					}
					foreach(NodeGene n2 in node2Disjoint)
					{
						_nodeGenes.Add(n2);
						
						_adjacencyList.Add(n2.nodeID, new List<ConnectionGene>());
					}
					foreach(ConnectionGene c1 in connections1Disjoint)
					{
						_connectionGenes.Add(c1);
						
						_adjacencyList[c1.nodeOut].Add(c1);
					}
					foreach(ConnectionGene c2 in connections2Disjoint)
					{
						_connectionGenes.Add(c2);
						
						_adjacencyList[c2.nodeOut].Add(c2);
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
				
				// Randomly mutate neural network weights and structure after creating new brain from parent brains
				mutate();				
			}
			
			// Randomly mutate the network
			public void mutate()
			{
				changeWeights();
				
				double prob = gen.NextDouble();
				if(prob <= Constants.PROBABILITY_ADD_NODE)
				{
					addNode();
				}
				
				prob = gen.NextDouble();
				if(prob <= Constants.PROBABILITY_ADD_CONNECTION)
				{
					addConnection();
				}
			}
			
			public void changeWeights()
			{
				foreach(ConnectionGene c in this._connectionGenes)
				{
					double prob = gen.NextDouble();
					if(prob <= Constants.PROBABILITY_MUTATE_WEIGHT)
					{
						double change = (gen.Next(-1*Constants.AMOUNT_MUTATE_WEIGHT, Constants.AMOUNT_MUTATE_WEIGHT))/100.0;
						if (c.weight+change < 0)
						{
							c.weight = 0;
						}
						else if(c.weight+change > 1)
						{
							c.weight = 1;
						}
						else
						{
							c.weight += change;
						}
					}
				}
			}
			
			private void addConnection()
			{
				// TODO: add connections to mutate the network
				while(true)
				{
					int index = gen.Next(0, this._nodeGenes.Count);
					NodeGene toConnect = this._nodeGenes[index];
					
					List<NodeGene> possibilities = new List<NodeGene>();
					foreach(NodeGene n in this._nodeGenes)
					{
						// If I am not looking at the same node AND
						// If I'm a hidden node OR
						// I'm an Input/Output node and the node I'm looking at is not the same type
						if(!n.Equals(toConnect) &&
						   (toConnect.type == NodeType.Hidden || (toConnect.type != NodeType.Hidden && toConnect.type != n.type)))
						{
							possibilities.Add(n);
						}
					}
					
					if(possibilities.Count == 0)
					{
						continue;
					}
					
					int nodeIndex = gen.Next(0, possibilities.Count);
					NodeGene toConnect2 = possibilities[nodeIndex];
					
					NodeGene nodeIn = toConnect;
					NodeGene nodeOut = toConnect2;
					
					if(nodeOut.type == NodeType.Input || nodeIn.type == NodeType.Output)
					{
						// Swap them so the connection is created in the right direction
						NodeGene temp = nodeIn;
						nodeIn = nodeOut;
						nodeOut = temp;
					}
					double randomWeight = ((double)gen.Next(-100,100))/100.0;						
					ConnectionGene toAdd = new ConnectionGene(innovationNum++, nodeIn.nodeID, nodeOut.nodeID, randomWeight);
					this._connectionGenes.Add(toAdd);
					
					// Add to the adjacency list for this node
					List<ConnectionGene> list = this._adjacencyList[toAdd.nodeOut];
					list.Add(toAdd);
					break;						
				}
				
			}
			
			private void addNode()
			{
				// Take an existing connection, and split it.
				int index = gen.Next(0, this._connectionGenes.Count);
				ConnectionGene toDisable = this._connectionGenes[index];
				NodeGene toAdd = new NodeGene(nodeID++, NodeType.Hidden);
				
				this._nodeGenes.Add(toAdd);
				
				this._adjacencyList.Add(toAdd.nodeID, new List<ConnectionGene>());
				
				ConnectionGene connect1 = new ConnectionGene(innovationNum++, toDisable.nodeIn, toAdd.nodeID, 1);
				ConnectionGene connect2 = new ConnectionGene(innovationNum++, toAdd.nodeID, toDisable.nodeOut, toDisable.weight);
				
				toDisable.enabled = false;
				// Maybe just remove this connection?
				
				this._connectionGenes.Add(connect1);
				this._connectionGenes.Add(connect2);
				
				// Add to the adjacency list for this node
				List<ConnectionGene> list = this._adjacencyList[connect1.nodeOut];
				list.Add(connect1);	
				
				// Add to the adjacency list for this node
				List<ConnectionGene> list2 = this._adjacencyList[connect2.nodeOut];
				list2.Add(connect2);			
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
					this._outputArray[j] = Activate(this._nodeGenes[j+this._inputCount].nodeID);
				}
			}
			
			public float Activate (int nodeID)
			{
				// Base case - did you reach an input node?
				if(nodeID < this._inputCount)
				{
					float input = this._inputArray[nodeID];
					//if(input < -1) input = -1;
					//if(input > 1) input = 1;
					return input;
				}
				
				// Calculate weighted sum of inputs
				float sum = 0;
				foreach (ConnectionGene c in this._adjacencyList[nodeID])
				{
					sum += ((float)c.weight*Activate(c.nodeIn));
					
				}
				return sum;
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
			
			public void DistanceFrom(SimpleNeuralNetwork network2, out int disjoint, out int N, out double weightedAverage)
			{	
				List<NodeGene> nodes1 = this._nodeGenes;
				List<NodeGene> nodes2 = network2.GetNodes();
				List<ConnectionGene> connections1 = this._connectionGenes;
				List<ConnectionGene> connections2 = network2.GetConnections();
				
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
				
				weightedAverage = 0;
				
				foreach(ConnectionGene c1 in connections1)
				{
					bool found = false;
					foreach(ConnectionGene c2 in connections2)
					{
						if(c1.Equals(c2))
						{
	                        ConnectionGene toAdd = new ConnectionGene(c1);
	                        connectionsIntersection.Add(toAdd);
	                        
	                        weightedAverage += Math.Abs(c1.weight-c2.weight);
	                        
	                        found = true;
	                    }
	                }
	                if(!found)
	                {
	                    connections1Disjoint.Add(new ConnectionGene(c1));
	                }
	            }
	            weightedAverage /= connectionsIntersection.Count;
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
	            
	        
	            disjoint = connections1Disjoint.Count + connections2Disjoint.Count;
	            N = Math.Max(nodes1.Count, nodes2.Count);
	        }   
            
            public void SaveNetworkToFile()
            {
                // TODO
            }
            
            public void LoadNetworkFromFile()
			{
	        	// TODO
	        }

			/* Calculates the distance from a bunny and a value for if the
		 	 * bunny is facing an enemy. Should be called on every move.*/
			public void UpdateEvaluator(List<int> distance, int firing) {
				this.distance = distance;
				this.firing = firing;
			}
					
			public float Evaluate() {
				if (distance != null)
					return fitEval.Evaluate(distance, firing);
				return 0;
			}
			
			public void setSliders(float near, float fire) {
				fitEval.setSliders(near, fire);
	        }
	}
}

