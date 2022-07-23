using AEAssist.Define;
using AEAssist.Utilities.CombatMessages;
using ff14bot;

namespace AEAssist.AI.Reaper
{
    public class ReaperCombatMessageStrategy
    {
        public static void RegisterCombatMessages()
        {

            //Highest priority: Don't show anything if we're not in combat
            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(100,
                                          "",
                                          () => !Core.Me.InCombat || !Core.Me.HasTarget));

            // Second priority: Don't show anything if positional requirements are Nulled
            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(200,
                                          "",
                                          "",
                                          () => Core.Me.HasAura(AurasDefine.TrueNorth)));

            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(300,
                    "GALLOW => BEHIND !!!",
                    "/AEAssist;component/Resources/Images/General/ArrowDownHighlighted.png",
                    ReaperSpellHelper.GallowsPOSCheck)
            );

            CombatMessageManager.RegisterMessageStrategy(
                new CombatMessageStrategy(400,
                    "GIBBET => SIDE !!!",
                    "/AEAssist;component/Resources/Images/General/ArrowSidesHighlighted.png",
                    ReaperSpellHelper.GibbetPOSCheck)
            );
        }
    }
}