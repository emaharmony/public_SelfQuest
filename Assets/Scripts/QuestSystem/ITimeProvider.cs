using System;

namespace SelfQuest.QuestSystem
{
    /// <summary>
    /// Abstraction over time to enable deterministic testing.
    /// </summary>
    public interface ITimeProvider
    {
        DateTime Now { get; }
    }
}
