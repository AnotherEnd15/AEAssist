using AEAssist.Define;
using AEAssist.Helper;
using ff14bot.Objects;
using System.Linq;
using System.Threading.Tasks;

namespace AEAssist.AI.Sage.Ability
{
    public class SageAbilityKardia : IAIHandler
    {
        public int Check(SpellEntity lastSpell)
        {
            if (!SpellsDefine.Kardia.IsReady()) return -1;            
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 75f);
            if (skillTarget == null)
            {
                return -3;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 75f);
            var spell = new SpellEntity(SpellsDefine.Kardia, skillTarget as BattleCharacter);
            //await spell.DoAbility();
            if (await spell.DoAbility()) return spell;
            return null;
        }
    }
}