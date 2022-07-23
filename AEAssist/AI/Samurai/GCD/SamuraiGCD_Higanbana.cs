using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Objects;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_Higanbana : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // var ta = Core.Me.CurrentTarget as Character;
            // if (SamuraiSpellHelper.SenCounts() == 1)
            //     if (ta.HasMyAuraWithTimeleft(AurasDefine.Higanbana, 3000))
            //         return 10;

            if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.OddMinutesBurstPhase
                || AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                return 0;
            }
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            if (await SpellsDefine.Higanbana.DoGCD())
            {
                AIRoot.GetBattleData<SamuraiBattleData>().higanBanaCount++;
                AIRoot.GetBattleData<SamuraiBattleData>().CurrCombo = SamuraiComboStages.HiganBana;
                return SpellsDefine.Higanbana.GetSpellEntity();
            }
            return null;
        }
    }
}