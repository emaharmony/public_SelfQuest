using System;

namespace SelfQuest.QuestSystem
{
    /// <summary>
    /// Deterministic time provider for tests. Start with a fixed time and advance manually.
    /// </summary>
    public sealed class ManualTimeProvider : ITimeProvider
    {
        public DateTime Now { get; private set; }

        public ManualTimeProvider(DateTime startUtc)
        {
            if (startUtc.Kind == DateTimeKind.Unspecified)
                startUtc = DateTime.SpecifyKind(startUtc, DateTimeKind.Utc);
            Now = startUtc.ToUniversalTime();
        }

        public void Advance(TimeSpan delta)
        {
            Now = Now.Add(delta);
        }
    }
}
