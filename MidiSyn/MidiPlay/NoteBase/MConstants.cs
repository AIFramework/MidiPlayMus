namespace Midi.NoteBase
{
    public static partial class MConstants
    {
        public static readonly string[] _notesBase = { "C", "D", "E", "F", "G", "A", "B" };
        public static readonly string[] _notesAll = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        public static readonly int _tonesCount = 108; // 0, 0.5, 1, 1.5, ..., 53.5
        public static readonly int _octavesCount = 10;
        public static readonly int _notesWithOctaveCount = 128;
        public static readonly int _bowLen = _octavesCount + _notesAll.Length + _tonesCount;
    }
}
