using System;
using NUnit.Framework;
using SelfQuest.QuestSystem;

namespace Tests.QuestSystem
{
    public class RepeatedQuestTests
    {
        private static QuestTemplate MakeTemplate()
        {
            return new QuestTemplate(
                name: "Daily Journal",
                description: "Write 3 sentences",
                subQuests: new[] { new SubQuestTemplate("Open notebook"), new SubQuestTemplate("Write 3 sentences") },
                assignedSkills: new[] { "Writing", "Mindfulness" }
            );
        }

        [Test]
        public void SpawnsNewAndFailsPrevious_WhenIntervalElapsesAndNotCompleted()
        {
            var time = new ManualTimeProvider(new DateTime(2025, 1, 1, 7, 0, 0, DateTimeKind.Utc));
            var rq = new RepeatedQuest(MakeTemplate(), repeatPeriod: TimeSpan.FromDays(1), overallDueDateUtc: null, time);

            Assert.That(rq.CurrentInstance, Is.Not.Null);
            Assert.That(rq.CurrentInstance.Status, Is.EqualTo(QuestInstanceStatus.Active));
            Assert.That(rq.CurrentInstance.SubQuests.Count, Is.EqualTo(2));
            Assert.That(rq.AssignedSkills, Is.EquivalentTo(new[] { "Writing", "Mindfulness" }));

            // Advance exactly one period
            time.Advance(TimeSpan.FromDays(1));
            rq.Tick();

            // Previous should be failed, a new active one started
            Assert.That(rq.History.Count, Is.EqualTo(2));
            Assert.That(rq.History[0].Status, Is.EqualTo(QuestInstanceStatus.Failed));
            Assert.That(rq.History[1].Status, Is.EqualTo(QuestInstanceStatus.Active));
            Assert.That(rq.CurrentInstance, Is.EqualTo(rq.History[1]));
            Assert.That(rq.CurrentInstance.SubQuests.Count, Is.EqualTo(2)); // template preserved
        }

        [Test]
        public void CompletedInstance_RemainsCompleted_WhenNextSpawns()
        {
            var time = new ManualTimeProvider(new DateTime(2025, 1, 1, 7, 0, 0, DateTimeKind.Utc));
            var rq = new RepeatedQuest(MakeTemplate(), TimeSpan.FromHours(12), null, time);

            rq.CompleteCurrent();
            Assert.That(rq.CurrentInstance.Status, Is.EqualTo(QuestInstanceStatus.Completed));

            time.Advance(TimeSpan.FromHours(12));
            rq.Tick();

            Assert.That(rq.History.Count, Is.EqualTo(2));
            Assert.That(rq.History[0].Status, Is.EqualTo(QuestInstanceStatus.Completed));
            Assert.That(rq.CurrentInstance.Status, Is.EqualTo(QuestInstanceStatus.Active));
        }

        [Test]
        public void StopsRepeating_WhenOverallDueReached()
        {
            var time = new ManualTimeProvider(new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            var overallDue = time.Now.AddDays(2);
            var rq = new RepeatedQuest(MakeTemplate(), TimeSpan.FromDays(1), overallDue, time);

            // After 1 day -> second instance
            time.Advance(TimeSpan.FromDays(1));
            rq.Tick();
            Assert.That(rq.History.Count, Is.EqualTo(2));

            // At overall due, should not spawn third
            time.Advance(TimeSpan.FromDays(1)); // Now == overallDue
            rq.Tick();
            Assert.That(rq.History.Count, Is.EqualTo(2));
            Assert.That(rq.IsOverdue, Is.True); // inherits TimedQuest behavior
        }
    }
}
