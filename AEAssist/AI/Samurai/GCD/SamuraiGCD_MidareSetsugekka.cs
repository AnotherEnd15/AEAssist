using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;

namespace AEAssist.AI.Samurai.GCD
{
    public class SamuraiGCD_MidareSetsugekka : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            // LogHelper.Info("Current Phase: " + AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase);
            // LogHelper.Info("MidareCount = " + AIRoot.GetBattleData<SamuraiBattleData>().MidareSetsugekkaCount);
            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.CooldownPhase)
                {
                    if (AIRoot.GetBattleData<SamuraiBattleData>().MidareSetsugekkaCount < 1)
                    {
                        AIRoot.GetBattleData<SamuraiBattleData>().MidareSetsugekkaCount++;
                        return 0;   
                    }
                    // Already used 1 time so the next time we use it it will be in Oddminutes.
                    // only go into Oddphase If we have charges of MeikyoShisui
                    if (SpellsDefine.MeikyoShisui.IsReady())
                    {
                        AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase = SamuraiPhase.OddMinutesBurstPhase;
                    }
                    
                    // reset count.
                    AIRoot.GetBattleData<SamuraiBattleData>().MidareSetsugekkaCount = 0;
                    return 0;
                }
                
                if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.OddMinutesBurstPhase
                    || AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
                {
                    if (AIRoot.GetBattleData<SamuraiBattleData>().MidareSetsugekkaCount < 1)
                    {
                        AIRoot.GetBattleData<SamuraiBattleData>().MidareSetsugekkaCount++;
                        return 0;   
                    }
                    // reset count
                    AIRoot.GetBattleData<SamuraiBattleData>().MidareSetsugekkaCount = 0;
                    // go back to cooldown.
                    AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase = SamuraiPhase.CooldownPhase;
                    return 0;
                }
                
                return 0;
            }

            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SamuraiSpellHelper.GetMidareSetsuGekka();
            if (spell == null)
                return null;
            var ret = await spell.DoGCD();
            return ret ? spell : null;
        }
    }
}