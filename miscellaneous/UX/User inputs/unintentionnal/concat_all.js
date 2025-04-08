const fs = require("fs");
const path = require("path");

function containsSubstring(filename, substring) {
  return filename.toLowerCase().includes(substring.toLowerCase());
}

function findFilesWithSubstring(directoryPath, substring) {
  const files = fs.readdirSync(directoryPath);

  return files.flatMap((file) => {
    const filePath = path.join(directoryPath, file);
    const stats = fs.statSync(filePath);

    if (stats.isDirectory()) {
      return findFilesWithSubstring(filePath, substring);
    } else if (stats.isFile() && containsSubstring(file, substring)) {
      const array = JSON.parse(fs.readFileSync(filePath, "utf8"));
      return array ?? [];
    }
  }).filter(x => !!x);
}

function main() {
  const currentDirectory = process.cwd();
  //   const substring = "in.json";
  const args = process.argv.slice(2); // Get command-line arguments excluding node and script name
  const substring = args[0]; // The first argument is the substring to search for

  console.log(
    `Searching for files containing "${substring}" in the directory: ${currentDirectory}`
  );
  const all = findFilesWithSubstring(currentDirectory, substring);
  const outputFilePath = path.join(__dirname, `all-${substring}.json`); // Append ".output.json" to the original file name
  console.log(`Found ${all.length} entries`)
  fs.writeFileSync(outputFilePath, JSON.stringify(all));
  console.log(
    `Saved as "${outputFilePath}"`
  );
}

main();
