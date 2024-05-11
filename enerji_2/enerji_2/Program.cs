using System;
using System.Collections.Generic;
using System.Linq;

class Node
{
    public int ID { get; set; }
    public double Energy { get; set; }
    public bool Clustered { get; set; }
    public int ClusterID { get; set; }
}

class Cluster
{
    public int ID { get; set; }
    public List<Node> Nodes { get; set; }
}

class LEACHAlgorithm
{
    private List<Node> nodes;
    private List<Cluster> clusters;
    private int round;

    public LEACHAlgorithm(List<Node> nodeList)
    {
        nodes = nodeList;
        clusters = new List<Cluster>();
        round = 1;
    }

    public void Run(int k, double threshold)
    {
        while (nodes.Any(n => !n.Clustered))
        {
            FormClusters(k, threshold);
            round++;
            RenewClusterHead();
        }
        DisplayClusters(); // Sonuçları ekrana yazdır
        Console.WriteLine("Çıkmak için bir tuşa basınız...");
        Console.ReadKey(); // Kapatmak için bir tuşa basılmasını bekler
    }

    private void FormClusters(int k, double threshold)
    {
        clusters.Clear();
        Random rnd = new Random();
        for (int i = 0; i < k; i++)
        {
            int randomNodeIndex = rnd.Next(nodes.Count);
            Node clusterHead = nodes[randomNodeIndex];
            Cluster cluster = new Cluster { ID = i, Nodes = new List<Node> { clusterHead } };
            clusterHead.Clustered = true;
            clusterHead.ClusterID = i;
            clusters.Add(cluster);
        }

        foreach (Node node in nodes)
        {
            if (!node.Clustered)
            {
                Cluster nearestCluster = null;
                double minDistance = double.MaxValue;

                foreach (Cluster cluster in clusters)
                {
                    double distance = Distance(node, cluster.Nodes[0]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestCluster = cluster;
                    }
                }

                if (minDistance <= threshold)
                {
                    nearestCluster.Nodes.Add(node);
                    node.Clustered = true;
                    node.ClusterID = nearestCluster.ID;
                }
            }
        }
    }

    private void RenewClusterHead()
    {
        foreach (Cluster cluster in clusters)
        {
            double minEnergy = double.MaxValue;
            Node newClusterHead = null;
            foreach (Node node in cluster.Nodes)
            {
                if (node.Energy < minEnergy)
                {
                    minEnergy = node.Energy;
                    newClusterHead = node;
                }
            }

            foreach (Node node in cluster.Nodes)
            {
                node.Clustered = false;
            }

            cluster.Nodes.Clear();
            newClusterHead.Clustered = true;
            newClusterHead.ClusterID = cluster.ID;
            cluster.Nodes.Add(newClusterHead);
        }
    }

    private double Distance(Node node1, Node node2)
    {
        return Math.Sqrt(Math.Pow((node1.ID - node2.ID), 2) + Math.Pow((node1.Energy - node2.Energy), 2));
    }

    public void DisplayClusters()
    {
        Console.WriteLine($"Round: {round}");
        foreach (Cluster cluster in clusters)
        {
            Console.WriteLine($"Cluster {cluster.ID}:");
            foreach (Node node in cluster.Nodes)
            {
                Console.WriteLine($"Node {node.ID}, Energy: {node.Energy}");
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<Node> nodeList = new List<Node>
        {
            new Node { ID = 1, Energy = 50 },
            new Node { ID = 2, Energy = 70 },
            new Node { ID = 3, Energy = 80 },
            new Node { ID = 4, Energy = 60 },
            new Node { ID = 5, Energy = 90 }
        };

        LEACHAlgorithm leach = new LEACHAlgorithm(nodeList);
        leach.Run(2, 20);
    }
}
