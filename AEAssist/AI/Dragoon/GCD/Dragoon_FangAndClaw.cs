using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;


namespace AEAssist.AI.Dragoon.GCD
{
    public class DragoonGCD_FangAndClaw : IAIHandler
    {
        uint spell;//龙牙龙爪


        public int Check(SpellEntity lastSpell)
        {
            spell = SpellsDefine.FangAndClaw;
            if (!Core.Me.HasMyAura(AurasDefine.SharperFangandClaw))
                return -1;

            if (!spell.IsReady() || spell==0)
                return -1;
            return 0;
        }

        public async Task<SpellEntity> Run()
        {
            if (await spell.DoGCD()) return spell.GetSpellEntity();

            return null;
        }



        public static uint Getyinghua()
        {   
            var tar = Core.Me as Character;
            //LogHelper.Debug(tar.HasMyAuraWithTimeleft(AurasDefine.SurgingTempest, 10000).ToString());
            switch (ActionManager.LastSpellId) 
            {
                case SpellsDefine.TrueThrust://精准刺
                    return SpellsDefine.Disembowel;
                case SpellsDefine.Disembowel://开膛枪
                    return SpellsDefine.ChaosThrust;
                //case SpellsDefine.ChaosThrust://樱花怒放
                //    return SpellsDefine.WheelingThrust;
                //case SpellsDefine.ChaoticSpring://樱花缭乱
                //    return SpellsDefine.WheelingThrust;//龙尾大回旋
                //case SpellsDefine.WheelingThrust:
                //    return SpellsDefine.FangAndClaw;//龙牙龙爪-樱花连结束

                default:
                    return SpellsDefine.TrueThrust;
            }
            
        }
        public static uint Getzhici()
        {
            var tar = Core.Me as Character;
            //LogHelper.Debug(tar.HasMyAuraWithTimeleft(AurasDefine.SurgingTempest, 10000).ToString());
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.TrueThrust://精准刺
                    return SpellsDefine.VorpalThrust;//贯通刺
                case SpellsDefine.VorpalThrust:
                    return SpellsDefine.FullThrust;//直刺
                case SpellsDefine.FullThrust:
                //    return SpellsDefine.FangAndClaw;//龙牙龙爪
                //case SpellsDefine.HeavensThrust://苍穹刺
                //    return SpellsDefine.FangAndClaw;//龙牙龙爪
                //case SpellsDefine.FangAndClaw:
                //    return SpellsDefine.WheelingThrust;//龙尾大回旋-直刺连结束

                default:
                    return SpellsDefine.TrueThrust;
            }

        }

        public static uint GetAOE()
        {
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.DoomSpike://死天枪
                    return SpellsDefine.SonicThrust;//音速刺
                case SpellsDefine.SonicThrust:
                    return SpellsDefine.CoerthanTorment;//山境酷刑

                default:
                    return SpellsDefine.DoomSpike;
            }

        }

    }
}