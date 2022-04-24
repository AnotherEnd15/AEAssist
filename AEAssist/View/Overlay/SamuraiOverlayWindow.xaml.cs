﻿using System.Windows;
using System.Windows.Controls;
using AEAssist.AI;
using AEAssist.Define;

namespace AEAssist.View.Overlay
{
    public partial class SamuraiOverlayWindow : UserControl
    {
        public SamuraiOverlayWindow()
        {
            InitializeComponent();
        }
        private void UseArmsLength_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ArmsLength;
        }

        private void UseFeint_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Feint;
        }

        private void UseSprint_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.Sprint;
        }

        private void UseTrueNorth_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.TrueNorth;
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = false;
        }

        private void UsePotion_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilityUsePotion = true;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            OverlayManager.Instance.Close();
        }

        private void UseThirdEye_OnClick(object sender, RoutedEventArgs e)
        {
            AIRoot.GetBattleData<BattleData>().NextAbilitySpellId = SpellsDefine.ThirdEye;
        }
    }
}