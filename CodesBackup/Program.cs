using System.IO.Compression;

Log("Start Init......");

var backupPaths = "Backups";
var rootPaths = Environment.CurrentDirectory;
var cSuffixes = new[] { "c", "h"};
var dateTime = DateTime.Now.ToString("MMddHHmm");
List<string> GetCodeFilesList(string path,string[] suffixes)
{
    var directoryInfo = new DirectoryInfo(path);
    var codeFiles = new List<string>();
    foreach (var fileSystemInfo in directoryInfo.GetFileSystemInfos())
    {
        if (fileSystemInfo is not FileInfo file) continue;
        if (suffixes.Any(suffix => file.FullName.EndsWith($".{suffix}"))||file.Name=="CMakeLists.txt")
        {
            codeFiles.Add(file.Name);
        }
    }
    return codeFiles;
}

void Log(string info)
{
    var logDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
    Console.WriteLine($"[{logDateTime}] {info}");
}

Log("Init Finished.");

if (!Directory.Exists(backupPaths))
{
    Directory.CreateDirectory(backupPaths);
    Log("Creat Backup Directory.");
}
else
    Log("Located Backup Directory.");

Directory.CreateDirectory("cache");

Log("Ready to Start Backup");
var codeFilesList = GetCodeFilesList(rootPaths, cSuffixes);
Log("Locate Code Files.");
foreach (var codeFile in codeFilesList)
{
    File.Copy(Path.Combine(rootPaths,codeFile),Path.Combine(rootPaths,"cache",codeFile));
}
ZipFile.CreateFromDirectory(Path.Combine(rootPaths,"cache"),Path.Combine(rootPaths,backupPaths,dateTime+".zip"));
Log("Code Files Backup Finished.");
foreach (var codeFile in codeFilesList)
{
    File.Delete(Path.Combine(rootPaths,"cache",codeFile));
}
Directory.Delete(Path.Combine(rootPaths,"cache"));
Log("Clean Up Finished.");
Console.ReadLine();