using System;

namespace SelfQuest.QuestSystem
{
    /// <summary>
    /// A quest that may have a deadline. If a due date is provided, a countdown starts at creation time.
    /// If no due date is provided, the quest is considered ever-present until completed or deleted.
    /// </summary>
    [Serializable]
    public class TimedQuest
    {
        public string Name { get; }
        public string Description { get; set; }

        /// <summary>
        /// When the quest timing started (creation time for current spec).
        /// </summary>
        public DateTime StartTime { get; }

        /// <summary>
        /// Optional due date (UTC). Null means no deadline (ever-present).
        /// </summary>
        public DateTime? DueDate { get; }

        /// <summary>
        /// External time provider for deterministic behavior and testing.
        /// </summary>
        private readonly ITimeProvider _time;

        public bool IsEverPresent => !DueDate.HasValue;

        public bool IsOverdue
        {
            get
            {
                if (IsEverPresent) return false;
                return _time.Now >= DueDate!.Value;
            }
        }

        public TimeSpan? TimeRemaining
        {
            get
            {
                if (IsEverPresent) return null;
                var remaining = DueDate!.Value - _time.Now;
                if (remaining < TimeSpan.Zero) return TimeSpan.Zero;
                return remaining;
            }
        }

        public TimedQuest(string name, string description, DateTime? dueDateUtc, ITimeProvider timeProvider)
        {
            Name = name;
            Description = description;
            _time = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));

            // Normalize to UTC if provided
            if (dueDateUtc.HasValue && dueDateUtc.Value.Kind == DateTimeKind.Unspecified)
            {
                dueDateUtc = DateTime.SpecifyKind(dueDateUtc.Value, DateTimeKind.Utc);
            }
            DueDate = dueDateUtc?.ToUniversalTime();

            StartTime = _time.Now;
        }
    }
}
