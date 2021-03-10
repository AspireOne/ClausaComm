using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ClausaComm.Extensions;
using ClausaComm.Utils;

namespace ClausaComm
{
    public static class UpdateManager
    {
        private const string NewVerTagUrl = "https://api.github.com/repos/AspireOne/ClausaComm/tags";
        private const string BinaryUrl = "https://github.com/AspireOne/ClausaComm/releases/latest/download/binaries.zip";
        private static readonly string BinarySavePath = Path.Combine(Path.GetTempPath(), "ClausaComm_update_binaries.zip");

        private static readonly string ExtractedFilesTempDir = Path.Combine(Path.GetTempPath(), "ClausaComm_update_binaries_extracted");
        private static readonly HttpClient Client = new() { DefaultRequestHeaders = { UserAgent = { ProductInfoHeaderValue.Parse("Other") } } };
        private static readonly Regex VersionRegex = new(@"(?<=""v)\d\.\d\.\d(?="")", RegexOptions.ECMAScript);
        public static bool UpdateDownloaded { get; private set; }

        public static async Task<(bool available, string newVer)> IsNewVersionAvailable()
        {
            string[] availableVersions = await FetchAvailableVersions();

            Debug.WriteLine("Available versions: ");
            Array.ForEach(availableVersions, x => Debug.WriteLine(x));

            string highestVer = GetHighestVersion(availableVersions);
            Debug.WriteLine("\nhighest version: " + highestVer);

            bool highestIsHigherThanCurr = highestVer.IsHigherThan(Program.Version);
            Debug.WriteLine("\nHighest available is higher than current: " + highestIsHigherThanCurr);

            return (highestIsHigherThanCurr, highestIsHigherThanCurr ? highestVer : null);
        }

        public static void DownloadNewVersionBinaryAsync(DownloadProgressChangedEventHandler progressHandler = null,
            AsyncCompletedEventHandler completedHandler = null, Action errorHandler = null)
        {
            using WebClient wc = new();
            wc.DownloadFileCompleted += (_, _) => UpdateDownloaded = true;

            if (progressHandler is not null)
                wc.DownloadProgressChanged += progressHandler;
            if (completedHandler is not null)
            {
                wc.DownloadFileCompleted += completedHandler;
            }

            try
            {
                wc.DownloadFileAsync(new Uri(BinaryUrl), BinarySavePath);
            }
            catch (WebException e)
            {
                Debug.WriteLine(e);
                errorHandler?.Invoke();
            }
        }

        public static void PrepareUpdateAndStartTimer(bool restart)
        {
            Directory.CreateDirectory(ExtractedFilesTempDir);
            ZipFile.ExtractToDirectory(BinarySavePath, ExtractedFilesTempDir, true);
            File.Delete(BinarySavePath);

            string restartCommand = $" & start {Program.ThisProgramPath}";

            /*
            using RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\ClausaComm");
            string installPath = key?.GetValue("InstallLocation")?.ToString()?.Replace("\"", "");
            */
            new Process
            {
                StartInfo = ConsoleUtils.GetProcessStartInfo(
                    $"{ConsoleUtils.GetDelay(1)}" +
                        $" & taskkill /f /im {Environment.ProcessId}" + // Even tho the process should be terminated before this command runs, we'll make sure.
                        $" & rmdir /s /q \"{Directory.GetCurrentDirectory()}\"" +
                        $" & move /y \"{ExtractedFilesTempDir}\" \"{Directory.GetCurrentDirectory()}\"" +
                        $"{(restart ? restartCommand : "")}",
                    true, true)
            }.Start();
        }

        private static async Task<string[]> FetchAvailableVersions()
        {
            using HttpResponseMessage response = await Client.GetAsync(NewVerTagUrl);
            using HttpContent content = response.Content;
            string result = await content.ReadAsStringAsync();

            return VersionRegex.Matches(result).Select(match => match.Value).ToArray();
        }

        private static string GetHighestVersion(IReadOnlyList<string> versions)
        {
            string lastHighest = versions[0];
            versions.ForEach(ver => lastHighest = ver.IsHigherThan(lastHighest) ? ver : lastHighest);
            return lastHighest;
        }

        private static bool IsHigherThan(this string version, string secondVersion)
        {
            if (version == secondVersion)
                return false;

            byte[] verNums = version.Split('.').Select(byte.Parse).ToArray();
            byte[] secondVerNums = secondVersion.Split('.').Select(byte.Parse).ToArray();

            return verNums[0] > secondVerNums[0]
                   || verNums[0] == secondVerNums[0] && verNums[1] > secondVerNums[1]
                   || verNums[0] == secondVerNums[0] && verNums[1] == secondVerNums[1] && verNums[2] > secondVerNums[2];
        }
    }
}