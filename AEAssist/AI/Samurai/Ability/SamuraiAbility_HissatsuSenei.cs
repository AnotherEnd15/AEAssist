using System;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_HissatsuSenei : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            LogHelper.Info("current Phase" +   AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase);
            if (ActionResourceManager.Samurai.Kenki < 25)
            {
                return -10;
            }

            if (!SpellsDefine.HissatsuSenei.GetSpellEntity().IsReady())
            {
                return -4;
            }

            if (SpellsDefine.HissatsuSenei.IsReady())
            {
                if (SpellsDefine.Ikishoten.IsReady())
                {
                    AIRoot.GetBattleData<SamuraiBattleData>().CurrPhase = SamuraiPhase.EvenMinutesBurstPhase;
                    return 0;
                }
            }

            return -4;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuSenei.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
            {
                return spell;
            }
            return null;
        }
    }
}