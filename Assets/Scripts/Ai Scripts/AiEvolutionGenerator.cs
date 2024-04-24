using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiEvolutionGenerator : MonoBehaviour
{
    // Population size
    public int populationSize = 10;

    // Range for random number generation
    public int minNumber = 0;
    public int maxNumber = 100;

    // Current population of numbers
    private List<int> population = new List<int>();
    private int tournamentSize = 10;
    private float mutationRat = 5;
    private float mutationRate = 4;

    void Start()
    {
        // Initialize the population with random numbers
        InitializePopulation();

        // Perform evolution
        Evolve();
    }

    void InitializePopulation()
    {
        for (int i = 0; i < populationSize; i++)
        {
            int randomNum = UnityEngine.Random.Range(minNumber, maxNumber + 1);
            population.Add(randomNum);
        }
    }

    void Evolve()
    {
        // Main evolution loop (for demonstration purposes, loop only once)
        for (int generation = 0; generation < 1; generation++)
        {
            // Display current population
            Debug.Log("Generation " + generation + ": " + string.Join(", ", population));

            // Get user feedback for each number in the population
            List<bool> feedback = GetUserFeedback();

            // Select parents based on tournament selection
            List<int> parents = TournamentSelection(feedback);

            // Generate offspring through crossover and mutation
            List<int> offspring = Crossover(parents);

            // Replace the worst individuals in the population with offspring
            ReplaceWorstWithOffspring(offspring, feedback);

            // Display the evolved population
            Debug.Log("Evolved Population: " + string.Join(", ", population));
        }
    }

    List<bool> GetUserFeedback()
    {
        // For demonstration purposes, randomly generate feedback (good or bad) for each number
        List<bool> feedback = new List<bool>();
        for (int i = 0; i < populationSize; i++)
        {
            bool isGood = (UnityEngine.Random.value > 0.5f); // 50% chance of being good or bad
            feedback.Add(isGood);
        }
        return feedback;
    }

    List<int> TournamentSelection(List<bool> feedback)
    {
        List<int> parents = new List<int>();
        for (int i = 0; i < populationSize; i++)
        {
            // Randomly select individuals for the tournament
            List<int> tournamentParticipants = new List<int>();
            for (int j = 0; j < tournamentSize; j++)
            {
                tournamentParticipants.Add(UnityEngine.Random.Range(0, populationSize));
            }

            // Find the winner (individual with the best fitness)
            int winnerIndex = tournamentParticipants[0];
            for (int j = 1; j < tournamentSize; j++)
            {
                if (feedback[tournamentParticipants[j]] && !feedback[winnerIndex])
                {
                    winnerIndex = tournamentParticipants[j];
                }
            }
            parents.Add(population[winnerIndex]);
        }
        return parents;
    }

    List<int> Crossover(List<int> parents)
    {
        List<int> offspring = new List<int>();
        for (int i = 0; i < populationSize; i += 2)
        {
            // Select two parents for crossover
            int parent1 = parents[i];
            int parent2 = parents[i + 1];

            // Perform single-point crossover
            int crossoverPoint = UnityEngine.Random.Range(1, 8);
            int child1 = (parent1 & (255 << crossoverPoint)) | (parent2 & ~(255 << crossoverPoint));
            int child2 = (parent2 & (255 << crossoverPoint)) | (parent1 & ~(255 << crossoverPoint));

            // Mutate offspring
            child1 = Mutate(child1);
            child2 = Mutate(child2);

            offspring.Add(child1);
            offspring.Add(child2);
        }
        return offspring;
    }

    int Mutate(int offspring)
    {
        // Randomly mutate each bit in the offspring with a given mutation rate
        for (int i = 0; i < 8; i++)
        {
            if (UnityEngine.Random.value < mutationRate)
            {
                offspring ^= (1 << i);
            }
        }
        return offspring;
    }

    void ReplaceWorstWithOffspring(List<int> offspring, List<bool> feedback)
    {
        // Replace the worst individuals in the population with offspring
        for (int i = 0; i < populationSize; i++)
        {
            if (!feedback[i])
            {
                population[i] = offspring[i];
            }
        }
    }
}

