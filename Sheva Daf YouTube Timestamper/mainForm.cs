using System.Text.RegularExpressions;

namespace Sheva_Daf_YouTube_Timestamper
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            string input = txtTimestamps.Text;
            if (string.IsNullOrWhiteSpace(input)) return;
            List<string> lines = [.. input.Split(Environment.NewLine)];
            List<string> cleanerlines = SanitizeLines(ref lines);
            lines = RemoveSimilarTimestamps(lines);

            cleanerlines.Clear();
            foreach (string line in lines)
            {
                cleanerlines.Add(line + " ");
            }

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

        private static (string Midpoint, string ElapsedTime) FindMidpointOfLargestGap(List<string> lines)
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

            // Step 2: Find the largest gap
            TimeSpan maxGap = TimeSpan.Zero;
            TimeSpan midpoint = TimeSpan.Zero;

            for (int i = 1; i < timestamps.Count; i++)
            {
                TimeSpan gap = timestamps[i] - timestamps[i - 1];

                if (gap > maxGap)
                {
                    maxGap = gap;
                    midpoint = timestamps[i - 1] + TimeSpan.FromTicks(gap.Ticks / 2);
                }
            }

            // Step 3: Format midpoint as HH:MM:SS
            string midpointFormatted = $"{(int)midpoint.TotalHours:D2}:{midpoint.Minutes:D2}:{midpoint.Seconds:D2}";

            // Step 4: Format elapsed time (excluding zero values)
            List<string> elapsedParts = new List<string>();
            if (maxGap.Hours > 0) elapsedParts.Add($"{maxGap.Hours} hour{(maxGap.Hours > 1 ? "s" : "")}");
            if (maxGap.Minutes > 0) elapsedParts.Add($"{maxGap.Minutes} minute{(maxGap.Minutes > 1 ? "s" : "")}");
            if (maxGap.Seconds > 0) elapsedParts.Add($"{maxGap.Seconds} second{(maxGap.Seconds > 1 ? "s" : "")}");

            string elapsedTime = string.Join(", ", elapsedParts);

            return (midpointFormatted, elapsedTime);
        }

        private static List<string> RemoveSimilarTimestamps(List<string> lines)
        {
            // Step 1: Parse the string timestamps to TimeSpan
            List<TimeSpan> timestamps = lines
                .Select(ts => TimeSpan.Parse(ts))
                .ToList();

            // Step 2: Sort the timestamps in chronological order
            timestamps.Sort();

            // Step 3: Remove duplicates if the timestamps are within 0 to 3 seconds of each other
            List<TimeSpan> filteredTimestamps = [];

            for (int i = 0; i < timestamps.Count; i++)
            {
                // For the first timestamp or if the difference is more than 3 seconds, keep the timestamp
                if (i == 0 || (timestamps[i] - timestamps[i - 1]).TotalSeconds > 3)
                {
                    filteredTimestamps.Add(timestamps[i]);
                }
            }

            List<string> output = [];
            output.AddRange(from ts in filteredTimestamps
                            select ts.ToString());

            List<string> finalOutput = [];
            foreach (string time in output)
            {
                if (time.StartsWith("0") && !time.StartsWith("0:"))
                {
                    finalOutput.Add(time.Substring(1));
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
            (string midpoint, string elapsedTime) = FindMidpointOfLargestGap(lines);
            MessageBox.Show($"Timestamp {midpoint} has the largest gap of {elapsedTime}.", "Detected Gap", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnPrefill_Click(object sender, EventArgs e)
        {
            txtLastDaf.Text = "א דף א ע\"ב";
        }
    }
}
