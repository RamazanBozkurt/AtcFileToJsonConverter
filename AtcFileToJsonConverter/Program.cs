using AtcFileToJsonConverter;

string fileName = "your_file_name";
string filePath = "your_file_path";
string targetPath = Path.Combine("your_target_path", fileName);

Process process = new Process();
process.Execute(filePath, targetPath);