namespace MidiPlay.Instruments
{
    public interface IInstrument
    {
        void Create(int fd);
        Note GetNoteSignal(string name, int octave, double time);
    }
}
