using System;
using System.Collections.Generic;
using System.Linq;

namespace tod_eines_handlungsreisenden
{
    static class HeldKarp
    {
        /// <summary>
        /// Solves the travelling salesman problem using the Held-Karp algorithm
        /// </summary>
        /// <param name="matrix">A square matrix containing the edge costs</param>
        /// <returns>
        /// A tuple containing the shortest length and an ordered list
        /// of the stops on the shortest tour (1-indexed)
        /// </returns>
        public static (double, IList<int>) Solve(double[,] matrix)
        {
            var sets = InitializeSets(matrix);
            CalculateIntermediateSets(matrix, ref sets);
            return CalculateFinalSet(matrix, sets);
        }

        /// <summary>
        /// Initializes the dictionary with all trivial sets of size one
        /// </summary>
        /// <param name="matrix">A square matrix containing the edge costs</param>
        /// <returns>A dictionary containing all trivial sets</returns>
        private static Dictionary<(int, int), Set> InitializeSets(double[,] matrix)
        {
            var results = new Dictionary<(int, int), Set>();

            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                results.Add((1 << i, i), new Set(i, matrix[i, 0]));
            }

            return results;
        }

        /// <summary>
        /// Calculates all intermediate sets while utilizing previously calculated sets
        /// </summary>
        /// <param name="matrix">A square matrix containing the edge costs</param>
        /// <param name="sets">A dictionary containing all trivial sets</param>
        private static void CalculateIntermediateSets(double[,] matrix, ref Dictionary<(int, int), Set> sets)
        {
            for (int setSize = 2; setSize < matrix.GetLength(0); setSize++)
            {
                var generatedSets = Helper.GenerateSets(1, matrix.GetLength(0) - 1, setSize);

                foreach (var currentSet in generatedSets)
                {
                    var currentMask = currentSet.Aggregate(0, (mask, vertex) => mask |= 1 << vertex);

                    foreach (var currentVertex in currentSet)
                    {
                        var previousMask = currentMask & ~(1 << currentVertex);
                        var possibleSets = new List<Set>();

                        foreach (var previousVertex in currentSet)
                        {
                            if (previousVertex != currentVertex)
                            {
                                var previousCost = sets[(previousMask, previousVertex)].MinimumCost;
                                var additionalCost = matrix[currentVertex, previousVertex];
                                possibleSets.Add(new Set(previousVertex, previousCost + additionalCost));
                            }
                        }

                        var optimalSet = possibleSets.OrderBy(x => x.MinimumCost).First();
                        sets.Add((currentMask, currentVertex), optimalSet);
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the last set containing all vertices while utilizing the intermediate sets
        /// </summary>
        /// <param name="matrix">A square matrix containing the edge costs</param>
        /// <param name="sets">A dictionary containing the intermediate sets</param>
        /// <returns></returns>
        private static (double, IList<int>) CalculateFinalSet(double[,] matrix, Dictionary<(int, int), Set> sets)
        {
            var currentMask = (int)(Math.Pow(2, matrix.GetLength(0)) - 2);
            var possibleSolutions = new List<Set>();
            var vertices = Enumerable.Range(1, matrix.GetLength(0) - 1);

            foreach (var vertex in vertices)
            {
                var previousCost = sets[(currentMask, vertex)].MinimumCost;
                var additionalCost = matrix[0, vertex];
                possibleSolutions.Add(new Set(vertex, previousCost + additionalCost));
            }

            var optimalSet = possibleSolutions.OrderBy(x => x.MinimumCost).First();

            // backtrack and construct solution by reading the previous vertex of the optimal intermediate sets
            var optimalTour = new List<int> { 0 };
            var previousVertex = optimalSet.PreviousVertex;

            for (int i = 0; i < matrix.GetLength(0) - 1; i++)
            {
                optimalTour.Add(previousVertex);
                var previousMask = currentMask & ~(1 << previousVertex);
                previousVertex = sets[(currentMask, previousVertex)].PreviousVertex;
                currentMask = previousMask;
            }

            optimalTour.Add(0);
            optimalTour.Reverse();
            optimalTour = optimalTour.Select(num => num + 1).ToList();

            return (optimalSet.MinimumCost, optimalTour);
        }

        /// <summary>
        /// Set
        /// </summary>
        private class Set
        {
            public int PreviousVertex;
            public double MinimumCost;

            public Set(int previousVertex, double minimumCost)
            {
                PreviousVertex = previousVertex;
                MinimumCost = minimumCost;
            }
        }
    }
}
