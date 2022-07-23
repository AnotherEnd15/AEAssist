using System.Linq;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using Buddy.Coroutines;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Astrologian.GCD
{
    internal class AstGCDAspectedBenefic:IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {            
            if (!MovementManager.IsMoving) return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            LogHelper.Debug("刷吉星");
            var spell = SpellsDefine.AspectedBenefic.GetSpellEntity();
            var ret = await AstSpellHelper.CastAspectedBenefic();
            return ret ? spell : null;            

        }
    }
}
