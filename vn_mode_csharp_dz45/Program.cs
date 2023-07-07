using System;
using System.Collections.Generic;

public static class Program
{
    private static string _fighter1Name = "Warrior1";
    private static int _fighter1Health = 100;
    private static int _fighter1Damage = 20;

    private static string _fighter2Name = "Warrior2";
    private static int _fighter2Health = 100;
    private static int _fighter2Damage = 20;

    private static string _fighter3Name = "Rogue";
    private static int _fighter3Health = 90;
    private static int _fighter3Damage = 15;

    private static string _fighter4Name = "Priest";
    private static int _fighter4Health = 80;
    private static int _fighter4Damage = 10;

    private static string _fighter5Name = "Mage";
    private static int _fighter5Health = 70;
    private static int _fighter5Damage = 15;
    private static int _fighter5Mana = 100;

    public static void Main()
    {
        List<Fighter> fighters = new List<Fighter> {
            new Warrior(_fighter1Name, _fighter1Health, _fighter1Damage),
            new Warrior(_fighter2Name, _fighter2Health, _fighter2Damage),
            new Rogue(_fighter3Name, _fighter3Health, _fighter3Damage),
            new Priest(_fighter4Name, _fighter4Health, _fighter4Damage),
            new Mage(_fighter5Name, _fighter5Health, _fighter5Damage, _fighter5Mana)
        };

        Console.WriteLine("Выберите двух бойцов для битвы:");

        for (int i = 0; i < fighters.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {fighters[i].Name}");
        }

        int fighter1Index = GetUserChoice(fighters.Count) - 1;
        int fighter2Index = GetUserChoice(fighters.Count) - 1;

        Fighter fighter1 = fighters[fighter1Index];
        Fighter fighter2 = fighters[fighter2Index];

        while (!fighter1.IsDead() && !fighter2.IsDead())
        {
            fighter1.Attack(fighter2);
            if (fighter2.IsDead()) break;
            fighter2.Attack(fighter1);
        }

        if (fighter1.IsDead())
        {
            Console.WriteLine(Messages.WinMessage, fighter2.Name, fighter1.Name);
        }
        else
        {
            Console.WriteLine(Messages.WinMessage, fighter1.Name, fighter2.Name);
        }
    }

    private static int GetUserChoice(int maxChoice)
    {
        int choice;

        while (true)
        {
            Console.Write("Введите число: ");
            if (int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= maxChoice)
            {
                break;
            }
            else
            {
                Console.WriteLine($"Неверный ввод. Введите число от 1 до {maxChoice}.");
            }
        }
        return choice;
    }
}

public abstract class Fighter
{
    public string Name { get; set; }
    public int Health { get; set; }
    public int Damage { get; set; }

    protected Fighter(string name, int health, int damage)
    {
        Name = name;
        Health = health;
        Damage = damage;
    }

    public abstract void SpecialAttack(Fighter enemy);

    public bool IsDead() => Health <= 0;

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health < 0) Health = 0;
    }

    public void Attack(Fighter enemy)
    {
        SpecialAttack(enemy);
        if (enemy.IsDead())
        {
            Console.WriteLine(Messages.DeathMessage, enemy.Name);
        }
    }
}

public class Warrior : Fighter
{
    private int _attackCounter = 0;

    public Warrior(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        _attackCounter++;
        int attackDamage = _attackCounter % 3 == 0 ? Damage * 2 : Damage;
        enemy.TakeDamage(attackDamage);
        Console.WriteLine(Messages.DamageMessage, Name, attackDamage, enemy.Name, enemy.Health);
    }
}

public class Rogue : Fighter
{
    private static Random _rnd = new Random();

    public Rogue(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        if (_rnd.Next(100) < 30)
        {
            Console.WriteLine(Messages.DodgeMessage, Name);
        }
        else
        {
            enemy.TakeDamage(Damage);
            Console.WriteLine(Messages.DamageMessage, Name, Damage, enemy.Name, enemy.Health);
        }
    }
}

public class Priest : Fighter
{
    public Priest(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        enemy.TakeDamage(Damage);
        Health += Damage / 2;
        Console.WriteLine(Messages.HealAndDamageMessage, Name, Damage, Damage / 2, enemy.Name, enemy.Health);
    }
}

public class Mage : Fighter
{
    public int Mana { get; set; }

    public Mage(string name, int health, int damage, int mana) : base(name, health, damage)
    {
        Mana = mana;
    }

    public override void SpecialAttack(Fighter enemy)
    {
        if (Mana >= 10)
        {
            enemy.TakeDamage(Damage * 2);
            Mana -= 10;
            Console.WriteLine(Messages.MagicDamageMessage, Name, Damage * 2, Mana, enemy.Name, enemy.Health);
        }
        else
        {
            enemy.TakeDamage(Damage);
            Console.WriteLine(Messages.DamageMessage, Name, Damage, enemy.Name, enemy.Health);
        }
    }
}

public static class Messages
{
    public const string DeathMessage = "{0} умер.";
    public const string DamageMessage = "{0} нанес {1} урона. Здоровье {2}: {3}";
    public const string DodgeMessage = "{0} увернулся от удара!";
    public const string HealAndDamageMessage = "{0} нанес {1} урона и восстановил {2} здоровья. Здоровье {3}: {4}";
    public const string MagicDamageMessage = "{0} использует магию. Нанесен урон: {1}. Остаток маны: {2}. Здоровье {3}: {4}";
    public const string WinMessage = "{0} победил {1}!";
}
