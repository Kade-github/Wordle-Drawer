string[] words = File.ReadAllLines("words.txt");

string[] grid = [".....", ".....", ".....", ".....", ".....", "....."];

int selectedX = 0;
int selectedY = 0;

string wordToGuess = "";

void DisplayTotal()
{
    var i = 0;
    foreach (var line in grid)
    {
        // Separate each character with a space
        char[] ar = line.ToCharArray();
        if (i == selectedY)
        {
            if (ar[selectedX] == '#')
                ar[selectedX] = '*';
            else if (ar[selectedX] == '.')
                ar[selectedX] = '_';
        }

        Console.WriteLine(string.Join(" ", ar));
        i++;
    }
}

Console.WriteLine("Loaded {0} words.", words.Length);

Console.WriteLine("Please enter the current wordle:");
wordToGuess = Console.ReadLine() ?? "";

while (true)
{
    Console.Clear();
    Console.WriteLine("Press WASD to move, Space to change, Q to quit.");
    Console.WriteLine($"Current wordle: {wordToGuess}");
    DisplayTotal();
    var key = Console.ReadKey(true).Key;
    if (key == ConsoleKey.Q)
        break;
    if (key == ConsoleKey.W && selectedY > 0)
        selectedY--;
    else if (key == ConsoleKey.S && selectedY < 5)
        selectedY++;
    else if (key == ConsoleKey.A && selectedX > 0)
        selectedX--;
    else if (key == ConsoleKey.D && selectedX < 4)
        selectedX++;
    else if (key == ConsoleKey.Spacebar) // Change to # or .
    {
        char[] lineChars = grid[selectedY].ToCharArray();
        lineChars[selectedX] = lineChars[selectedX] == '.' ? '#' : '.';
        grid[selectedY] = new string(lineChars);
    }
}

// Solve for a combination of words that have characters in the correct positions (#)

List<string> possibleWords = [];
List<string> wordsToUse = [];
for (int i = 0; i < 6; i++)
{
    possibleWords = new List<string>(words);
    // Get all positions that have #
    var positions = new List<int>();
    var antiPositions = new List<int>();
    for (int j = 0; j < 5; j++)
    {
        if (grid[i][j] == '#')
            positions.Add(j);
        else if (grid[i][j] == '.')
            antiPositions.Add(j);
    }
    
    possibleWords = possibleWords.Where(word =>
    {
        foreach (var pos in positions)
        {
            if (word[pos] != wordToGuess[pos])
                return false;
        }
        foreach (var pos in antiPositions)
        {
            if (word[pos] == wordToGuess[pos])
                return false;
        }
        return true;
    }).ToList();
    wordsToUse.Add(possibleWords.Count > 0 ? possibleWords[0] : "-----");
}

Console.WriteLine("Words to use:");
foreach (var word in wordsToUse)
{
    Console.WriteLine(word);
}
