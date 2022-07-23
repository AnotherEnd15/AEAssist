using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_Dot : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var tar = Core.Me.CurrentTarget as Character;
            if (!AEAssist.DataBinding.Instance.UseDot)
                return -1;
            if (TTKHelper.IsTargetTTK(tar))
                return -2;

            if (DotBlacklistHelper.IsBlackList(Core.Me.CurrentTarget as Character))
                return -10;

            if (SamuraiSpellHelper.SenCounts() != 1)
            {
                return -4;
            }

            if (Core.Me.HasMyAura(AurasDefine.Kaiten) || SpellsDefine.HissatsuKaiten.RecentlyUsed())
            {
                if (!tar.HasMyAuraWithTimeleft(AurasDefine.Higanbana, 10000))
                {
                    return 1;
                }
            }
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SamuraiSpellHelper.Burst();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            if (ret)
                return spell;
            return null;
        }
    }
}