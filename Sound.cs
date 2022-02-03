using System.IO;
using System.Threading;
using ClausaComm.Utils;
using NAudio.Wave;

namespace ClausaComm;

public static class Sound
{
    public static void PlayNotificationSound()
    {
        // TODO: Reuse the thread!
        ThreadUtils.RunThread(() =>
        {
            using var audioFile =
                new AudioFileReader(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\notification_sound.mp3"));
            using var outputDevice = new WaveOutEvent();
            outputDevice.Init(audioFile);
            outputDevice.Play();
            while (outputDevice.PlaybackState == PlaybackState.Playing)
                Thread.Sleep(200); 
        });
    }
}