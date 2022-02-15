using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ClausaComm.Utils;
using NAudio.Wave;

namespace ClausaComm;

public static class Sound
{
    public static void PlayNotificationSound()
    {
        Task.Run(() =>
        {
            using var audioFile =
                new AudioFileReader(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\notification_sound.mp3"));
            audioFile.Volume = 0.8f;
            using var outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
                Thread.Sleep(190);
        });
    }
}