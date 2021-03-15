using Midi.NoteBase;
using NUnit.Framework;

namespace Midi.UnitTests
{
    [TestFixture]
    public class AccordTests
    {
        [Test]
        public void AddToneTest()
        {
            //Arrange
            var baseNotes = new string[] { "E1", "G2", "F#3", "E2", "C5" };
            var tones = new float[] { 1.5f, 1.5f, 7f, 7f, 6f };
            var goalNotes = new string[] { "G1", "A#2", "G#4", "F#3", "C6" };
            var testTones = new string[tones.Length];

            //Act
            for (int i = 0; i < testTones.Length; i++)
            {
                testTones[i] = Accord.AddTone(baseNotes[i], tones[i]);
            }

            //Assert
            for (int i = 0; i < testTones.Length; i++)
                Assert.AreEqual(testTones[i], goalNotes[i]);
        }
    }
}
