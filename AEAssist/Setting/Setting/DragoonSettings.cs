using PropertyChanged;

namespace AEAssist
{
    [AddINotifyPropertyChangedInterface]
    public class DragoonSettings : IBaseSetting
    {
        public DragoonSettings()
        {
            Reset();
        }

        public bool EarlyDecisionMode { get; set; }

        public bool LifeSurge { get; set; }//龙剑
        public bool LanceCharge { get; set; }//猛枪
        public bool SpineshatterDive { get; set; }//破碎冲
        public bool DragonfireDive { get; set; }//龙炎冲
        public bool BattleLitany { get; set; }//战斗连祷
        public bool Jump { get; set; }//跳跃-高跳
        public bool WyrmwindThrust { get; set; }//天龙点睛
        public bool Geirskogul { get; set; }//武神枪

        public void Reset()
        {
            EarlyDecisionMode = false;
            LifeSurge = true;
            LanceCharge = true;
            SpineshatterDive = true;
            DragonfireDive = true;
            BattleLitany = true;
            Jump = true;
            WyrmwindThrust = true;
            Geirskogul = true;
        }

        public void ResetToggles()
        {

        }

        public void OnLoad()
        {

        }
    }
}