//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.04.2025 21:12:07</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

using System;

namespace Console.ObjectTracking
{
    public class Program
    {
        private static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.WriteLine("1. Menüpunkt 1");
                Console.WriteLine("2. Menüpunkt 2");
                Console.WriteLine("X. Beenden");

                Console.WriteLine("Wählen Sie einen Menüpunkt oder 'x' für beenden");
                ConsoleKey key = Console.ReadKey(true).Key;
                if (key == ConsoleKey.X)
                {
                    Environment.Exit(0);
                }
                else
                {
                    if (key == ConsoleKey.D1)
                    {
                        MenuPoint1();
                    }
                    else if (key == ConsoleKey.D2)
                    {
                        MenuPoint2();
                    }
                }
            }
            while (true);
        }

        private static void MenuPoint1()
        {
            Console.Clear();
            Console.WriteLine("Menüpunkt 1, eine Taste drücken für zurück!");
            Console.ReadKey();
        }

        private static void MenuPoint2()
        {
            Console.Clear();
            Console.WriteLine("Menüpunkt 2, eine Taste drücken für zurück!");
            Console.ReadKey();
        }
    }
}
