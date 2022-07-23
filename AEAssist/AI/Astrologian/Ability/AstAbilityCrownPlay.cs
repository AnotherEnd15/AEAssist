using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Astrologian.Ability
{
    internal class AstAbilityCrownPlay : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            if (!SpellsDefine.CrownPlay.IsReady()) return -1;
            if (AIRoot.Instance.CloseBurst)
                return -3;
            if (!(Core.Me.HasAura(AurasDefine.LordOfCrownsDrawn) || Core.Me.HasAura(AurasDefine.LadyOfCrownsDrawn))) return -2;
            if (Core.Me.HasAura(AurasDefine.LordOfCrownsDrawn))
            {
                if (SpellsDefine.MinorArcana.GetSpellEntity().Cooldown.TotalSeconds < 10)
                {
                    return 0;
                }
                //if (TargetHelper.GetNearbyEnemyCount(Core.Me, 20, 20) > 2)
                //{
                    //return 0;
                //}

            }
            if (Core.Me.HasAura(AurasDefine.LadyOfCrownsDrawn))
            {
                if (SpellsDefine.MinorArcana.GetSpellEntity().Cooldown.TotalSeconds < 10)
                {
                    return 0;
                }
                if (GroupHelper.CastableAlliesWithin20.Count(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 50f) > 3)
                {
                    return 0;
                }
            }

            return -5;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.CrownPlay.GetSpellEntity();
            if (spell == null) return null;
            var ret = await spell.DoAbility();
            return ret ? spell : null;

        }
    }
}
