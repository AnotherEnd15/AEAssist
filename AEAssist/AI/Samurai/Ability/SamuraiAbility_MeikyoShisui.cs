using System;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_MeikyoShisui : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {

            var needUseAoe = TargetHelper.CheckNeedUseAOE(2, 5);
            if (needUseAoe)
            {
                if (SpellsDefine.MeikyoShisui.IsReady())
                {
                    return 0;
                }
            }

            if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.CooldownPhase)
            {
                return -1;
            }

            if (!SpellsDefine.MeikyoShisui.IsReady())
            {
                return -14;
            }
            
            if (Core.Me.HasAura(AurasDefine.MeikyoShisui))
            {
                return -13;
            }

            // var charges = Math.Abs(SpellsDefine.MeikyoShisui.GetSpellEntity().SpellData.Charges - 1);
            // LogHelper.Info("Current charges after - 1: " + charges);

            if (AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.OddMinutesBurstPhase  
                || AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
            {
                if (SpellsDefine.MeikyoShisui.GetSpellEntity().SpellData.Charges >= 1.0)
                {
                    
                    {
                        LogHelper.Info("Use Meikyo...");
                        AIRoot.GetBattleData<SamuraiBattleData>().burstingMeikyoShisuiCount++;
                        return 0;
                    }
                }
            }
            
            return -5;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.MeikyoShisui;
            if (await spell.DoAbility())
            {
                return spell.GetSpellEntity();
            }
            return null;
        }
    }
}