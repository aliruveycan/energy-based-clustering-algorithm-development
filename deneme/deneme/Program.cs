using System;
using System.Collections.Generic;

class Node
{
    public int ID = -1; // Düğümün kimlik numarası, başlangıç değeri atanıyor
    public double Energy = 100.0; // Düğümün mevcut enerji seviyesi, başlangıç değeri atanıyor
}

class Cluster
{
    public int ClusterID; // Kümenin kimlik numarası
    public List<Node> Nodes; // Kümedeki düğümlerin listesi
    public double ClusterEnergy; // Kümenin toplam enerji seviyesi

    public Cluster(int id)
    {
        ClusterID = id;
        Nodes = new List<Node>();
        ClusterEnergy = 0;
    }
}

class EnergyBasedClustering
{
    List<Node> nodes; // Tüm düğümlerin listesi
    public List<Cluster> clusters; // Oluşturulan kümelerin listesi

    public EnergyBasedClustering(List<Node> nodeList)
    {
        nodes = nodeList;
        clusters = new List<Cluster>();
    }

    public void CreateClusters(int k)
    {
        // Kümelerin oluşturulması
        for (int i = 0; i < k; i++)
        {
            Cluster cluster = new Cluster(i);
            clusters.Add(cluster);
        }

        // Düğümlerin kümelere atanması
        foreach (Node node in nodes)
        {
            Cluster nearestCluster = FindNearestCluster(node);
            nearestCluster.Nodes.Add(node);
            nearestCluster.ClusterEnergy += node.Energy;
        }
    }

    private Cluster FindNearestCluster(Node node)
    {
        if (clusters.Count == 0)
        {
            // Eğer hiç küme yoksa, yeni bir küme oluşturulur
            return new Cluster(0);
        }

        Cluster nearestCluster = clusters[0];
        double minDistance = double.MaxValue; // Başlangıçta maksimum değer atanır

        foreach (Cluster cluster in clusters)
        {
            if (cluster.Nodes.Count == 0)
            {
                // Eğer kümenin hiç düğümü yoksa, bu küme geçersizdir ve diğer kümelere bakılır
                continue;
            }

            foreach (Node clusterNode in cluster.Nodes)
            {
                double distance = Distance(node, clusterNode);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCluster = cluster;
                }
            }
        }

        return nearestCluster;
    }

    private double Distance(Node node1, Node node2)
    {
        // İki düğüm arasındaki mesafenin hesaplanması
        return Math.Sqrt(Math.Pow((node1.ID - node2.ID), 2) + Math.Pow((node1.Energy - node2.Energy), 2));
    }
}

class Program
{
    static void Main(string[] args)
    {
        // Örnek düğümler oluşturuluyor
        List<Node> nodeList = new List<Node>
        {
            new Node { ID = 1, Energy = 50.0 },
            new Node { ID = 2, Energy = 70.0 },
            new Node { ID = 3, Energy = 80.0 }
        };

        // Enerji tabanlı kümeleme algoritması oluşturuluyor ve küme sayısı belirleniyor
        EnergyBasedClustering clustering = new EnergyBasedClustering(nodeList);
        clustering.CreateClusters(2);

        // Sonuçlar ekrana yazdırılıyor
        Console.WriteLine("Enerji tabanlı kümeleme algoritması başarıyla tamamlandı.");
        Console.WriteLine("Oluşturulan kümeler:");

        foreach (Cluster cluster in clustering.clusters)
        {
            Console.WriteLine($"Küme {cluster.ClusterID}: Toplam enerji = {cluster.ClusterEnergy}");

            Console.WriteLine("Küme içeriği:");
            foreach (Node node in cluster.Nodes)
            {
                Console.WriteLine($"Düğüm {node.ID}: Enerji = {node.Energy}");
            }
        }

        // Sonucun görüntülenmesi için bir tuşa basılması bekleniyor
        Console.WriteLine("Çıkmak için bir tuşa basınız...");
        Console.ReadKey();
    }
}
