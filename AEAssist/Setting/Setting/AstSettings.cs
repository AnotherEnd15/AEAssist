using AEAssist.Define;
using AEAssist.Helper;
using AEAssist.Opener;
using ff14bot.Enums;
using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class AstSettings : IBaseSetting
    {
        public AstSettings()
        {
            Reset();
        }
        public void Reset()
        {
            EarlyDecisionMode = true;
            Dot_TimeLeft = ConstValue.AuraTick;
            TTK_Aero = 30;
            AstOpener = "Default";

        }
        public void OnLoad()
        {
            OpenerMgr.Instance.SpecifyOpenerByName[ClassJobType.Astrologian] = AstOpener;
            LogHelper.Info($"Ast Opener: {AstOpener}");
        }
        public int Dot_TimeLeft { get; set; } = ConstValue.AuraTick;
        public int TTK_Aero { get; set; }
        public bool Divination { get; set; } = false;
        public bool Lightspeed { get; set; } = false;
        public int LucidDreamingTrigger { get; set; } = ConstValue.LucidDreamingDefaultRefresh;
        public bool LucidDreamingToggle { get; set; } = true;
        public bool SwiftResToggle { get; set; } = true;

        public bool DivinationToggle { get; set; } = true;
        public bool LighSpeedToggle { get; set; } = true;
        public bool NeutralSectToggle { get; set; } = true;
        public bool HoroScopeToggle { get; set; } = true;
        
        public bool AstHalfCard { get; set; } = true;
        public bool Heal { get; set; } = true;
        public bool GcdHeal { get; set; } = true;
        public bool EarlyDecisionMode { get; set; }
        public string AstOpener { get; set; } = "Default";
        public int AstResPriority { get; set; } = 0;
        public int AstCardWeight { get; set; } = 18;
        public int MnkCardWeight { get; set; } = 3;
        public int BlmCardWeight { get; set; } = 8;
        public int DrgCardWeight { get; set; } = 5;
        public int SamCardWeight { get; set; } = 1;
        public int MchCardWeight { get; set; } = 11;
        public int SmnCardWeight { get; set; } = 6;
        public int BrdCardWeight { get; set; } = 10;
        public int NinCardWeight { get; set; } = 2;
        public int RdmCardWeight { get; set; } = 9;
        public int DncCardWeight { get; set; } = 7;
        public int PldCardWeight { get; set; } = 17;
        public int WarCardWeight { get; set; } = 16;
        public int DrkCardWeight { get; set; } = 0;
        public int GnbCardWeight { get; set; } = 12;
        public int WhmCardWeight { get; set; } = 14;
        public int SchCardWeight { get; set; } = 15;
        public int RprCardWeight { get; set; } = 4;
        public int SgeCardWeight { get; set; } = 13;
        public int BluCardWeight { get; set; } = 19;

        public int AstHalfCardWeight { get; set; } = 18;
        public int MnkHalfCardWeight { get; set; } = 2;
        public int BlmHalfCardWeight { get; set; } = 7;
        public int DrgHalfCardWeight { get; set; } = 5;
        public int SamHalfCardWeight { get; set; } = 0;
        public int MchHalfCardWeight { get; set; } = 8;
        public int SmnHalfCardWeight { get; set; } = 6;
        public int BrdHalfCardWeight { get; set; } = 9;
        public int NinHalfCardWeight { get; set; } = 3;
        public int RdmHalfCardWeight { get; set; } = 10;
        public int DncHalfCardWeight { get; set; } = 11;
        public int PldHalfCardWeight { get; set; } = 14;
        public int WarHalfCardWeight { get; set; } = 13;
        public int DrkHalfCardWeight { get; set; } = 12;
        public int GnbHalfCardWeight { get; set; } = 1;
        public int WhmHalfCardWeight { get; set; } = 15;
        public int SchHalfCardWeight { get; set; } = 16;
        public int RprHalfCardWeight { get; set; } = 4;
        public int SgeHalfCardWeight { get; set; } = 17;
        public int BluHalfCardWeight { get; set; } = 19;

    }
}
