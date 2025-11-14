using System;
using NUnit.Framework;
using SelfQuest.QuestSystem;

namespace Tests.QuestSystem
{
    public class TimedQuestTests
    {
        [Test]
        public void StartsCountdown_WhenDueDateProvided()
        {
            var time = new ManualTimeProvider(new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc));
            var due = time.Now.AddHours(2);
            var q = new TimedQuest("Read", "Read 20 pages", due, time);

            Assert.That(q.IsEverPresent, Is.False);
            Assert.That(q.StartTime, Is.EqualTo(time.Now));
            Assert.That(q.DueDate, Is.EqualTo(due));
            Assert.That(q.TimeRemaining!.Value.TotalHours, Is.EqualTo(2).Within(0.001));

            time.Advance(TimeSpan.FromMinutes(30));
            Assert.That(q.TimeRemaining!.Value.TotalMinutes, Is.EqualTo(90).Within(0.001));
            Assert.That(q.IsOverdue, Is.False);
        }

        [Test]
        public void EverPresent_WhenNoDueDate()
        {
            var time = new ManualTimeProvider(new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc));
            var q = new TimedQuest("Meditate", "10 min", null, time);

            Assert.That(q.IsEverPresent, Is.True);
            Assert.That(q.DueDate, Is.Null);
            Assert.That(q.TimeRemaining, Is.Null);
            Assert.That(q.IsOverdue, Is.False);
        }

        [Test]
        public void Overdue_WhenPastDueDate()
        {
            var time = new ManualTimeProvider(new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc));
            var q = new TimedQuest("Workout", "30 min", time.Now.AddMinutes(10), time);

            time.Advance(TimeSpan.FromMinutes(11));
            Assert.That(q.IsOverdue, Is.True);
            Assert.That(q.TimeRemaining.Value.TotalSeconds, Is.EqualTo(0).Within(0.001));
        }
    }
}
