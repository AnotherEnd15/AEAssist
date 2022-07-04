﻿using System.Collections.Generic;

namespace AEAssist.AI.Ninja.SpellQueue
{
    public class SpellQueue_HyoshoRanryu : IAISpellQueue
    {
        public List<IAISpellQueueSlot> SlotQueue { get; } = new List<IAISpellQueueSlot>()
        {
            new SpellQueueSlot_HyoshoRanryu(),
        };
    }
}