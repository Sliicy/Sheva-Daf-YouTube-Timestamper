using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Sheva_Daf_YouTube_Timestamper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // Helper method to normalize timestamp strings to a consistent format (H:MM:SS)
        private static string NormalizeTimestampString(string ts)
        {
            if (TimeSpan.TryParse(ts, out TimeSpan timeSpan))
            {
                // Format as H:MM:SS, e.g., "5:11:07" or "15:11:07"
                return $"{timeSpan.Hours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
            return ts; // Fallback, though regex should ensure parseable timestamps
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            string input = txtTimestamps.Text;
            if (string.IsNullOrWhiteSpace(input)) return;

            List<string> rawLines = [.. input.Split(Environment.NewLine)];
            List<string> regularLines = new List<string>();
            List<string> chiddushTimestamps = new List<string>();

            // Logic to separate Chiddushim from Regular timestamps
            Regex tsRegex = new Regex(@"(\d{1,2}:\d{2}:\d{2})");
            bool nextIsChiddush = false;

            foreach (string line in rawLines)
            {
                // Detect context
                if (line.Contains("HOTKEY:Chiddush"))
                {
                    nextIsChiddush = true;
                    continue; // Don't add the Hotkey label to either list
                }
                else if (line.Contains("HOTKEY:Timestamp"))
                {
                    nextIsChiddush = false;
                    continue;
                }

                // Check if line has a timestamp
                Match match = tsRegex.Match(line);
                if (match.Success)
                {
                    string cleanTs = NormalizeTimestampString(match.Value);

                    if (nextIsChiddush)
                    {
                        chiddushTimestamps.Add(cleanTs);
                        nextIsChiddush = false; // Reset after consuming
                                                // Do not add to regularLines
                    }
                    else
                    {
                        regularLines.Add(line);
                    }
                }
                else
                {
                    // Non-timestamp lines (e.g., EVENT:STOP) go to regular processing
                    // unless they are specifically part of the Chiddush block structure, 
                    // but usually we want to keep structure for existing logic.
                    regularLines.Add(line);
                }
            }

            // Process Chiddushim into pairs
            List<string> chiddushPairs = new List<string>();
            for (int i = 0; i < chiddushTimestamps.Count; i += 2)
            {
                string start = chiddushTimestamps[i];
                string end = (i + 1 < chiddushTimestamps.Count) ? chiddushTimestamps[i + 1] : "???";
                chiddushPairs.Add($"{start} - {end}");
            }

            if (chiddushPairs.Count > 0)
            {
                txtChiddushim.Text = string.Join(Environment.NewLine, chiddushPairs);
                if (chiddushTimestamps.Count % 2 != 0)
                {
                    MessageBox.Show("Odd number of Chiddush timestamps detected. Please check the last entry in the bottom box.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // --- Proceed with existing logic using 'regularLines' instead of original input ---

            HashSet<string> timestampsToFilterOut = new HashSet<string>();
            Regex timestampRegex = new Regex(@"(\d{1,2}:\d{2}:\d{2})");

            foreach (string originalLineText in regularLines)
            {
                if (originalLineText.Contains("recording is paused"))
                {
                    Match match = timestampRegex.Match(originalLineText);
                    if (match.Success)
                    {
                        timestampsToFilterOut.Add(NormalizeTimestampString(match.Groups[1].Value));
                    }
                }
            }

            List<string> cleanerlines = SanitizeLines(ref regularLines);

            if (timestampsToFilterOut.Count > 0)
            {
                regularLines = regularLines.Where(tsLine => !timestampsToFilterOut.Contains(NormalizeTimestampString(tsLine))).ToList();
            }

            regularLines = RemoveSimilarTimestamps(regularLines);

            cleanerlines.Clear();
            foreach (string line in regularLines)
            {
                cleanerlines.Add(line + " ");
            }

            if (cleanerlines.Count > 0)
            {
                cleanerlines[0] = cleanerlines[0] + "הקדמה";
                txtLastDaf.Text = txtLastDaf.Text.Trim();
                string referencedDaf = txtLastDaf.Text;

                List<string> output = [cleanerlines[0]];

                foreach (string line in cleanerlines.Skip(1))
                {
                    string currentAmud = "";
                    if (referencedDaf.Contains("ע\"א"))
                    {
                        currentAmud = "ע\"ב";
                        referencedDaf = referencedDaf.Replace("ע\"א", currentAmud);
                    }
                    else
                    {
                        currentAmud = "ע\"א";
                        referencedDaf = referencedDaf.Replace("ע\"ב", currentAmud);
                        string currentDafLetter = referencedDaf.Split("דף ")[1].Split(" " + currentAmud)[0];
                        referencedDaf = referencedDaf.Replace(
                            "דף " + currentDafLetter + " " + currentAmud,
                            "דף " + IncrementHebrewWord(currentDafLetter) + " " + currentAmud
                            );
                    }
                    output.Add(line + referencedDaf);
                }
                txtTimestamps.Text = string.Join(Environment.NewLine, output);
            }
        }

        private void BtnExtractChiddushim_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtChiddushim.Text))
            {
                MessageBox.Show("No Chiddush timestamps found.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- CONFIGURATION ---
            // CHANGE THIS if ffmpeg is not in your global System PATH.
            // Example: string ffmpegPath = @"C:\ffmpeg\bin\ffmpeg.exe";
            string ffmpegPath = "ffmpeg";
            // ---------------------

            // Check if video exists
            string videoPath = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "*.mkv").FirstOrDefault();
            if (videoPath == null)
            {
                MessageBox.Show("No .mkv file found in My Videos folder.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string outputDir = Path.GetDirectoryName(videoPath);
            string baseFileName = Path.GetFileNameWithoutExtension(videoPath);
            string[] lines = txtChiddushim.Lines;
            int count = 1;
            List<string> errors = new List<string>();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split('-');
                if (parts.Length != 2) continue;

                string start = parts[0].Trim();
                string end = parts[1].Trim();

                if (end == "???" || string.IsNullOrEmpty(end)) continue;

                string outputFile = Path.Combine(outputDir, $"{baseFileName}_Chiddush_{count}.mp3");

                // FFMPEG Command Explanation:
                // -y : Overwrite output without asking
                // -i : Input file
                // -ss : Start time (placed AFTER -i here for precision cutting, though slower than placing before)
                // -to : End time
                // -vn : No video (audio only)
                // -acodec libmp3lame : Encode to MP3
                // -q:a 2 : High quality VBR audio

                string arguments = $"-y -i \"{videoPath}\" -ss {start} -to {end} -vn -acodec libmp3lame -q:a 2 \"{outputFile}\"";

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardError = true, // Capture errors
                    RedirectStandardOutput = true
                };

                try
                {
                    using (Process p = Process.Start(psi))
                    {
                        // FFmpeg writes progress/errors to StandardError, not StandardOutput
                        string errorOutput = p.StandardError.ReadToEnd();
                        p.WaitForExit();

                        if (p.ExitCode != 0)
                        {
                            errors.Add($"Snippet {count} Failed:\n{errorOutput}");
                        }
                    }
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    MessageBox.Show($"Could not find FFmpeg at: '{ffmpegPath}'.\n\nPlease ensure FFmpeg is installed and the path in the code is correct.",
                        "FFmpeg Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                catch (Exception ex)
                {
                    errors.Add($"Snippet {count} Exception: {ex.Message}");
                }

                count++;
            }

            if (errors.Count > 0)
            {
                // Show the first error log to help debug
                MessageBox.Show($"Completed with errors.\n\nFirst Error Detail:\n{errors[0]}", "FFmpeg Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show($"Successfully created {count - 1} audio snippets in:\n{outputDir}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private static List<string> SanitizeLines(ref List<string> lines)
        {
            List<string> cleanerlines = [];
            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)
                    || line.Contains("EVENT:START")
                    || line.Contains("EVENT:STOP")
                    || line.Contains("EVENT:RECORDING")
                    || line.Contains("(not recording)")
                    || line.Contains(DateTime.Now.ToString("yyyy-MM-")))
                {
                    continue;
                }
                cleanerlines.Add(line);
            }
            string cleanedInput = Regex.Replace(string.Join(Environment.NewLine, cleanerlines), @"[^0-9:\n\r]", "");
            lines = [.. cleanedInput.Split(Environment.NewLine)];
            return cleanerlines;
        }

        private static List<(string Midpoint, string ElapsedTime)> FindMidpointsOfLargestGaps(List<string> lines, int numberOfGaps = 5)
        {
            // Step 1: Parse timestamps to TimeSpan and sort them
            List<TimeSpan> timestamps = lines
                .Select(ts => TimeSpan.Parse(ts))
                .OrderBy(ts => ts)
                .ToList();

            if (timestamps.Count < 2)
            {
                throw new ArgumentException("At least two timestamps are required.");
            }

            // Step 2: Compute all gaps with their midpoints
            var gaps = new List<(TimeSpan Gap, TimeSpan Midpoint)>();

            for (int i = 1; i < timestamps.Count; i++)
            {
                TimeSpan gap = timestamps[i] - timestamps[i - 1];
                TimeSpan midpoint = timestamps[i - 1] + TimeSpan.FromTicks(gap.Ticks / 2);
                gaps.Add((gap, midpoint));
            }

            // Step 3: Sort by gap descending and take the top `numberOfGaps`
            var topGaps = gaps
                .OrderByDescending(g => g.Gap)
                .Take(numberOfGaps)
                .ToList();

            // Step 4: Format the results
            var results = new List<(string Midpoint, string ElapsedTime)>();
            foreach (var (gap, midpoint) in topGaps)
            {
                string midpointFormatted = $"{(int)midpoint.TotalHours:D2}:{midpoint.Minutes:D2}:{midpoint.Seconds:D2}";

                List<string> elapsedParts = new List<string>();
                if (gap.Hours > 0) elapsedParts.Add($"{gap.Hours} hour{(gap.Hours > 1 ? "s" : "")}");
                if (gap.Minutes > 0) elapsedParts.Add($"{gap.Minutes} minute{(gap.Minutes > 1 ? "s" : "")}");
                if (gap.Seconds > 0) elapsedParts.Add($"{gap.Seconds} second{(gap.Seconds > 1 ? "s" : "")}");

                string elapsedTime = string.Join(", ", elapsedParts);
                results.Add((midpointFormatted, elapsedTime));
            }

            return results;
        }

        private static List<string> RemoveSimilarTimestamps(List<string> lines)
        {
            // Step 1: Parse the string timestamps to TimeSpan
            List<TimeSpan> timestamps = [.. lines.Select(ts => TimeSpan.Parse(ts))];

            // Step 2: Sort the timestamps in chronological order
            timestamps.Sort();

            // Step 3: Reverse iterate and keep the last timestamp in any 60-second cluster
            List<TimeSpan> filteredTimestamps = [];

            for (int i = timestamps.Count - 1; i >= 0; i--)
            {
                if (filteredTimestamps.Count == 0 || (filteredTimestamps[0] - timestamps[i]).TotalSeconds > 60)
                {
                    // Insert at beginning to preserve chronological order
                    filteredTimestamps.Insert(0, timestamps[i]);
                }
            }

            List<string> output = [.. filteredTimestamps.Select(ts => ts.ToString())];

            List<string> finalOutput = [];
            foreach (string time in output)
            {
                if (time.StartsWith('0') && !time.StartsWith("0:"))
                {
                    finalOutput.Add(time[1..]);
                }
                else
                {
                    finalOutput.Add(time);
                }
            }
            return finalOutput;
        }


        public static string IncrementHebrewWord(string word)
        {
            var hebrewLetters = new Dictionary<string, string>
    {
        { "א", "ב" }, { "ב", "ג" }, { "ג", "ד" }, { "ד", "ה" }, { "ה", "ו" },
        { "ו", "ז" }, { "ז", "ח" }, { "ח", "ט" }, { "ט", "י" }, { "י", "יא" },
        { "יא", "יב" }, { "יב", "יג" }, { "יג", "יד" }, { "יד", "טו" }, { "טו", "טז" },
        { "טז", "יז" }, { "יז", "יח" }, { "יח", "יט" }, { "יט", "כ" }, { "כ", "כא" },
        { "כא", "כב" }, { "כב", "כג" }, { "כג", "כד" }, { "כד", "כה" }, { "כה", "כו" },
        { "כו", "כז" }, { "כז", "כח" }, { "כח", "כט" }, { "כט", "ל" }, { "ל", "לא" }, { "לא", "לב" },
        { "לב", "לג" }, { "לג", "לד" }, { "לד", "לה" }, { "לה", "לו" }, { "לו", "לז" },
        { "לז", "לח" }, { "לח", "לט" }, { "לט", "מ" }, { "מ", "מא" }, { "מא", "מב" },
        { "מב", "מג" }, { "מג", "מד" }, { "מד", "מה" }, { "מה", "מו" }, { "מו", "מז" },
        { "מז", "מח" }, { "מח", "מט" }, { "מט", "נ" }, { "נ", "נא" }, { "נא", "נב" },
        { "נב", "נג" }, { "נג", "נד" }, { "נד", "נה" }, { "נה", "נו" }, { "נו", "נז" },
        { "נז", "נח" }, { "נח", "נט" }, { "נט", "ס" }, { "ס", "סא" }, { "סא", "סב" },
        { "סב", "סג" }, { "סג", "סד" }, { "סד", "סה" }, { "סה", "סו" }, { "סו", "סז" },
        { "סז", "סח" }, { "סח", "סט" }, { "סט", "ע" }, { "ע", "עא" }, { "עא", "עב" },
        { "עב", "עג" }, { "עג", "עד" }, { "עד", "עה" }, { "עה", "עו" }, { "עו", "עז" },
        { "עז", "עח" }, { "עח", "עט" }, { "עט", "פ" }, { "פ", "פא" }, { "פא", "פב" },
        { "פב", "פג" }, { "פג", "פד" }, { "פד", "פה" }, { "פה", "פו" }, { "פו", "פז" },
        { "פז", "פח" }, { "פח", "פט" }, { "פט", "צ" }, { "צ", "צא" }, { "צא", "צב" },
        { "צב", "צג" }, { "צג", "צד" }, { "צד", "צה" }, { "צה", "צו" }, { "צו", "צז" },
        { "צז", "צח" }, { "צח", "צט" }, { "צט", "ק" }, { "ק", "קא" }, { "קא", "קב" },
        { "קב", "קג" }, { "קג", "קד" }, { "קד", "קה" }, { "קה", "קו" }, { "קו", "קז" },
        { "קז", "קח" }, { "קח", "קט" }, { "קט", "קי" }, { "קי", "קיא" }, { "קיא", "קיב" },
        { "קיב", "קיג" }, { "קיג", "קיד" }, { "קיד", "קטו" }, { "קטו", "קטז" }, { "קטז", "קיז" },
        { "קיז", "קיח" }, { "קיח", "קיט" }, { "קיט", "קכ" }, { "קכ", "קכא" }, { "קכא", "קכב" },
        { "קכב", "קכג" }, { "קכג", "קכד" }, { "קכד", "קכה" }, { "קכה", "קכו" }, { "קכו", "קכז" },
        { "קכז", "קכח" }, { "קכח", "קכט" }, { "קכט", "קל" }, { "קל", "קלא" }, { "קלא", "קלב" },
        { "קלב", "קלג" }, { "קלג", "קלד" }, { "קלד", "קלה" }, { "קלה", "קלו" }, { "קלו", "קלז" },
        { "קלז", "קלח" }, { "קלח", "קלט" }, { "קלט", "קמ" }, { "קמ", "קמא" }, { "קמא", "קמב" },
        { "קמב", "קמג" }, { "קמג", "קמד" }, { "קמד", "קמה" }, { "קמה", "קמו" }, { "קמו", "קמז" },
        { "קמז", "קמח" }, { "קמח", "קמט" }, { "קמט", "קנ" }, { "קנ", "קנא" }, { "קנא", "קנב" },
        { "קנב", "קנג" }, { "קנג", "קנד" }, { "קנד", "קנה" }, { "קנה", "קנו" }, { "קנו", "קנז" },
        { "קנז", "קנח" }, { "קנח", "קנט" }, { "קנט", "קס" }, { "קס", "קסא" }, { "קסא", "קסב" },
        { "קסב", "קסג" }, { "קסג", "קסד" }, { "קסד", "קסה" }, { "קסה", "קסו" }, { "קסו", "קסז" },
        { "קסז", "קסח" }, { "קסח", "קסט" }, { "קסט", "קע" }, { "קע", "קעא" }, { "קעא", "קעב" },
        { "קעב", "קעג" }, { "קעג", "קעד" }, { "קעד", "קעה" }, { "קעה", "קעו" }
    };

            return hebrewLetters.ContainsKey(word) ? hebrewLetters[word] : "א";
        }

        private void BtnClearTimes_Click(object sender, EventArgs e)
        {
            txtTimestamps.Text = txtTimestamps.Text
                .Replace("1", "0")
                .Replace("2", "0")
                .Replace("3", "0")
                .Replace("4", "0")
                .Replace("5", "0")
                .Replace("6", "0")
                .Replace("7", "0")
                .Replace("8", "0")
                .Replace("9", "0");
        }

        private void BtnFindMissingTimes_Click(object sender, EventArgs e)
        {
            string input = txtTimestamps.Text;
            if (string.IsNullOrWhiteSpace(input)) return;
            List<string> lines = [.. input.Split(Environment.NewLine)];
            List<string> cleanerlines = SanitizeLines(ref lines);
            lines = RemoveSimilarTimestamps(lines);
            List<(string midpoint, string elapsedTime)> gaps = FindMidpointsOfLargestGaps(lines);
            string finalMessage = "";
            foreach (var gap in gaps)
            {
                finalMessage += $"{gap.midpoint} has a gap of {gap.elapsedTime}." + Environment.NewLine;
            }
            //(string midpoint, string elapsedTime) = FindMidpointOfLargestGap(lines);
            MessageBox.Show(finalMessage, "Detected Gap", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnPrefill_Click(object sender, EventArgs e)
        {
            txtLastDaf.Text = "א דף א ע\"ב";
        }

        private void TxtLastDaf_TextChanged(object sender, EventArgs e)
        {
            UserSettings.Default.LastDaf = txtLastDaf.Text;
            UserSettings.Default.Save();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            txtLastDaf.Text = UserSettings.Default.LastDaf;
        }

        private void BtnGoToEnd_Click(object sender, EventArgs e)
        {
            string buttonText = btnGoToEnd.Text;

            // Extract the timestamp from the button text (e.g., "G&o To 2:16:32")
            Match match = Regex.Match(buttonText, @"\b\d{1,2}:\d{2}:\d{2}\b");
            if (!match.Success)
            {
                MessageBox.Show("No valid timestamp found in the button text.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string timestamp = match.Value;

            if (!TimeSpan.TryParse(timestamp, out TimeSpan timeSpan))
            {
                MessageBox.Show("Invalid timestamp format.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Subtract 3 seconds for best viewing context, but ensure it doesn't go below zero
            TimeSpan seekTime = timeSpan.TotalSeconds > 3
                ? timeSpan - TimeSpan.FromSeconds(3)
                : TimeSpan.Zero;

            double seekMinutes = seekTime.TotalMinutes;

            string videoPath = Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyVideos), "*.mkv").FirstOrDefault();
            if (videoPath == null)
            {
                MessageBox.Show("No .mkv file found in My Videos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LaunchVlcPausedAtTime(
                @"C:\Program Files\VideoLAN\VLC\vlc.exe",
                videoPath,
                seekMinutes,
                10
            );
        }

        static void LaunchVlcPausedAtTime(string vlcPath, string videoPath, double seekMinutes, int killAfterXSeconds = 0)
        {
            if (string.IsNullOrWhiteSpace(vlcPath))
                throw new ArgumentException("VLC path is required.", nameof(vlcPath));
            if (string.IsNullOrWhiteSpace(videoPath))
                throw new ArgumentException("Video path is required.", nameof(videoPath));
            if (seekMinutes < 0)
                throw new ArgumentOutOfRangeException(nameof(seekMinutes), "Seek time must be non-negative.");

            int seekSeconds = (int)(seekMinutes * 60);

            var startInfo = new ProcessStartInfo
            {
                FileName = vlcPath,
                Arguments = $"\"{videoPath}\" --start-time={seekSeconds} --play-and-pause",
                UseShellExecute = false
            };

            Process vlcProcess = Process.Start(startInfo);

            if (killAfterXSeconds > 0)
            {
                Task.Run(async () =>
                {
                    await Task.Delay(killAfterXSeconds * 1000);

                    // Kill all VLC processes
                    foreach (var proc in Process.GetProcessesByName("vlc"))
                    {
                        try
                        {
                            proc.Kill();
                        }
                        catch (Exception ex)
                        {
                            // Optional: log or handle exceptions from killing processes
                            Console.WriteLine($"Failed to kill process {proc.Id}: {ex.Message}");
                        }
                    }
                });
            }
        }

        private static double GetLastTimestampInMinutes(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return 0;

            string[] lines = input
                .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Select(line => Regex.Match(line, @"\d{1,2}:\d{2}(?::\d{2})?").Value) // extract time
                .Where(time => !string.IsNullOrWhiteSpace(time))
                .ToArray();

            if (lines.Length == 0)
                return 0;

            string lastTime = lines.Last();

            // Normalize format to hh:mm:ss
            if (lastTime.Count(c => c == ':') == 1)
                lastTime = "00:" + lastTime; // e.g. "12:34" → "00:12:34"

            if (!TimeSpan.TryParse(lastTime, out TimeSpan ts))
                return 0;

            return ts.TotalMinutes;
        }

        private void UpdateGoToButton()
        {
            int caretIndex = txtTimestamps.SelectionStart;
            string[] lines = txtTimestamps.Lines;

            int runningLength = 0;
            foreach (string line in lines)
            {
                int lineLengthWithNewline = line.Length + Environment.NewLine.Length;

                if (caretIndex >= runningLength && caretIndex < runningLength + lineLengthWithNewline)
                {
                    // Try to find a timestamp anywhere in the line
                    Match match = Regex.Match(line, @"\b\d{1,2}:\d{2}:\d{2}\b");
                    if (match.Success)
                    {
                        string normalized = NormalizeTimestampString(match.Value);
                        btnGoToEnd.Text = $"G&o To {normalized}";
                    }
                    return;
                }

                runningLength += lineLengthWithNewline;
            }
        }

        private void TxtTimestamps_Click(object sender, EventArgs e)
        {
            UpdateGoToButton();
        }

        private void TxtTimestamps_KeyDown(object sender, KeyEventArgs e)
        {
            UpdateGoToButton();
        }

        private void TxtTimestamps_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateGoToButton();
        }

        private void BtnUseNextSet_Click(object sender, EventArgs e)
        {
            // Get all lines from the timestamps box
            string[] lines = txtTimestamps.Lines;

            // Find the last daf from the bottom, looking for: "דף <Daf> ע"<letter>"
            string lastDaf = null;
            for (int i = lines.Length - 1; i >= 0; i--)
            {
                string line = lines[i];
                if (line.Contains("דף "))
                {
                    Match dafMatch = Regex.Match(line, @"דף\s+(.+?)\s+ע""[אב]");
                    if (dafMatch.Success)
                    {
                        lastDaf = dafMatch.Groups[1].Value.Trim();
                        break;
                    }
                }
            }

            if (lastDaf == null)
            {
                return;
            }

            // Replace the daf in txtLastDaf, preserving the rest of the text (side like ע"א or ע"ב)
            string currentText = txtLastDaf.Text;
            string updatedText = Regex.Replace(currentText, @"(דף\s+)(.+?)(\s+ע""[אב])", $"$1{lastDaf}$3");

            txtLastDaf.Text = updatedText;
            txtTimestamps.Clear();
        }
    }
}
