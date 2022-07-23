using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Managers;
using System.Threading.Tasks;
using System.Windows.Media;

namespace AEAssist.AI.Samurai.Ability
{
    public class SamuraiAbility_HissatsuShinten : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            var bd = AIRoot.GetBattleData<SamuraiBattleData>();

            if (SamuraiSpellHelper.SenCounts() == 3)
            {
                return -3;
            }

            if (ActionResourceManager.Samurai.Kenki >= 90)
            {
                return 6;
            }

            if (ActionResourceManager.Samurai.Kenki >= 25)
            {
                if (bd.CurrPhase == SamuraiPhase.CooldownPhase)
                {
                    // cooldown
                    if (ActionManager.LastSpellId == SpellsDefine.Kasha)
                    {
                        if (bd.burstingShintenCount < 1)
                        {
                            bd.burstingShintenCount++;
                            return 5;
                        }
                        
                        if (bd.burstingShintenCount > 1)
                        {
                            // reset
                            bd.burstingShintenCount = 0;
                            return -1;
                        }
                    }
                }
                
                if (bd.CurrPhase == SamuraiPhase.OddMinutesBurstPhase)
                {

                    if (bd.burstingShintenCount > 3)
                    {
                        // reset
                        bd.burstingShintenCount = 0;
                        return -1;
                    }

                    if (lastSpell == SpellsDefine.Kasha.GetSpellEntity() ||
                        lastSpell == SpellsDefine.Gekko.GetSpellEntity()
                        || lastSpell == SpellsDefine.Yukikaze.GetSpellEntity())
                    {

                        bd.burstingShintenCount++;
                        return 1;
                    }
                }

                if (bd.CurrPhase == SamuraiPhase.EvenMinutesBurstPhase)
                {
                    if (bd.burstingShintenCount > 4)
                    {
                        // reset
                        bd.burstingShintenCount = 0;
                        return -2;
                    }

                    if (lastSpell == SpellsDefine.Gekko.GetSpellEntity()
                        || lastSpell == SpellsDefine.Yukikaze.GetSpellEntity())
                    {

                        bd.burstingShintenCount++;
                        return 2;
                    }
                }
                
            }
            return -1;
        }

        public async Task<SpellEntity> Run()
        {
            var spell = SpellsDefine.HissatsuShinten.GetSpellEntity();
            if (spell == null) return null;
            if (await spell.DoAbility())
                return spell;
            return null;
        }
    }
}