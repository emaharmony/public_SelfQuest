using System;
using System.Collections.Generic;
using System.Linq;

namespace SelfQuest.QuestSystem
{
    public enum QuestInstanceStatus
    {
        Active,
        Completed,
        Failed
    }

    [Serializable]
    public sealed class SubQuestTemplate
    {
        public string Title { get; }
        public SubQuestTemplate(string title) => Title = title;
    }

    [Serializable]
    public sealed class QuestTemplate
    {
        public string Name { get; }
        public string Description { get; }
        public IReadOnlyList<SubQuestTemplate> SubQuests { get; }
        public IReadOnlyList<string> AssignedSkills { get; }

        public QuestTemplate(string name, string description, IEnumerable<SubQuestTemplate>? subQuests = null, IEnumerable<string>? assignedSkills = null)
        {
            Name = name;
            Description = description;
            SubQuests = (subQuests ?? Enumerable.Empty<SubQuestTemplate>()).ToList();
            AssignedSkills = (assignedSkills ?? Enumerable.Empty<string>()).ToList();
        }
    }

    [Serializable]
    public sealed class QuestInstance
    {
        public string Name { get; }
        public string Description { get; }
        public DateTime StartTimeUtc { get; }
        public QuestInstanceStatus Status { get; private set; }
        public List<string> SubQuests { get; } = new();

        public QuestInstance(string name, string description, DateTime startUtc, IEnumerable<SubQuestTemplate> subs)
        {
            Name = name;
            Description = description;
            StartTimeUtc = startUtc;
            Status = QuestInstanceStatus.Active;
            foreach (var s in subs) SubQuests.Add(s.Title);
        }

        public void Complete()
        {
            if (Status == QuestInstanceStatus.Active)
                Status = QuestInstanceStatus.Completed;
        }

        public void Fail()
        {
            if (Status == QuestInstanceStatus.Active)
                Status = QuestInstanceStatus.Failed;
        }
    }

    /// <summary>
    /// A quest that repeats every given period. It inherits TimedQuest to support an overall deadline.
    /// When the period elapses, it spawns a new active instance. If the previous instance is still active,
    /// it is marked Failed and archived.
    /// </summary>
    [Serializable]
    public sealed class RepeatedQuest : TimedQuest
    {
        private readonly ITimeProvider _time;
        private readonly QuestTemplate _template;
        private readonly TimeSpan _repeatPeriod;

        public QuestInstance CurrentInstance { get; private set; }
        public List<QuestInstance> History { get; } = new();
        public IReadOnlyList<string> AssignedSkills => (IReadOnlyList<string>)_template.AssignedSkills;

        private DateTime _nextSpawnUtc;

        public RepeatedQuest(QuestTemplate template, TimeSpan repeatPeriod, DateTime? overallDueDateUtc, ITimeProvider time)
            : base(template.Name, template.Description, overallDueDateUtc, time)
        {
            _template = template;
            _repeatPeriod = repeatPeriod <= TimeSpan.Zero
                ? throw new ArgumentOutOfRangeException(nameof(repeatPeriod), "Repeat period must be positive")
                : repeatPeriod;
            _time = time;

            CurrentInstance = CreateInstance();
            History.Add(CurrentInstance);
            _nextSpawnUtc = StartTime + _repeatPeriod;
        }

        private QuestInstance CreateInstance()
        {
            return new QuestInstance(_template.Name, _template.Description, _time.Now, _template.SubQuests);
        }

        /// <summary>
        /// Advance logic. Should be called periodically (e.g., per frame or on a timer) to roll the quest.
        /// </summary>
        public void Tick()
        {
            // Do not spawn beyond overall due date
            if (DueDate.HasValue && _time.Now >= DueDate.Value)
            {
                return;
            }

            // Spawn as many intervals as have passed, but cap at one spawn per call for simplicity
            if (_time.Now >= _nextSpawnUtc)
            {
                // If current is still active, fail it
                if (CurrentInstance.Status == QuestInstanceStatus.Active)
                {
                    CurrentInstance.Fail();
                }

                // Start new
                CurrentInstance = CreateInstance();
                History.Add(CurrentInstance);
                _nextSpawnUtc = _nextSpawnUtc + _repeatPeriod;
            }
        }

        public void CompleteCurrent()
        {
            CurrentInstance.Complete();
        }
    }
}
