using AI;
using Midi.NoteBase;
using Midi.NoteSeqData;

namespace WindowsFormsApp1
{
    public class DatasetGen
    {
        public static Vector[] VectorsFromAudio(string name) 
        {
            var noteSeq = NoteSeq.LoadMidiAsNoteSequence(name);
            var timesteps = NoteSeq.GroupByTimeStep(noteSeq);
            Vector[] melody = new Vector[timesteps.Length];

            for (int i = 0; i < melody.Length; i++)
                melody[i] = Accord.ToBOW(timesteps[i]).TransformVector(x => x > 0 ? 1 : 0);

            return melody;
        }
    }
}
