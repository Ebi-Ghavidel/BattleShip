using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip
{
    class Program
    {
        public struct Location
        {
            public int x, y;
            public Location(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
            public static int operator -(Location L1, Location L2) => Math.Abs(L1.x-L2.x)+Math.Abs(L1.y-L2.y);
            public static bool operator ==(Location L1, Location L2) => (L1.x == L2.x && L1.y == L2.y) ? true : false;
            public static bool operator !=(Location L1, Location L2) => (L1.x != L2.x || L1.y != L2.y) ? true : false;
        }
        public struct Board
        {
            public int Rows, Columns;
            public Board (int Rows, int Columns)
            {
                this.Rows = Rows;
                this.Columns = Columns;
            }
        }
        public class Ship
        {
            public Location Location;
            public bool Exploded;
            public Ship() { }
            public Ship(Location Location) => this.Location = Location;
        }

        public class Play
        {
            private List<Ship> ships = new List<Ship>();
            public Board Board { get; private set; }
            public int NumberOfGuesses { get; private set; }

            public Play(Board Board, int NumberOfShips, int NumberOfGuesses)
            {
                this.Board = Board;
                this.NumberOfGuesses = NumberOfGuesses;
                PlaceShips(NumberOfShips);
            }

            private void PlaceShips(int numberOfShips)
            {
                for (int i = 0; i < numberOfShips; i++)
                {
                    Ship newShip;
                    do
                    {
                        var random = new Random();
                        newShip = new Ship(new Location(random.Next(0, Board.Rows), random.Next(0, Board.Columns)));
                    } while (!ships.All(predicate: s => s.Location != newShip.Location));
                    ships.Add(newShip);
                }
            }

            public void Start()
            {
                while (NumberOfGuesses > 0 && !ships.All(predicate: s => s.Exploded))
                {
                    Location hitPlace;
                    Console.WriteLine("You have " + NumberOfGuesses + " Guess(es). Please enter the number of row (x):");
                    hitPlace.x = GetInput();
                    Console.WriteLine("Please enter the number of column (y):");
                    hitPlace.y = GetInput();
                    ShowMessage(Fire.GetDistance(hitPlace, ref ships));
                    NumberOfGuesses--;
                }
                GameOver();   
            }

            private int GetInput()
            {
                try
                {
                    return Convert.ToInt32(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Incorrect input! Please enter the correct value.");
                    GetInput();
                }
                return 0;
            }
            public static class Fire
            {
                public static int GetDistance(Location hitPlace, ref List<Ship> ships)
                {
                    int distance = int.MaxValue;
                    for (int i = 0; i < ships.Count; i++)
                    {
                        if (!ships[i].Exploded && hitPlace - ships[i].Location < distance)
                        {
                            distance = hitPlace - ships[i].Location;
                            if (distance == 0)
                            {
                                ships[i].Exploded = true;
                                return distance;
                            }
                        }
                    }
                    return distance;
                }
            }
            private void ShowMessage(int minDistance)
            {
                switch (minDistance)
                {
                    case 0:
                        Console.WriteLine("hit");
                        break;
                    case 1:
                    case 2:
                        Console.WriteLine("hot");
                        break;
                    case 3:
                    case 4:
                        Console.WriteLine("warm");
                        break;
                    default:
                        Console.WriteLine("cold");
                        break;
                }
            }
            private void GameOver()
            {
                Console.WriteLine("Game Finished.. " +
                    (ships.All(predicate: s => s.Exploded) ? "You Won!" : "You Lose!!"));
                Console.ReadLine();
            }
        }
        static void Main(string[] args)
        {
            var myBoard = new Board(8, 8);
            var newPlay = new Play(myBoard, 2, 20);
            newPlay.Start();
        }
    }
}
