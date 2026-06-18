using System;
using XRL.World;
using XRL.Core;
using XRL.Rules;
using XRL.Messages;
using XRL.UI;

namespace XRL.World.Parts {

// Differs from other engulfing damage parts. Only damages when the player remains still. Ends after specified turns passed

[Serializable]
public class Ursa_PlayerEngulfing : IPart
{
    public string DamageMessage = "from %t digestive enzymes!";
    public string Damage = "2d1";
    public string DamageAttributes = "Acid";
    public int Length = 8;

    public Engulfing EngulfingPart;

    int activeRounds = 0;
    
    public override bool WantEvent(int ID, int cascade)
    {
        return base.WantEvent(ID, cascade)
            || ID == UseEnergyEvent.ID && (EngulfingPart != null ? EngulfingPart.CheckEngulfed() : false)
            || ID == SingletonEvent<EndTurnEvent>.ID && (EngulfingPart != null ? EngulfingPart.CheckEngulfed() : false);
    }

    public override bool HandleEvent(UseEnergyEvent E)
    {
        if (E.Passive || (E.Type != null && E.Type.Contains("Pass")))
        {
            if (EngulfingPart == null) {
                Popup.ShowFail("Engulfing part is null");
            }
            if (EngulfingPart.Engulfed != null)
            {
                Damage value = new Damage(Stat.Roll(Damage));
                Event obj = Event.New("TakeDamage");
                obj.AddParameter("Damage", value);
                obj.AddParameter("Owner", ParentObject);
                obj.AddParameter("Attacker", ParentObject);
                obj.AddParameter("Message", DamageMessage);
                obj.AddParameter("Attributes", DamageAttributes);
                EngulfingPart.Engulfed.FireEvent(obj);
            }
        }
        return base.HandleEvent(E);
    }
    
    public override bool HandleEvent(EndTurnEvent E)
    {
        activeRounds += 1;
        if (activeRounds > Length)
        {
            EngulfingPart.EndAllEngulfment();
            activeRounds = 0;
        }
        return base.HandleEvent(E);
    }

}

}