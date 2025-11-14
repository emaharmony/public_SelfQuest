using System;
using System.Collections.Generic;
using System.Linq;

namespace SelfQuest.QuestSystem
{
    /// <summary>
    /// In-memory repository for saved quest templates (Cached/Saved Quests).
    /// Provides factory methods to create Timed or Repeated quests from templates.
    /// </summary>
    public sealed class CachedQuestRepository
    {
        private readonly Dictionary<string, QuestTemplate> _templates = new(StringComparer.OrdinalIgnoreCase);

        public void SaveTemplate(QuestTemplate template)
        {
            if (template == null) throw new ArgumentNullException(nameof(template));
            _templates[template.Name] = template;
        }

        public IReadOnlyList<QuestTemplate> GetAllTemplates()
        {
            return _templates.Values.OrderBy(t => t.Name, StringComparer.OrdinalIgnoreCase).ToList();
        }

        public TimedQuest CreateTimedFromTemplate(string name, DateTime? dueDateUtc, ITimeProvider time)
        {
            var t = GetTemplateOrThrow(name);
            return new TimedQuest(t.Name, t.Description, dueDateUtc, time);
        }

        public RepeatedQuest CreateRepeatedFromTemplate(string name, TimeSpan repeatPeriod, DateTime? overallDueDateUtc, ITimeProvider time)
        {
            var t = GetTemplateOrThrow(name);
            return new RepeatedQuest(t, repeatPeriod, overallDueDateUtc, time);
        }

        private QuestTemplate GetTemplateOrThrow(string name)
        {
            if (!_templates.TryGetValue(name, out var t))
                throw new ArgumentException($"No template named '{name}'", nameof(name));
            return t;
        }
    }
}
