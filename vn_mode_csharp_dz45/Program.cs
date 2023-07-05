using System;

public static class Program
{
    private static string fighter1Name = "Warrior1";
    private static int fighter1Health = 100;
    private static int fighter1Damage = 20;

    private static string fighter2Name = "Warrior2";
    private static int fighter2Health = 100;
    private static int fighter2Damage = 20;

    private static string fighter3Name = "Rogue";
    private static int fighter3Health = 90;
    private static int fighter3Damage = 15;

    private static string fighter4Name = "Priest";
    private static int fighter4Health = 80;
    private static int fighter4Damage = 10;

    private static string fighter5Name = "Mage";
    private static int fighter5Health = 70;
    private static int fighter5Damage = 15;
    private static int fighter5Mana = 100;

    public static void Main()
    {
        Warrior warrior1 = new Warrior(fighter1Name, fighter1Health, fighter1Damage);
        Rogue rogue = new Rogue(fighter3Name, fighter3Health, fighter3Damage);

        while (true)
        {
            warrior1.Attack(rogue);
            if (rogue.IsDead())
            {
                Console.WriteLine(Messages.WinMessage, warrior1.Name, rogue.Name);
                break;
            }

            rogue.Attack(warrior1);
            if (warrior1.IsDead())
            {
                Console.WriteLine(Messages.WinMessage, rogue.Name, warrior1.Name);
                break;
            }
        }
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

    public void Attack(Fighter enemy)
    {
        SpecialAttack(enemy);
        if (enemy.Health <= 0)
        {
            enemy.Health = 0;
            Console.WriteLine(Messages.DeathMessage, enemy.Name);
        }
    }
}

public class Warrior : Fighter
{
    private int attackCounter = 0;

    public Warrior(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        attackCounter++;
        int attackDamage = attackCounter % 3 == 0 ? Damage * 2 : Damage;
        enemy.Health -= attackDamage;
        Console.WriteLine(Messages.DamageMessage, Name, attackDamage, enemy.Name, enemy.Health);
    }
}

public class Rogue : Fighter
{
    private static Random rnd = new Random();
    public Rogue(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        if (rnd.Next(100) < 30)
        {
            Console.WriteLine(Messages.DodgeMessage, Name);
        }
        else
        {
            enemy.Health -= Damage;
            Console.WriteLine(Messages.DamageMessage, Name, Damage, enemy.Name, enemy.Health);
        }
    }
}

public class Priest : Fighter
{
    public Priest(string name, int health, int damage) : base(name, health, damage) { }

    public override void SpecialAttack(Fighter enemy)
    {
        enemy.Health -= Damage;
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
            enemy.Health -= Damage * 2;
            Mana -= 10;
            Console.WriteLine(Messages.MagicDamageMessage, Name, Damage * 2, Mana, enemy.Name, enemy.Health);
        }
        else
        {
            enemy.Health -= Damage;
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

