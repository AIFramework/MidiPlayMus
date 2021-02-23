using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Midi.NoteBase;
using NUnit.Framework;

namespace Midi.UnitTests
{
    [TestFixture]
    public class IntervalTests
    {
        [Test]
        public void RecognizeToneTest()
        {
            //Arrange
            var baseNotes = new string[] { "E1", "G2", "F#3", "E2", "C5" };
            var tones = new float[] { 1.5f, 1.5f, 7f, 7f, 6f };
            var goalNotes = new string[] { "G1", "A#2", "G#4", "F#3", "C6" };
            var testTones = new float[tones.Length];

            //Act
            for (int i = 0; i < testTones.Length; i++)
            {
                testTones[i] = Interval.RecognizeTone(baseNotes[i], goalNotes[i]);
            }

            //Assert
            for (int i = 0; i < testTones.Length; i++)
                Assert.AreEqual(testTones[i], tones[i]);
        }
    }
}
