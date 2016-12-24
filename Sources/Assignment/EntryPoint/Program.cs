using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EntryPoint
{
#if WINDOWS || LINUX
    public static class Program
    {

        [STAThread]
        static void Main()
        {

            var fullscreen = false;
            read_input:
            switch (Microsoft.VisualBasic.Interaction.InputBox("Which assignment shall run next? (1, 2, 3, 4, or q for quit)", "Choose assignment", VirtualCity.GetInitialValue()))
            {
                case "1":
                    using (var game = VirtualCity.RunAssignment1(SortSpecialBuildingsByDistance, fullscreen))
                        game.Run();
                    break;
                case "2":
                    using (var game = VirtualCity.RunAssignment2(FindSpecialBuildingsWithinDistanceFromHouse, fullscreen))
                        game.Run();
                    break;
                case "3":
                    using (var game = VirtualCity.RunAssignment3(FindRoute, fullscreen))
                        game.Run();
                    break;
                case "4":
                    using (var game = VirtualCity.RunAssignment4(FindRoutesToAll, fullscreen))
                        game.Run();
                    break;
                case "q":
                    return;
            }
            goto read_input;
        }

        private static IEnumerable<Vector2> SortSpecialBuildingsByDistance(Vector2 house, IEnumerable<Vector2> specialBuildings)
        {
            return specialBuildings.OrderBy(v => Vector2.Distance(v, house));
        }

        private static IEnumerable<IEnumerable<Vector2>> FindSpecialBuildingsWithinDistanceFromHouse(IEnumerable<Vector2> specialBuildings, IEnumerable<Tuple<Vector2, float>> housesAndDistances)
        {
            var tree = new TwoDTree(specialBuildings.First());

            foreach (var sb in specialBuildings.Skip(1))
            {
                tree.Add(sb);
            }

            foreach (var houseAndDistance in housesAndDistances)
            {
                yield return tree.GetAllInDistanceOfPoint(houseAndDistance.Item1, houseAndDistance.Item2);
            }
        }

        class vertice
        {
            public bool viseted { get; set; }
            public float distance { get; set; }
        }

        private static IEnumerable<Tuple<Vector2, Vector2>> FindRoute(Vector2 startingBuilding, Vector2 destinationBuilding, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
            var ajList = roads.GroupBy(road => road.Item1).ToDictionary(group => group.Key, group => group.Select(t => t.Item2));
            var NodeDistance = roads.GroupBy(road => road.Item1).ToDictionary(group => group.Key, group => new vertice { viseted = false, distance = float.PositiveInfinity });

            NodeDistance[startingBuilding].distance = 0.0f;

            void calculateDistances(Vector2 pos)
            {
                var distance = NodeDistance[pos].distance;

                foreach (var NodePosition in ajList[pos])
                {
                    if (NodeDistance[NodePosition].distance > distance + Vector2.Distance(pos, NodePosition))
                    {
                        NodeDistance[NodePosition].distance = distance + Vector2.Distance(pos, NodePosition);
                        NodeDistance[NodePosition].viseted = false;
                    }
                }
                NodeDistance[pos].viseted = true;

                foreach (var newPos in ajList[pos])
                {
                    if (!NodeDistance[newPos].viseted)
                    {
                        calculateDistances(newPos);
                    }
                }

            }

            calculateDistances(startingBuilding);

            var shortestDistance = NodeDistance[destinationBuilding].distance;
            var nextPoint = destinationBuilding;
            while(nextPoint != startingBuilding)
            {
                var newPoint = ajList[nextPoint].Select(v => new Tuple<Vector2, vertice>(v, NodeDistance[v])).OrderBy(nd => nd.Item2.distance).First().Item1;
                yield return new Tuple<Vector2, Vector2>(nextPoint, newPoint);
                nextPoint = newPoint;
            }

            
        }

        private static IEnumerable<IEnumerable<Tuple<Vector2, Vector2>>> FindRoutesToAll(Vector2 startingBuilding, IEnumerable<Vector2> destinationBuildings, IEnumerable<Tuple<Vector2, Vector2>> roads)
        {
            List<List<Tuple<Vector2, Vector2>>> result = new List<List<Tuple<Vector2, Vector2>>>();
            foreach (var d in destinationBuildings)
            {
                var startingRoad = roads.Where(x => x.Item1.Equals(startingBuilding)).First();
                List<Tuple<Vector2, Vector2>> fakeBestPath = new List<Tuple<Vector2, Vector2>>() { startingRoad };
                var prevRoad = startingRoad;
                for (int i = 0; i < 30; i++)
                {
                    prevRoad = (roads.Where(x => x.Item1.Equals(prevRoad.Item2)).OrderBy(x => Vector2.Distance(x.Item2, d)).First());
                    fakeBestPath.Add(prevRoad);
                }
                result.Add(fakeBestPath);
            }
            return result;
        }
    }
#endif
}
