using System;
using XRL.World.Parts;

namespace XRL.World.Parts.Mutation {

[Serializable]
public class Ursa_Devour : BaseMutation
{
    public Engulfing EngulfingPart;

    public override bool CanLevel()
    {
        return true;
    }

    public override void Register(GameObject Object, IEventRegistrar Registrar)
    {
        base.Register(Object, Registrar);
    }

    public override bool FireEvent(Event E)
    {
        return base.FireEvent(E);
    }
    
    public string GetBaseDamage(int Level)
    {
        return "2d" + (1 + Level / 2);
    }

    public int GetTurnCount(int Level)
    {
        return (7 + Level / 2);
    }

    public override string GetDescription()
    {
        return "You devour and digest your enemies whole.";
    }

    public override string GetLevelText(int Level)
    {
        string text = "Devour a nearby enemy for {{rules|" + GetTurnCount(Level) + "}} rounds.\n";
        text = text + "Deals {{rules|" + GetBaseDamage(Level) + "}} damage per turn.\n";
        return text;
    }

    public override bool ChangeLevel(int NewLevel)
    {
        EngulfingPart.Damage = GetBaseDamage(Level);
        return base.ChangeLevel(NewLevel);
    }

    public override bool Mutate(GameObject GO, int Level)
    {
        EngulfingPart = ParentObject.RequirePart<Engulfing>();
        EngulfingPart.Damage = GetBaseDamage(Level);
        EngulfingPart.DamageChance = 100;
        return base.Mutate(GO, Level);
    }

    public override bool Unmutate(GameObject GO)
    {
        ParentObject.RemovePart<Engulfing>();
        ParentObject.RequirePart<EngulfingSedentary>();
        return base.Unmutate(GO);
    }
}

}