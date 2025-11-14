using System;
using System.Linq;
using NUnit.Framework;
using SelfQuest.QuestSystem;

namespace Tests.QuestSystem
{
    public class CachedQuestRepositoryTests
    {
        private static QuestTemplate TemplateA => new(
            name: "Hydrate",
            description: "Drink 8 glasses of water",
            subQuests: new[] { new SubQuestTemplate("Fill bottle"), new SubQuestTemplate("Track glasses") },
            assignedSkills: new[] { "Health" }
        );

        [Test]
        public void Save_And_List_Templates()
        {
            var repo = new CachedQuestRepository();
            repo.SaveTemplate(TemplateA);

            var all = repo.GetAllTemplates();
            Assert.That(all.Select(t => t.Name), Is.EquivalentTo(new[] { "Hydrate" }));
        }

        [Test]
        public void CreateTimed_From_Template_PreservesNameAndDescription()
        {
            var repo = new CachedQuestRepository();
            repo.SaveTemplate(TemplateA);
            var time = new ManualTimeProvider(new DateTime(2025, 1, 2, 9, 0, 0, DateTimeKind.Utc));

            var due = time.Now.AddHours(3);
            var q = repo.CreateTimedFromTemplate("Hydrate", due, time);

            Assert.That(q.Name, Is.EqualTo("Hydrate"));
            Assert.That(q.Description, Is.EqualTo("Drink 8 glasses of water"));
            Assert.That(q.DueDate, Is.EqualTo(due));
            Assert.That(q.IsEverPresent, Is.False);
        }

        [Test]
        public void CreateRepeated_From_Template_PreservesSkillsAndSubquests()
        {
            var repo = new CachedQuestRepository();
            repo.SaveTemplate(TemplateA);
            var time = new ManualTimeProvider(new DateTime(2025, 1, 2, 6, 0, 0, DateTimeKind.Utc));

            var rq = repo.CreateRepeatedFromTemplate("Hydrate", TimeSpan.FromDays(1), null, time);

            Assert.That(rq.AssignedSkills, Is.EquivalentTo(new[] { "Health" }));
            Assert.That(rq.CurrentInstance.SubQuests.Count, Is.EqualTo(2));
        }

        [Test]
        public void UnknownTemplate_Throws()
        {
            var repo = new CachedQuestRepository();
            var time = new ManualTimeProvider(new DateTime(2025, 1, 2, 6, 0, 0, DateTimeKind.Utc));
            Assert.Throws<ArgumentException>(() => repo.CreateTimedFromTemplate("Missing", null, time));
            Assert.Throws<ArgumentException>(() => repo.CreateRepeatedFromTemplate("Missing", TimeSpan.FromDays(1), null, time));
        }

        [Test]
        public void InvalidRepeatPeriod_Throws()
        {
            var repo = new CachedQuestRepository();
            repo.SaveTemplate(TemplateA);
            var time = new ManualTimeProvider(new DateTime(2025, 1, 2, 6, 0, 0, DateTimeKind.Utc));
            Assert.Throws<ArgumentOutOfRangeException>(() => repo.CreateRepeatedFromTemplate("Hydrate", TimeSpan.Zero, null, time));
        }
    }
}
