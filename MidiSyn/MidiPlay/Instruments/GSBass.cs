using AI;
using AI.DSPCore;

namespace MidiPlay.Instruments
{
    public class GSBass : GSyntPiano
    {

        

        public override void Create(int fd = 16000)
        {
            alpha = 0.2;
            Fd = fd;
            Magns = new double[] { 1,1 };
        }

        protected override Vector Postprocessing(Vector inp, Vector freq, Vector w)
        {
            Vector k = Filters.FilterKontur(AI.Statistics.Statistic.rand(inp.Count), 3, freq[0], Fd);
            k /= k.Max();
            return  inp*(1+k) * w;
        }
    }
}

