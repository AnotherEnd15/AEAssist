using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;


namespace AEAssist.AI.Dragoon.GCD
{
    public class DragoonGCD_WheelingThrust : IAIHandler
    {
        uint spell;//

        public int Check(SpellEntity lastSpell)
        {
            spell = SpellsDefine.WheelingThrust;
            if (!Core.Me.HasMyAura(AurasDefine.EnhancedWheelingThrust))
                return -1; 

            if (!spell.IsReady() || spell == 0)
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }

    }
}