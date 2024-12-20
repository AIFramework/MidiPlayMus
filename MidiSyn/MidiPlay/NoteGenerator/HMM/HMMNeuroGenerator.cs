﻿using AI;
using AI.ML.NeuralNetwork.CoreNNW.Activations;
using System;
using Decoder = AI.ML.NeuralNetwork.Architectures.ClassifierNet.VectorPredictorStates;
using DecoderPositionCoder = AI.ML.DataEncoding.PositionalEncoding.MultiscaleEncoder;
using Encoder = AI.ML.NeuralNetwork.Architectures.Encoders.VectorWithPositionEncoder;
using EncoderPositionCoder = AI.ML.DataEncoding.PositionalEncoding.MultiscaleEncoder;
using KHMM = AI.ML.HMM.KMeanHMM;
using StateEncoder = AI.ML.DataEncoding.PositionalEncoding.OneHotPositionEncoder;

namespace Midi.NoteGenerator.HMM
{
    public class HMMNeuroGenerator
    {
        KHMM hmm;
        Encoder encoder;
        Decoder decoder;
        int n = 100;


        public HMMNeuroGenerator(int vDim, int statesCount)
        {
            hmm = new KHMM(statesCount);
            encoder = new Encoder(new EncoderPositionCoder(2), vDim, 32, new ReLU(0.3));
            decoder = new Decoder(new DecoderPositionCoder(2), new StateEncoder(statesCount), 16, vDim, 1, oActivation: new SigmoidUnit());
        }

        // генерация нот
        public Vector[] Generate(Vector note)
        {
            Vector latent = encoder.Forward(note, 0);
            var states = hmm.Generate(latent, n);
            Vector[] outp = new Vector[n];

            outp[0] = decoder.Predict(new int[] { states[0], 0 }, 0);

            for (int i = 1; i < n; i++)
            {
                outp[i] = decoder.Predict(new int[] { states[i], states[i - 1] }, i);
            }

            return outp;
        }

        // Обучение генератора
        public void Train(Vector[][] notes)
        {
            var vectInt = GetVectsPositions(notes);
            encoder.Train(vectInt.Item1, vectInt.Item2);
            hmm.Train(vectInt.Item1);
            int[] states = hmm.KMean.Classify(vectInt.Item1);
            decoder.Train(States(states, vectInt.Item2), vectInt.Item2, vectInt.Item1);
        }


        private Tuple<Vector[], int[]> GetVectsPositions(Vector[][] data)
        {
            int len = 0;
            for (int i = 0; i < data.Length; i++) len += data[i].Length;

            Vector[] outp = new Vector[len]; // признаки
            int[] outpInt = new int[len]; // позиции

            for (int i = 0, k = 0; i < data.Length; i++)
            {
                for (int j = 0; j < data[i].Length; j++)
                {
                    outp[k] = data[i][j];
                    outpInt[k] = j;
                    k++;
                }
            }

            return new Tuple<Vector[], int[]>(outp, outpInt);
        }

        private int[][] States(int[] st, int[] pos)
        {
            int[][] stOut = new int[st.Length][];

            for (int i = 0; i < st.Length; i++)
            {
                if (pos[i] == 0)
                    stOut[i] = new[] { st[i], 0 };

                else
                    stOut[i] = new[] { st[i], st[i - 1] };
            }

            return stOut;
        }
    }
}
