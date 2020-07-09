using System.Collections.Generic;
using System.Linq;

namespace tod_eines_handlungsreisenden
{
    static class Helper
    {
        /// <summary>
        /// Generates a list containing all permutations of a given list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Permutate<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null || !enumerable.Any())
                yield return Enumerable.Empty<T>();

            foreach (var selected in enumerable)
            {
                foreach (var perm in enumerable.Where(item => !item.Equals(selected)).Permutate())
                {
                    yield return perm.Prepend(selected);
                }
            }
        }

        /// <summary>
        /// Generates all possible sets of a given length for the numbers from min to max
        /// </summary>
        /// <param name="min">Minimum value (inclusive)</param>
        /// <param name="max">Maximum value (inclusive)</param>
        /// <param name="size">Length of the sets to generate</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<int>> GenerateSets(int min, int max, int size)
        {
            var sequences = Enumerable.Range(min, max).Select(x => new List<int> { x });
            var result = new List<List<int>> { new List<int>() };

            foreach (var sequence in sequences)
            {
                int length = result.Count;

                for (int i = 0; i < length; i++)
                {
                    if (result[i].Count >= size)
                        continue;

                    result.Add(result[i].Concat(sequence).ToList());
                }
            }

            return result.Where(x => x.Count == size);
        }
    }
}
