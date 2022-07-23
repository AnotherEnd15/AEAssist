using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot.Enums;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class ReaperSettings : IBaseSetting
    {
        public ReaperSettings()
        {
            Reset();
        }

        public bool GallowsPrefer { get; set; }

        public bool EarlyDecisionMode { get; set; }

        public bool DoubleEnshroudPrefer { get; set; } = true;

        public bool UseHarpe { get; set; }
        public string ReaperOpener { get; set; } = "Default";

        public void Reset()
        {
            GallowsPrefer = false;
            EarlyDecisionMode = true;
            DoubleEnshroudPrefer = true;
            UseHarpe = false;
            ReaperOpener = "Default";
        }

        public void OnLoad()
        {
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Reaper] = ReaperOpener;
            LogHelper.Info($"Reaper Opener: {ReaperOpener}");
        }
    }
}