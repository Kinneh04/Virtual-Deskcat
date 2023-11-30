using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using HuggingFace;
using HuggingFace.API;

public class SpeechRecgonition : MonoBehaviour
{
    [Header("Buttons")]
    public Button SpeakButton;

    public bool isSpeaking;

    public TMP_Text SpeakResultText;


    [Header("Speech")]
    private AudioClip clip;
    private byte[] bytes;
    public void PushSpeakButton()
    {
        if(!isSpeaking)
        {
            StartRecording();
            SpeakResultText.text = "Listening...";
        }
        else
        {
         
            StopRecording();
            SpeakResultText.text = "Processing...";
        }
    }

    private void StartRecording()
    {
        clip = Microphone.Start(null, false, 10, 44100);
        isSpeaking = true;
    }

    private void Update()
    {
        if(isSpeaking && Microphone.GetPosition(null) >= clip.samples)
        {
            StopRecording();
        }
    }

    private void StopRecording()
    {
        var position = Microphone.GetPosition(null);
        Microphone.End(null);
        var samples = new float[position * clip.channels];
        clip.GetData(samples, 0);
        bytes = EncodeAsWAV(samples, clip.frequency, clip.channels);
        isSpeaking = false;
        ProcessRecording();
    }
    private byte[] EncodeAsWAV(float[] samples, int frequency, int channels)
    {
        using (var memoryStream = new MemoryStream(44 + samples.Length * 2))
        {
            using (var writer = new BinaryWriter(memoryStream))
            {
                writer.Write("RIFF".ToCharArray());
                writer.Write(36 + samples.Length * 2);
                writer.Write("WAVE".ToCharArray());
                writer.Write("fmt ".ToCharArray());
                writer.Write(16);
                writer.Write((ushort)1);
                writer.Write((ushort)channels);
                writer.Write(frequency);
                writer.Write(frequency * channels * 2);
                writer.Write((ushort)(channels * 2));
                writer.Write((ushort)16);
                writer.Write("data".ToCharArray());
                writer.Write(samples.Length * 2);

                foreach (var sample in samples)
                {
                    writer.Write((short)(sample * short.MaxValue));
                }
            }
            return memoryStream.ToArray();
        }
    }

    public void ProcessRecording()
    {
        HuggingFaceAPI.AutomaticSpeechRecognition(bytes, response =>
        {
            SpeakResultText.text = response;
        }, error =>
        {
            SpeakResultText.text = "error: " + error;
        });
    }
}
