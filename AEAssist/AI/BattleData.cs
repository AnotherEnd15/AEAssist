﻿using System.Collections.Generic;
using AEAssist.Define;
using AEAssist.Gamelog;
using AEAssist.Helper;
using AEAssist.TriggerSystem;

namespace AEAssist.AI
{
    public class BattleData : IBattleData
    {
        public int OpenerIndex;

        public BattleData()
        {
            ResetMaxAbilityTimes();
        }

        public long BattleStartTime { get; set; }
        public long CurrBattleTimeInMs { get; private set; }

        public void Update(long currTime)
        {
            CurrBattleTimeInMs = currTime - BattleStartTime;
            // var enemys = TargetMgr.Instance.EnemysIn25;
            // foreach (var v in enemys.Values)
            // {
            //     if (v.SpellCastInfo == null || !v.IsCasting)
            //         continue;
            //     LogHelper.Info($"Character {v.Name} Casting===>{v.SpellCastInfo.SpellEntity.LocalizedName}");
            // }

            // foreach (var v in Core.Me.CharacterAuras.AuraList)
            // {
            //     LogHelper.Info($"{v.LocalizedName} Id: {v.Id}  TimeLeft: {v.TimeLeft}");
            // }

            CalTriggerLine();
            AEGamelogManager.Instance.CheckLog();
        }

        #region BaseSpellControl

        public SpellEntity lastAbilitySpell;

        public long lastCastTime;

        public int lastGCDIndex;
        public SpellEntity lastGCDSpell;
        public int maxAbilityTimes;
        public bool LimitAbility;

        public HashSet<uint> LockSpellId = new HashSet<uint>();


        public void ResetMaxAbilityTimes()
        {
            maxAbilityTimes = SettingMgr.GetSetting<GeneralSettings>().MaxAbilityTimsInGCD;

            if (LimitAbility)
            {
                LogHelper.Info("limit maxAbilityTimes => 1");
                maxAbilityTimes = 1;
                LimitAbility = false;
            }
        }

        #endregion

        #region NextSpell

        private SpellEntity _NextAbilitySpellId;

        public SpellEntity NextAbilitySpellId
        {
            get => _NextAbilitySpellId;
            set
            {
                _NextAbilitySpellId = value;
                if (_NextAbilitySpellId != null) AbilityRetryEndTime = TimeHelper.Now() + 6000;

                LogHelper.Debug("NextAbility: " + NextAbilitySpellId);
            }
        }

        public bool NextAbilityUsePotion;

        private SpellEntity _NextGcdSpellId;

        public SpellEntity NextGcdSpellId
        {
            get => _NextGcdSpellId;
            set
            {
                _NextGcdSpellId = value;
                if (_NextGcdSpellId != null) GCDRetryEndTime = TimeHelper.Now() + 6000;
            }
        }

        public long GCDRetryEndTime;
        public long AbilityRetryEndTime;

        #endregion

        #region TriggerLine

        private readonly Dictionary<string, long> ExecutedTriggers = new Dictionary<string, long>();

        private readonly Dictionary<ITriggerCond, long> CondHitTime = new Dictionary<ITriggerCond, long>();


        private void CalTriggerLine()
        {
            var CurrTriggerLine = AEAssist.DataBinding.Instance.CurrTriggerLine;
            if (CurrTriggerLine == null)
                return;


            if (ExecutedTriggers.Count == CurrTriggerLine.Triggers.Count)
                return;
            foreach (var v in CurrTriggerLine.Triggers)
            {
                if (ExecutedTriggers.ContainsKey(v.Id))
                    continue;
                if (TriggerSystemMgr.Instance.HandleTriggers(v)) ExecutedTriggers.Add(v.Id, TimeHelper.Now());
            }
        }

        public long GetExecutedTriggersTime(string id)
        {
            ExecutedTriggers.TryGetValue(id, out var time);
            return time;
        }

        public void RecordCondHitTime(ITriggerCond cond)
        {
            CondHitTime[cond] = TimeHelper.Now();
        }

        public bool GetCondHitTime(ITriggerCond cond, out long time)
        {
            return CondHitTime.TryGetValue(cond, out time);
        }

        #endregion
    }
}