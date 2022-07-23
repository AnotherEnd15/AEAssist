using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using AEAssist.AI;
using AEAssist.Define;
using AEAssist.Helper;
using PropertyChanged;

namespace AEAssist.View.Overlay

{
    [AddINotifyPropertyChangedInterface]
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// </summary>
    public partial class DragoonOverlayWindow : UserControl
    {
        public Action DragMove;

        public DragoonOverlayWindow()
        {
            InitializeComponent();
        }

        private void DragoonOverlayWindow_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // this.DragMove();
            }
        }

        // Surecast
        private void UseArmsLength_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ArmsLength.GetSpellEntity();
        }

        // Sprint
        private void UseSprint_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Sprint.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = false;
        }

        private void UsePotion_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = true;
        }


        // Assize
        private void UseAssize_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Assize.GetSpellEntity();
        }

        // Asylum
        private void UseAsylum_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Asylum.GetSpellEntity();
        }
        // Asylum Self
        private void UseAsylumSelf_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = new SpellEntity(SpellsDefine.Asylum, SpellTargetType.Self);

        }

        // LiturgyOfTheBell
        private void UseLiturgyoftheBell_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.LiturgyOfTheBell.GetSpellEntity();
        }

        // PresenceofMind
        private void UsePresenceofMind_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.PresenceofMind.GetSpellEntity();
        }

        // AfflatusRapture
        private void UseAfflatusRapture_OnClick(object sender, RoutedEventArgs e)
        {
            //AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.AfflatusRapture.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.AfflatusRapture.GetSpellEntity();
        }
        private void UseCureIII_OnClick(object sender, RoutedEventArgs e)
        {
            //AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.AfflatusRapture.GetSpellEntity();
            //AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ThinAir.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.Cure3.GetSpellEntity();
        }
        private void UseMedicaII_OnClick(object sender, RoutedEventArgs e)
        {
            //AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.AfflatusRapture.GetSpellEntity();
            //AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ThinAir.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.Medica2.GetSpellEntity();
        }

        // PlenaryIndulgence
        private void UsePlenaryIndulgence_OnClick(object sender, RoutedEventArgs e)
        {

            //AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.PlenaryIndulgence.GetSpellEntity();
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.PlenaryIndulgence.GetSpellEntity();
            //LogHelper.Debug("trying to use Pneuma next");
            //AIRoot.GetBattleData<BattleData>().NextGcdSpellId = SpellsDefine.Pneuma.GetSpellEntity();
        }

        // ThinAir
        private void UseThinAir_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ThinAir.GetSpellEntity();
        }



        private void Expander_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed) DragMove?.Invoke();
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.OverlayManager.Instance.Close();
        }
    }
}

