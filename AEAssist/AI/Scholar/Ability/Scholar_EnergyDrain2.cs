using System.Collections.Generic;
using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;

namespace AEAssist.AI.Scholar.Ability
{
    public class ScholarAbility_EnergyDrain2 : IAIHandler
    {
        uint spell;        
        static public uint GetSpell()
        {
            List<uint> raidbuffs = new List<uint>
            {
                AurasDefine.ChainStratagem
            };
            if (Core.Me.HasAura(AurasDefine.Dissipation) && ActionResourceManager.Scholar.Aetherflow > 0 && SpellsDefine.Aetherflow.IsReady())
            {
                return SpellsDefine.EnergyDrain2;//打空转化豆子
            }
            if (ActionResourceManager.Scholar.Aetherflow > 0 && SpellsDefine.Aetherflow.CoolDownInGCDs(3))//吸收转好前打光豆子
                return SpellsDefine.EnergyDrain2;
            return 0;
        }
        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            if (spell == 0) return -1;
            if (!spell.IsReady())
                return -1;
            //LogHelper.Debug("NO10:" + spell.ToString());
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoAbility()) return spell.GetSpellEntity();

            return null;
        }
    }

}