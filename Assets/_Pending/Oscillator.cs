using System;
using UnityEngine;

namespace Prototype.Pending
{
    public class Oscillator : MonoBehaviour
    {
        public enum Waveform { Sinus, Square, Triangle }
        public Waveform waveform = Waveform.Square;

        public double minFrequency = 13.75;
        public double maxFrequency = 110;
        [Range(0, 1)] public double gain = .15;
        [Range(-.5f, .5f)] public double channelSplit = .05;
        [Range(.1f, 20)] public float speed = 4;

        double frequency;

        readonly double[] increments = new double[2];
        readonly double[] phases = new double[2];

        double sampleRate;

        void Awake()
        {
            if (!TryGetComponent(out AudioSource _))
                gameObject.AddComponent<AudioSource>();

            sampleRate = AudioSettings.outputSampleRate;
        }

        void Update()
        {
            var t = Mathf.Abs(Time.time * speed % 2 - 1);
            frequency = minFrequency + (maxFrequency - minFrequency) * t;
        }

        void OnAudioFilterRead(float[] data, int channels)
        {
            var maxPhase = Math.PI * 2;

            if (channels == 2)
            {
                increments[0] = frequency * (1 - channelSplit) * maxPhase / sampleRate;
                increments[1] = frequency * (1 + channelSplit) * maxPhase / sampleRate;

                for (int i = 0; i < data.Length; i += channels)
                {
                    phases[0] += increments[0];
                    phases[1] += increments[1];

                    data[i] = (float) GetWaveform(phases[0]);
                    data[i + 1] = (float) GetWaveform(phases[1]);

                    if (phases[0] > maxPhase)
                        phases[0] = 0;

                    if (phases[1] > maxPhase)
                        phases[1] = 0;
                }
                return;
            }

            increments[0] = frequency * maxPhase / sampleRate;
            for (int i = 0; i < data.Length; i += channels)
            {
                phases[0] += increments[0];
                data[i] = (float) GetWaveform(phases[0]);
                if (phases[0] > maxPhase)
                    phases[0] = 0;
            }
        }

        double GetWaveform(double phase) => waveform switch
        {
            Waveform.Sinus => GetWaveform_Sinus(phase, gain),
            Waveform.Square => GetWaveform_Square(phase, gain),
            Waveform.Triangle => GetWaveform_Triangle(phase, gain),
            _ => 0,
        };

        double GetWaveform_Sinus(double phase, double gain)
        {
            return Math.Sin(phase) * gain;
        }

        double GetWaveform_Square(double phase, double gain)
        {
            var sin = Math.Sin(phase);
            if (sin < 0) gain *= -1;
            return gain * .6;
        }

        double GetWaveform_Triangle(double phase, double gain)
        {
            return Mathf.PingPong((float) phase, 1) * gain;
        }
    }
}