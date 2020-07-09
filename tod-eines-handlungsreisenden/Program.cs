using BAMCIS.GIS;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;

namespace tod_eines_handlungsreisenden
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// A program to solve the travelling salesman problem
        /// </summary>
        /// <param name="filepath">Path to the CSV file containing the sites</param>
        /// <param name="bruteForce">Use a brute force approach instead of the Held-Karp algorithm</param>
        static int Main(string filepath = "msg_standorte_deutschland.csv", bool bruteForce = false)
        {
            var sites = ReadSitesFromCsvFile(filepath);
            var matrix = CalculateMatrixFromSites(sites);

            if (!IsMatrixValid(matrix))
            {
                Console.WriteLine($"Error: Could not read values from the file. Make sure the file \"{filepath}\" exists and is not read-protected.");
                return -1;
            }

            if (bruteForce)
            {
                Console.WriteLine("Calculating optimal route using the brute force algorithm...");
                var stopwatch = Stopwatch.StartNew();
                var results = BruteForce.Solve(matrix);
                stopwatch.Stop();
                PrintResults(results, stopwatch, sites);
            }
            else
            {
                Console.WriteLine("Calculating optimal route using the Held–Karp algorithm...");
                var stopwatch = Stopwatch.StartNew();
                var results = HeldKarp.Solve(matrix);
                stopwatch.Stop();
                PrintResults(results, stopwatch, sites);
            }

            return 0;
        }

        /// <summary>
        /// Reads the sites from a given CSV file
        /// </summary>
        /// <param name="filePath">The path to the file</param>
        /// <returns>A list containing all sites</returns>
        private static List<Site> ReadSitesFromCsvFile(string filePath)
        {
            try
            {
                using var reader = new StreamReader(filePath);
                using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

                return csv.GetRecords<Site>().ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Calculates the distance matrix with distances between all sites
        /// </summary>
        /// <param name="sites">A list containing all sites</param>
        /// <returns>A square matrix containing the edge costs</returns>
        private static double[,] CalculateMatrixFromSites(List<Site> sites)
        {
            try
            {
                var total = sites.Count();
                var matrix = new double[total, total];

                for (int first = 0; first < total; first++)
                {
                    for (int second = 0; second < total; second++)
                    {
                        matrix[first, second] = GetDistance(sites[first], sites[second]);
                    }
                }

                return matrix;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Site
        /// </summary>
        public class Site
        {
            /// <summary>
            /// Index
            /// </summary>
            [Index(0)]
            public int Index { get; set; }

            /// <summary>
            /// Name
            /// </summary>
            [Index(1)]
            public string Name { get; set; }

            /// <summary>
            /// Street
            /// </summary>
            [Index(2)]
            public string Street { get; set; }

            /// <summary>
            /// HouseNumber
            /// </summary>
            [Index(3)]
            public string HouseNumber { get; set; }

            /// <summary>
            /// ZipCode
            /// </summary>
            [Index(4)]
            public string ZipCode { get; set; }

            /// <summary>
            /// City
            /// </summary>
            [Index(5)]
            public string City { get; set; }

            /// <summary>
            /// Latitude
            /// </summary>
            [Index(6)]
            public double Latitude { get; set; }

            /// <summary>
            /// Longitude
            /// </summary>
            [Index(7)]
            public double Longitude { get; set; }
        }

        /// <summary>
        /// Calculates the distance between two sites
        /// </summary>
        /// <param name="firstSite">The first site</param>
        /// <param name="secondSite">The second site</param>
        /// <returns>The distance between the two sites in kilometers</returns>
        private static double GetDistance(Site firstSite, Site secondSite)
        {
            var firstLocation = new GeoCoordinate(firstSite.Latitude, firstSite.Longitude);
            var secondLocation = new GeoCoordinate(secondSite.Latitude, secondSite.Longitude);

            return firstLocation.DistanceTo(secondLocation, DistanceType.KILOMETERS);
        }

        /// <summary>
        /// Checks whether the matrix contains any entries and is square
        /// </summary>
        /// <param name="matrix">A square matrix containing the edge costs</param>
        /// <returns>Whether the matrix fulfills the requirements</returns>
        private static bool IsMatrixValid(double[,] matrix)
        {
            if (matrix == null || matrix.GetLength(0) != matrix.GetLength(1))
                return false;

            return true;
        }

        /// <summary>
        /// Prints the results in a readable way to the console
        /// </summary>
        /// <param name="optimal">A tuple containing the optimal length and the ordered list of site indexes</param>
        /// <param name="stopwatch">A stopwatch that measured the time</param>
        /// <param name="sites">A list containing all sites</param>
        public static void PrintResults((double length, IList<int> tour) optimal, Stopwatch stopwatch, List<Site> sites)
        {
            var routeLength = Math.Round(optimal.length, 2);
            var stops = optimal.tour.Select(stop => $"{sites.First(site => site.Index == stop).Name} ({stop})");
            var elapsedSeconds = Math.Round(stopwatch.Elapsed.TotalSeconds, 2);
            var divider = "==========================================";

            Console.WriteLine(divider);
            Console.WriteLine($"Shortest route length: {routeLength} km");
            Console.WriteLine(divider);
            Console.WriteLine(string.Join("\n-> ", stops));
            Console.WriteLine(divider);
            Console.WriteLine($"Calculation took: {elapsedSeconds} seconds");
            Console.WriteLine(divider);
        }
    }
}
