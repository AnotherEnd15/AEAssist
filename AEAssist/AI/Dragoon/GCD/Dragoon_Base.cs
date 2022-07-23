using System.Threading.Tasks;
using AEAssist.Define;
using AEAssist.Helper;
using ff14bot;
using ff14bot.Managers;
using ff14bot.Objects;


namespace AEAssist.AI.Dragoon.GCD
{
    public class DragoonGCD_Base : IAIHandler
    {
        uint spell;//樱花连-直刺连
        static public uint GetSpell()
        {
            var aoeChecker = TargetHelper.CheckNeedUseAOE(5, 5, ConstValue.WhiteMageAOECount);
            var target = Core.Me.CurrentTarget as Character;
            bool Yingdot;
            if (Core.Me.ClassLevel <= 86)
                Yingdot = target.HasMyAuraWithTimeleft(AurasDefine.ChaosThrust, 12000);
            else
                Yingdot = target.HasMyAuraWithTimeleft(AurasDefine.ChaoticSpring, 12000);
            if (aoeChecker)//判断是否需要AOE
                return GetAOE();

            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.TrueThrust://精准刺
                    if (Yingdot)//樱花dot持续时间大于12秒就打直刺
                        return SpellsDefine.VorpalThrust;
                    else
                        return SpellsDefine.Disembowel;
                case SpellsDefine.RaidenThrust://龙眼雷电
                    if (Yingdot)//樱花dot持续时间大于12秒就打直刺
                        return SpellsDefine.VorpalThrust;
                    else
                        return SpellsDefine.Disembowel;
                case SpellsDefine.Disembowel://开膛枪
                    return SpellsDefine.ChaosThrust;//樱花怒放
                case SpellsDefine.VorpalThrust://贯通刺
                    return SpellsDefine.FullThrust;//直刺
                default:
                    return SpellsDefine.TrueThrust;
            }
        }

        public int Check(SpellEntity lastSpell)
        {
            spell = GetSpell();
            var target = Core.Me.CurrentTarget as Character;
            bool Yingdot;
            if (Core.Me.ClassLevel <= 86)
                Yingdot = target.HasMyAuraWithTimeleft(AurasDefine.ChaosThrust, 12000);
            else
                Yingdot = target.HasMyAuraWithTimeleft(AurasDefine.ChaoticSpring, 12000);
            
            LogHelper.Info($"当前等级为：{Core.Me.ClassLevel}，下一个技能: {spell.ToString()},上一个技能:{ActionManager.LastSpellId},dot需要补吗 {!Yingdot}");
            if (!spell.IsUnlock())
                spell = SpellsDefine.TrueThrust;

            if (!spell.IsReady())
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
            //LogHelper.Info($"樱花判断");
            switch (ActionManager.LastSpellId) 
            {
                case SpellsDefine.TrueThrust://精准刺                    
                    return SpellsDefine.Disembowel;
                case SpellsDefine.Disembowel://开膛枪
                    return SpellsDefine.ChaosThrust;

                default:
                    return SpellsDefine.TrueThrust;
            }
            
        }
        public static uint Getzhici()
        {
            var tar = Core.Me as Character;
            //LogHelper.Info($"直刺判断");
            switch (ActionManager.LastSpellId)
            {
                case SpellsDefine.TrueThrust://精准刺
                    return SpellsDefine.VorpalThrust;
                case SpellsDefine.VorpalThrust://贯通刺
                    return SpellsDefine.FullThrust;

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