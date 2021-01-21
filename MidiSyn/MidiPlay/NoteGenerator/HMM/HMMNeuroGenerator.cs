using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KHMM = AI.ML.HMM.KMeanHMM;
using Encoder = AI.ML.NeuralNetwork.Architectures.Encoders.VectorWithPositionEncoder;
using Decoder = AI.ML.NeuralNetwork.Architectures.ClassifierNet.VectorPredictorStates;
using EncoderPositionCoder = AI.ML.DataEncoding.PositionalEncoding.TrigonometricPositionalEncoder;
using DecoderPositionCoder = AI.ML.DataEncoding.PositionalEncoding.PositionEncoderOnDeductionRings;
using StateEncoder = AI.ML.DataEncoding.PositionalEncoding.OneHotPositionEncoder;
using AI.ML.NeuralNetwork.CoreNNW.Activations;

namespace Midi.NoteGenerator.HMM
{
    public class HMMNeuroGenerator
    {
        KHMM hmm;
        Encoder encoder;
        Decoder decoder;

        public HMMNeuroGenerator() 
        {
            hmm = new KHMM(9);
            encoder = new Encoder(new EncoderPositionCoder(32), 3, 10, new ReLU(0.3));
            decoder = new Decoder(new DecoderPositionCoder(16), new StateEncoder(9), 100, 3, 2);
        }
    }
}
