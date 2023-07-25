using System;
using System.Collections.Generic;

public static class Program
{
    public static void Main()
    {
        BattleController controller = new BattleController();
        controller.StartBattle();
    }
}

public class BattleController
{
    private List<Fighter> _fighters;

    public BattleController()
    {
        _fighters = new List<Fighter>
        {
            new Warrior("Warrior1", 100, 20),
            new Warrior("Warrior2", 100, 20),
            new Rogue("Rogue", 90, 15),
            new Priest("Priest", 80, 10),
            new Mage("Mage", 70, 15, 100)
        };
    }

    public void StartBattle()
    {
        Console.WriteLine("Выберите двух бойцов для битвы:");

        for (int i = 0; i < _fighters.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_fighters[i].Name}");
        }

        int fighter1Index = GetUserChoice(_fighters.Count) - 1;
        int fighter2Index = GetUserChoice(_fighters.Count) - 1;

        Fighter fighter1 = _fighters[fighter1Index];
        Fighter fighter2 = _fighters[fighter2Index];

        while (!fighter1.IsDead() && !fighter2.IsDead())
        {
            fighter1.Attack(fighter2);

            if (fighter2.IsDead()) continue;
            fighter2.Attack(fighter1);
        }

        Console.WriteLine(fighter1.IsDead()
            ? string.Format(Messages.WinMessage, fighter2.Name, fighter1.Name)
            : string.Format(Messages.WinMessage, fighter1.Name, fighter2.Name));
    }

    private static int GetUserChoice(int maxChoice)
    {
        bool validInput = false;

        while (!validInput)
        {
            int choice = 0;
            Console.Write("Введите число: ");
            validInput = int.TryParse(Console.ReadLine(), out choice) && choice >= 1 && choice <= maxChoice;

            if (!validInput)
            {
                Console.WriteLine($"Неверный ввод. Введите число от 1 до {maxChoice}.");
            }
            else
            {
                return choice;
            }
        }
        return 0; // This line will never be reached but is necessary for compilation
    }
}

public abstract class Fighter
{
    public string Name { get; private set; }
    public int Health { get; protected set; }
    public int Damage { get; private set; }

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
    private const int AttackMultiplier = 2;
    private const int SpecialAttackFrequency = 3;
    private int _attackCounter = 0;

    public Warrior(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        _attackCounter++;
        int attackDamage = _attackCounter % SpecialAttackFrequency == 0 ? Damage * AttackMultiplier : Damage;
        enemy.TakeDamage(attackDamage);
        Console.WriteLine(Messages.DamageMessage, Name, attackDamage, enemy.Name, enemy.Health);
    }
}

public class Rogue : Fighter
{
    private static Random _rnd = new Random();
    private const int DodgeChance = 30;

    public Rogue(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        if (_rnd.Next(100) < DodgeChance)
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
    private const int HealMultiplier = 2;

    public Priest(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        enemy.TakeDamage(Damage);
        Health += Damage / HealMultiplier;
        Console.WriteLine(Messages.HealAndDamageMessage, Name, Damage, Damage / HealMultiplier, enemy.Name, enemy.Health);
    }
}

public class Mage : Fighter
{
    public int Mana { get; private set; }
    private const int MagicDamageMultiplier = 2;
    private const int ManaCost = 10;

    public Mage(string name, int health, int damage, int mana) : base(name, health, damage)
    {
        Mana = mana;
    }

    public override void SpecialAttack(Fighter enemy)
    {
        if (Mana >= ManaCost)
        {
            enemy.TakeDamage(Damage * MagicDamageMultiplier);
            Mana -= ManaCost;
            Console.WriteLine(Messages.MagicDamageMessage, Name, Damage * MagicDamageMultiplier, Mana, enemy.Name, enemy.Health);
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
