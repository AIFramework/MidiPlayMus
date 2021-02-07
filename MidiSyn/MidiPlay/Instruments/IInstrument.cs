namespace Midi.Instruments
{
    public interface IInstrument
    {
        void Create(int fd);
        NoteVec GetNoteSignal(string name, int octave, double time);
    }
}
