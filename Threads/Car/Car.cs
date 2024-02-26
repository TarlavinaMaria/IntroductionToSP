using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Car
{
    internal class Car
    {
        static readonly int MAX_SPEED_LOW_LIMIT = 100;
        static readonly int MAX_SPEED_HIGH_LIMIT = 400;
        public int MAX_SPEED { get; }
        Engine engine;//двигатель
        Tank tank;//бак
        bool driver_inside;
        struct Threads
        {
            public Thread PanelThread { get; set; }
            public Thread EngineIdleThread { get; set; }
        }
        Threads threads;
        public Car(double consumption, int volume, int max_speed = 250)
        {
            engine = new Engine(consumption);
            tank = new Tank(volume);
            driver_inside = false;
            threads = new Threads();
            if (max_speed < MAX_SPEED_LOW_LIMIT) max_speed = MAX_SPEED_LOW_LIMIT;
            if (max_speed > MAX_SPEED_HIGH_LIMIT) max_speed = MAX_SPEED_HIGH_LIMIT;
            this.MAX_SPEED = max_speed;
        }
        public void GetIn()
        {
            driver_inside = true;
            threads.PanelThread = new Thread(Panel);
            threads.PanelThread.Start();
        }
        public void GetOut()
        {
            driver_inside = false;
            threads.PanelThread.Join();
            Console.Clear();
            Console.WriteLine("You are out of the car");
        }
        public void Start()
        {
            if (driver_inside && tank.FuelLevel != 0)
            {
                engine.Start();
                threads.EngineIdleThread = new Thread(EngineIdle);
                threads.EngineIdleThread.Start();
            }
        }
        public void Stop()
        {
            engine.Stop();
            threads.EngineIdleThread.Join();
        }
        void EngineIdle()//расходует бензин на холостом ходу
        {
            while (engine.Started)
            {
                tank.GiveFuel(engine.ConsumptionPerSecond);
                if (tank.FuelLevel == 0) Stop();
                Thread.Sleep(800);
            }
        }
        public void Control()
        {
            Console.WriteLine("You car is ready, press Enter to get in");
            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                switch (key)
                {
                    case ConsoleKey.Enter:
                        if (driver_inside) GetOut();
                        else GetIn();
                        break;
                    case ConsoleKey.Escape:
                        GetOut();
                        break;
                    case ConsoleKey.F:
                        if (!driver_inside)
                        {
                            Console.Write("Введите обьем топлива:");
                            double amount = Convert.ToInt32(Console.ReadLine());
                            tank.Fill(amount);
                        }
                        else
                        {
                            Console.WriteLine("Get out of the car:");
                        }
                        break;
                    case ConsoleKey.I:
                        if (engine.Started) engine.Stop();
                        else Start();
                        break;
                }
            } while (key != ConsoleKey.Escape);
        }
        void Panel()
        {
            while (driver_inside)
            {
                Console.Clear();
                Console.WriteLine($"Fuel level: {tank.FuelLevel} liters");
                if (tank.FuelLevel < 5) Console.WriteLine("LOW FUEL");
                Console.WriteLine($"Engine is {(engine.Started ? "started" : "stopped")}");
                Thread.Sleep(500);
            }
        }
        public void Info()
        {
            engine.Info();
            tank.Info();
            Console.WriteLine($"Max speed {MAX_SPEED} km/h");
        }
    }
}
