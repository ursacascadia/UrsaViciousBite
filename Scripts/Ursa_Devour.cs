using System;
using XRL.World.Parts;

namespace XRL.World.Parts.Mutation {

[Serializable]
public class Ursa_Devour : BaseMutation
{
    public Engulfing EngulfingPart;
    public Ursa_PlayerEngulfing EngulfingPlayerPart;

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
        text = text + "Deals {{rules|" + GetBaseDamage(Level) + "}} acid damage per turn while stationary.\n";
        return text;
    }

    public override bool ChangeLevel(int NewLevel)
    {
        EngulfingPlayerPart.Damage = GetBaseDamage(Level);
        EngulfingPlayerPart.Length = GetTurnCount(Level);
        return base.ChangeLevel(NewLevel);
    }

    public override bool Mutate(GameObject GO, int Level)
    {
        EngulfingPart = ParentObject.RequirePart<Engulfing>();
        EngulfingPlayerPart = ParentObject.RequirePart<Ursa_PlayerEngulfing>();
        EngulfingPlayerPart.DamageAttributes = "Acid";
        EngulfingPlayerPart.Damage = GetBaseDamage(Level);
        EngulfingPlayerPart.Length = GetTurnCount(Level);
        EngulfingPlayerPart.EngulfingPart = EngulfingPart;
        return base.Mutate(GO, Level);
    }

    public override bool Unmutate(GameObject GO)
    {
        ParentObject.RemovePart<Engulfing>();
        ParentObject.RemovePart<Ursa_PlayerEngulfing>();
        return base.Unmutate(GO);
    }
}

}