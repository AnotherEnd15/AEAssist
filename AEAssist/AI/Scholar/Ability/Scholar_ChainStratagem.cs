using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Scholar.Ability
{
    public class ScholarAbility_ChainStratagem : IAIHandler
    {
        uint spell;
        static public uint GetSpell()
        {
            if (SpellsDefine.ChainStrategem.IsUnlock())
            {
                return SpellsDefine.ChainStrategem;
            }            
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (spell==0) return -1;
            if (!spell.IsReady())
                return -2;
            //LogHelper.Debug("NO10:" + spell.ToString());
            if (!Core.Me.CurrentTarget.IsBoss())
            {
                return -3;
            }
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}