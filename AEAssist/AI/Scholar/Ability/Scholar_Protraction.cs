using System.Linq;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;

namespace AEAssist.AI.Scholar.Ability
{
    public class ScholarAbility_Protraction : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (SpellsDefine.Protraction.IsUnlock())
                return SpellsDefine.Protraction;
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (spell==0) return -1;
            if (!spell.IsReady())
                return -2;
            //LogHelper.Debug("NO10:" + spell.ToString());            
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 80f && r.IsTank());
            if (skillTarget == null)
            {
                return -3;
            }            
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            var skillTarget = GroupHelper.CastableAlliesWithin30.FirstOrDefault(r => r.CurrentHealth > 0 && r.CurrentHealthPercent <= 80f && r.IsTank());            
            var spell = new SpellEntity(SpellsDefine.Protraction, skillTarget as BattleCharacter);
            //await spell.DoAbility();
            if (await spell.DoAbility()) return spell;
            return null;
        }
    }

}