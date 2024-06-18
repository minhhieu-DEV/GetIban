using System.IO;
using System.Diagnostics;
using System.Text.RegularExpressions;

bool SaveData(string nameFile, string data)
{
    try
    {
        string folder = "Data";
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
        string path = Path.Combine(folder, nameFile);
        if (File.Exists(path))
        {
            using (StreamWriter writer = File.AppendText(path))
            {
                writer.WriteLine(data);
                return true;
            }
        }
        else
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine(data);
                return true;
            }
        }
    }
    catch (Exception ex)
    {
        return false;
    }

}
int IndexFile(string nameFile)
{
    List<int> list = new List<int>();
    string folder = "Data";
    if (!Directory.Exists(folder))
    {
        Directory.CreateDirectory(folder);
    }
    string path = Path.Combine(folder);
    string[] files = Directory.GetFiles(path);

    // Output each file name
    foreach (string file in files)
    {
        var name = Path.GetFileName(file);
        if (name.Contains(nameFile))
        {
            Match match = Regex.Match(name, "\\((\\d+)\\)");
            if (match.Success)
            {
                string numberString = match.Groups[1].Value;
                int number = int.Parse(numberString);
                list.Add(number);
            }
        }
    }
    if (list.Count == 0)
    {
        return 1;
    }
    return list.Max() + 1;
}
int index = IndexFile(DateTime.UtcNow.ToString("yyyy-MM-dd"));
string nameFile = $"{DateTime.UtcNow:yyyy-MM-dd}({index}).txt";
try{
    if(Directory.GetFiles("node_modules").Length == 0){
        Process process = Process.Start("npm i");
        process.WaitForExit();
    }
}catch(Exception error){
    Console.WriteLine($"Error: {error}"); 
}
while(true){
    ProcessStartInfo startInfo = new ProcessStartInfo
    {
        FileName = "node",  // Path to your Node.js executable
        Arguments = "index.js Brazil", 
        UseShellExecute = false,
        RedirectStandardOutput = true, // Capture output (optional)
        RedirectStandardError = true   // Capture errors (optional)
    };
    using (Process process = Process.Start(startInfo))
    {
        process.WaitForExit();

        if (process.ExitCode == 0)
        {
            string output = process.StandardOutput.ReadToEnd();
            if(SaveData(nameFile, output)){
                Console.WriteLine(output.Trim()); 
            }            
        }
        else
        {
            string error = process.StandardError.ReadToEnd();
            Console.WriteLine($"Error: {error}"); 
        }
    }
    
}



