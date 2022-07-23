using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Scholar.GCD
{
    public class ScholarGCD_SummonEos : IAIHandler
    {
        uint spell = SpellsDefine.SummonEos;
        
        public int Check(SpellEntity lastSpell)
        {
            if (Core.Me.HasAura(AurasDefine.Dissipation))
            {
                return -3;
            }
            if (GameObjectManager.PetObjectId != GameObjectManager.EmptyGameObject)
                return -2;

            if (!spell.IsReady())
                return -1;

            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            spell.GetSpellEntity().SpellTargetType = SpellTargetType.Self;

            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }


    }
}