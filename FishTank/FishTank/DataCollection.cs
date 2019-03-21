using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FishTank.Anima;
using ModularGenetics;

namespace FishTank
{
    public class DataCollection
    {
        private const string DATA_DIRECTORY = "Data";

        private enum DataRecording
        {
            AverageFitness,
            Population,
            AverageGeneticDiversity
        }

        //Object
        private string savePath;
        private int tickResolution;

        public DataCollection(int tickResolution)
        {
            this.tickResolution = tickResolution;
            savePath = Path.Combine(Directory.GetCurrentDirectory(), DATA_DIRECTORY, DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));

            for (int i = 0; i < dataCollections.Length; i++) dataCollections[i] = new List<object>();
        }

        private List<object>[] dataCollections = new List<object>[Enum.GetNames(typeof(DataRecording)).Length];
        private int collectionCounter = 0;
        public void UpdateCollector(Tank fishTank)
        {
            if (++collectionCounter >= tickResolution)
            {
                collectionCounter = 0;
                dataCollections[(int)DataRecording.AverageFitness].Add(GetAverageFitness(fishTank));
                dataCollections[(int)DataRecording.Population].Add(GetPopulation(fishTank));
            }
        }

        public void SaveData()
        {
            string[] names = Enum.GetNames(typeof(DataRecording));
            Directory.CreateDirectory(savePath);
            for (int i = 0; i < dataCollections.Length; i++)
            {
                File.WriteAllLines(Path.Combine(savePath, names[i] + ".csv"), dataCollections[i].Select(x => string.Join(",", x)));
            }
        }

        private int GetPopulation(Tank fishTank)
        {
            return fishTank.ContainedEntities.Where(entity => entity is Fish).Count();
        }

        private double GetAverageFitness(Tank fishTank)
        {
            Entity[] fish = fishTank.ContainedEntities.Where(entity => entity is Fish).ToArray();
            return fish.Sum(entity => ((Fish)entity).Fitness) / fish.Length;
        }

        //ASSUMES EQUAL GENOMES BETWEEN FISH
        private double GetAverageGeneticDiversity(Tank fishTank)
        {
            Entity[] fish = fishTank.ContainedEntities.Where(entity => entity is Fish).ToArray();
            GeneticSequence[][] geneticSequences = fish.Select(entity => ((Fish)entity).ModularMember.Genome).ToArray()
                //Must transpose
        }
    }
}
