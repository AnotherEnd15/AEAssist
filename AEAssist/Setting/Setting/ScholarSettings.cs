using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot.Enums;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class ScholarSettings : IBaseSetting
    {
        public ScholarSettings()
        {
            Reset();
        }
        public void Reset()
        {
            EarlyDecisionMode = false;//提前决策模式
            Dot_TimeLeft = ConstValue.AuraTick;
            TTK_Aero = 30;
            SchOpener = "Default";
            
        }
        public void OnLoad()
        {
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Scholar] = SchOpener;
            LogHelper.Info($"Sch Opener: {SchOpener}");
        }
        public int Dot_TimeLeft { get; set; } = ConstValue.AuraTick;
        public int TTK_Aero { get; set; }

        public int LucidDreamingTrigger { get; set; } = ConstValue.LucidDreamingDefaultRefresh;
        public bool LucidDreamingToggle { get; set; } = true;
        public bool SwiftResToggle { get; set; } = true;
        public bool Heal { get; set; } = true;
        public bool EarlyDecisionMode { get; set; }
        public string SchOpener { get; set; } = "Default";
        public int SchResPriority { get; set; } = 0;
        
    }
}
