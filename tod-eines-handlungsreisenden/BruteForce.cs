using System.Collections.Generic;
using System.Linq;

namespace tod_eines_handlungsreisenden
{
    static class BruteForce
    {
        /// <summary>
        /// Solves the travelling salesman problem using brute force
        /// </summary>
        /// <param name="matrix">A square matrix containing the edge costs</param>
        /// <returns>
        /// A tuple containing the shortest length and an ordered list
        /// of the stops on the shortest tour (1-indexed)
        /// </returns>
        public static (double, IList<int>) Solve(double[,] matrix)
        {
            var optimalTour = new List<int>();
            var optimalLength = double.MaxValue;

            var permutations = Enumerable.Range(2, matrix.GetLength(0) - 1).Permutate();

            // iterate through all possible permutations and check if the length for
            // the given permutation is shorter than all previously seen permutations
            foreach (var permutation in permutations)
            {
                var currentTour = permutation.Prepend(1).Append(1).ToList();
                var currentLength = 0d;

                for (int i = 0; i < currentTour.Count() - 1; i++)
                {
                    currentLength += matrix[currentTour[i] - 1, currentTour[i + 1] - 1];
                }

                if (currentLength < optimalLength)
                {
                    optimalLength = currentLength;
                    optimalTour = currentTour;
                }
            }

            optimalTour.Reverse();

            return (optimalLength, optimalTour);
        }
    }
}
