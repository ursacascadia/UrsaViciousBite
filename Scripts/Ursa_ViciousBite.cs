using System;

namespace XRL.World.Parts.Mutation {

[Serializable]
public class Ursa_ViciousBite : BaseDefaultEquipmentMutation
{
    public GameObject BiteObject;

    public string BodyPartType = "Face";

    public override IPart DeepCopy(GameObject Parent, Func<GameObject, GameObject> MapInv)
    {
        Ursa_ViciousBite obj = base.DeepCopy(Parent, MapInv) as Ursa_ViciousBite;
        obj.BiteObject = null;
        return obj;
    }

    public override bool CanLevel()
    {
        return true;
    }

    public override bool GeneratesEquipment()
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
        int bonus_int = 0 + Level / 4;
        string bonus = bonus_int >= 1 ? bonus_int.ToString() : "";
        return "1d" + (3 + Level / 2) + (bonus != "" ? "+" : "") + bonus;
    }

    public override string GetDescription()
    {
        return "Your jaws are capable of bone crushing force.";
    }

    public override string GetLevelText(int Level)
    {
        string WeaponType = "a short-blade";
        if (!Variant.IsNullOrEmpty() && Variant == "Ursa_ViciousBite_Carnassial")
        {
            WeaponType = "an axe";
        }
        string text = "Grants a {{rules|vicious bite}}, " + WeaponType + " class natural weapon.\n";
        text = text + "Deals {{rules|" + GetBaseDamage(Level) + "}} damage.\n";
        return text + "+100 reputation with {{w|snapjaws}}";
    }

    public override void OnRegenerateDefaultEquipment(Body body)
    {
        if (!TryGetRegisteredSlot(body, BodyPartType, out var Part))
        {
            Part = body.GetFirstPart(BodyPartType);
            if (Part != null)
            {
                RegisterSlot(BodyPartType, Part);
            }
        }
        if (Part != null)
        {
            BiteObject = GameObjectFactory.Factory.CreateObject(Variant ?? "Ursa_ViciousBite");
            MeleeWeapon part = BiteObject.GetPart<MeleeWeapon>();
            part.BaseDamage = GetBaseDamage(base.Level);
            part.MaxStrengthBonus = 100;
            part.Slot = Part.Type;
            Part.DefaultBehavior = BiteObject;
            Part.DefaultBehavior.SetStringProperty("TemporaryDefaultBehavior", "Bite");
            BiteObject.SetStringProperty("HitSound", "Sounds/Creatures/VO/sfx_creature_animal_snapjaw_vo_attack");
            ResetDisplayName();
        }
        base.OnRegenerateDefaultEquipment(body);
    }

    public override bool ChangeLevel(int NewLevel)
    {
        return base.ChangeLevel(NewLevel);
    }

    public override bool Mutate(GameObject GO, int Level)
    {
        return base.Mutate(GO, Level);
    }

    public override bool Unmutate(GameObject GO)
    {
        CleanUpMutationEquipment(GO, ref BiteObject);
        return base.Unmutate(GO);
    }
}

}