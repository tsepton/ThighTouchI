const fs = require("fs");
const path = require("path");

function containsSubstring(filename, substring) {
  return filename.toLowerCase().includes(substring.toLowerCase());
}

function findFilesWithSubstring(directoryPath, substring) {
  const files = fs.readdirSync(directoryPath);

  return files
    .flatMap((file) => {
      const filePath = path.join(directoryPath, file);
      const stats = fs.statSync(filePath);

      if (stats.isDirectory()) {
        return findFilesWithSubstring(filePath, substring);
      } else if (stats.isFile() && containsSubstring(file, substring)) {
        const array = JSON.parse(fs.readFileSync(filePath, "utf8"));
        return array ?? [];
      }
    })
    .filter((x) => !!x);
}

function main() {
  const currentDirectory = process.cwd();
  const args = process.argv.slice(2);
  const substring = args[0];

  console.log(
    `Searching for files containing "${substring}" in the directory: ${currentDirectory}`
  );
  const all = findFilesWithSubstring(currentDirectory, substring);
  const outputFilePath = path.join(__dirname, `all-${substring}.json`);
  console.log(`Found ${all.length} entries`);
  fs.writeFileSync(outputFilePath, JSON.stringify(all));
  console.log(`Saved as "${outputFilePath}"`);
}

main();
