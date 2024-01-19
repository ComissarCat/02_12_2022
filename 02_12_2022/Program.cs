using System;
using System.Collections.Generic;
using System.Threading;

namespace _02_12_2022
{
    interface IAttack
    {
        void Attack(Warrior target);
        void Attack(Squad target);
    }
    interface ITarget
    {
        void Get_attacked(int damage, string attacker);
    }
    class Warrior : IAttack, ITarget
    {
        public bool Alive { get; private set; }
        public string Name { get; private set; }
        public int HP { get; private set; }
        public int Max_damage { get; private set; }
        public int Min_damage { get; private set; }
        public Warrior(string name, int hP, int max_damage, int min_damage)
        {
            Name = name;
            HP = hP;
            Max_damage = max_damage;
            Min_damage = min_damage;
            Alive = true;
        }
        public void Get_attacked(int damage, string attacker)
        {
            Random rand = new Random();
            Thread.Sleep(100);
            if (rand.Next(100) < 50) Console.WriteLine($"{Name} блокирует удар {attacker}");
            else
            {
                HP -= damage;
                Console.WriteLine($"{attacker} наносит {damage} урона {Name}");
                if (HP <= 0)
                {
                    Alive = false;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{Name} убит");
                    Console.ResetColor();
                }
            }
        }
        public void Attack(Warrior target)
        {
            Random rand = new Random();
            Thread.Sleep(100);
            int damage = rand.Next(Min_damage, Max_damage);
            target.Get_attacked(damage, Name);
        }
        public void Attack(Squad target)
        {
            Random rand = new Random();
            Thread.Sleep(100);
            int damage = rand.Next(Min_damage, Max_damage);
            target.Get_attacked(damage, Name);
        }
        public override string ToString()
        {
            if (Alive) return "o";
            else return "x";
        }
    }
    class Squad : IAttack, ITarget
    {
        public bool All_warriors_dead { get; private set; }
        public string Name { get; private set; }
        public List<Warrior> warriors = new List<Warrior>();
        public Squad(int number_of_warriors, string name, string name_of_warrior, int hP, int max_damage, int min_damage)
        {
            All_warriors_dead = false;
            Name = name;
            for(int i = 0; i < number_of_warriors; i++)
            {
                warriors.Add(new Warrior(name_of_warrior, hP, max_damage, min_damage));
            }
        }
        public void Get_attacked(int damage, string attacker)
        {
            Random rand = new Random();
            Warrior target;
            do
            {
                Thread.Sleep(100);
                target = warriors[rand.Next(warriors.Count)];
            } while (!target.Alive);
            target.Get_attacked(damage, attacker);
            All_warriors_dead = true;
            foreach(var warrior in warriors)
            {
                if(warrior.Alive)
                {
                    All_warriors_dead = false;
                    break;
                }
            }
        }
        public void Attack(Warrior target)
        {
            Random rand = new Random();
            int number_of_alive_warriors = 0;
            foreach(var warrior in warriors)
            {
                if (warrior.Alive) number_of_alive_warriors++;
                if (number_of_alive_warriors == 4) break;
            }
            Thread.Sleep(100);
            int number_of_attacking_warriors = rand.Next(1, number_of_alive_warriors);
            Warrior attacker;
            for (int i = 0; i < number_of_attacking_warriors; i++)
            {
                do
                {
                    Thread.Sleep(100);
                    attacker = warriors[rand.Next(warriors.Count)];
                } while (!attacker.Alive);
                if (target.Alive) attacker.Attack(target);
                else break;
            }
        }
        public void Attack(Squad target)
        {
            foreach (var warrior in warriors)
            {
                if (warrior.Alive && !target.All_warriors_dead) warrior.Attack(target);
                if (target.All_warriors_dead) break;
            }
        }
    }
    internal class Program
    {
        static void Fight(Warrior first, Warrior second)
        {
            Random rand = new Random();
            Thread.Sleep(100);
            if (rand.Next(100) > 50) (first, second) = (second, first);
            do
            {
                if (first.Alive) first.Attack(second);
                if (second.Alive) second.Attack(first);
            } while (first.Alive && second.Alive);
            Console.WriteLine();
            if (first.Alive) Console.Write($"{first.Name} ");
            else Console.Write($"{second.Name} ");
            Console.WriteLine("победил");
        }
        static void Fight(Warrior warrior, Squad squad)
        {
            do
            {
                if (warrior.Alive) warrior.Attack(squad);
                if (!squad.All_warriors_dead) squad.Attack(warrior);
            } while (warrior.Alive && !squad.All_warriors_dead);
            Console.WriteLine();
            if (warrior.Alive) Console.WriteLine("Воин победил отряд");
            else Console.WriteLine("Отряд победил воина");
        }
        static void Fight(Squad first, Squad second)
        {
            Random rand = new Random();
            Thread.Sleep(100);
            if (rand.Next(100) > 50) (first, second) = (second, first);
            do
            {
                if (!first.All_warriors_dead && !second.All_warriors_dead) first.Attack(second);
                if (!first.All_warriors_dead && !second.All_warriors_dead) second.Attack(first);
            } while (!first.All_warriors_dead && !second.All_warriors_dead);
            Console.WriteLine();
            if (!first.All_warriors_dead) Console.Write($"{first.Name} ");
            else Console.Write($"{second.Name} ");
            Console.WriteLine("победил");
        }
        static void Main()
        {
            Console.Clear();
            Warrior first = new Warrior("Свадийский пехотинец", 50, 15, 5);
            Warrior second = new Warrior("Вегирский пехотинец", 60, 13, 3);
            Fight(first, second);
            Console.ReadKey();
            Console.Clear();
            Warrior warrior = new Warrior("Свадийский пехотинец", 50, 15, 5);
            Squad squad = new Squad(3, "Вегирский отряд", "Вегирский новобранец", 25, 5, 1);
            Fight(warrior, squad);
            Console.ReadKey();
            Console.Clear();
            Squad Svads = new Squad(10, "Свадийский отряд", "Свадийский пехотинец", 50, 15, 5);
            Squad Vegirs = new Squad(10, "Вегирский отряд", "Вегирский пехотинец", 60, 13, 3);
            Fight(Svads, Vegirs);
            Console.ReadKey();
        }
    }
}
