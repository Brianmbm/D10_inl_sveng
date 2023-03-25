namespace MJU23v_D10_inl_sveng
{
    internal class Program
    {
        static List<SweEngGloss> dictionary;
        class SweEngGloss
        {
            public string word_swe, word_eng;
            public SweEngGloss(string word_swe, string word_eng)
            {
                this.word_swe = word_swe; this.word_eng = word_eng;
            }
            public SweEngGloss(string line)
            {
                string[] words = line.Split('|');
                this.word_swe = words[0]; this.word_eng = words[1];
            }
        }
        static void Main(string[] args)
        {
            string defaultFile = "..\\..\\..\\dict\\sweeng.lis";
            Console.WriteLine("Welcome to the dictionary app!");
            Help();
            do
            {
                Console.Write("> ");
                string[] argument = Console.ReadLine().Split();
                string command = argument[0];
                if (command == "quit")
                {
                    Console.WriteLine("Goodbye!");
                }
                else if (command == "load")
                {
                    if (argument.Length == 2)
                    {
                        using (StreamReader sr = new StreamReader(argument[1]))
                        {
                            Load(sr);
                        }
                    }
                    else if (argument.Length == 1)
                    {
                        using (StreamReader sr = new StreamReader(defaultFile))
                        {
                            Load(sr);
                        }
                    }
                }
                else if (command == "list")//FIXME: If list before load "System.NullReferenceException", add try/catch
                {
                    foreach (SweEngGloss gloss in dictionary)
                    {
                        Console.WriteLine($"{gloss.word_swe}  - {gloss.word_eng}"); //Removed "-10", unclear if it did anything
                    }
                }
                else if (command == "new")
                {
                    NewWord(argument);
                }
                else if (command == "delete")
                {
                    if (argument.Length == 3)
                    {
                        DeleteWordsPrompt(argument);
                    }
                    else if (argument.Length == 1)
                    {
                        DeleteWords();
                    }
                }
                else if (command == "translate")
                {
                    string translateWord;
                    if (argument.Length == 2)
                    {
                        translateWord = argument[1];
                        Translate(translateWord);
                    }
                    else if (argument.Length == 1)
                    {
                        Console.WriteLine("Write word to be translated: ");
                        translateWord = Console.ReadLine();
                                Translate(translateWord);
                    }
                }
                else if (command == "help")
                {
                    Help();
                }
                else
                {
                    Console.WriteLine($"Unknown command: '{command}'");
                }
            }
            while (true);
        }

        private static void Translate(string translateWord)
        {
            foreach (SweEngGloss gloss in dictionary)
            {
                if (gloss.word_swe == translateWord)
                    Console.WriteLine($"English for {gloss.word_swe} is {gloss.word_eng}");
                if (gloss.word_eng == translateWord)
                    Console.WriteLine($"Swedish for {gloss.word_eng} is {gloss.word_swe}");
            }
        }

        private static void DeleteWordsPrompt(string[] argument)
        {
            int index = -1;
            for (int i = 0; i < dictionary.Count; i++)
            {
                SweEngGloss gloss = dictionary[i];
                if (gloss.word_swe == argument[1] && gloss.word_eng == argument[2])
                    index = i;
            }
            dictionary.RemoveAt(index);
        }

        private static void DeleteWords()
        {
            Console.WriteLine("Write word in Swedish: ");
            string swedishWord = Console.ReadLine();
            Console.Write("Write word in English: ");
            string englishWord = Console.ReadLine();
            int index = -1;
            for (int i = 0; i < dictionary.Count; i++)
            {
                SweEngGloss gloss = dictionary[i];
                if (gloss.word_swe == swedishWord && gloss.word_eng == englishWord)
                    index = i;
            }
            dictionary.RemoveAt(index);
        }

        private static void NewWord(string[] argument)
        {
            if (argument.Length == 3)
            {
                dictionary.Add(new SweEngGloss(argument[1], argument[2]));
            }
            else if (argument.Length == 1)
            {
                Console.WriteLine("Write word in Swedish: ");
                string swedishWord = Console.ReadLine();
                Console.Write("Write word in English: ");
                string englishWord = Console.ReadLine();
                dictionary.Add(new SweEngGloss(swedishWord, englishWord));
            }
        }

        private static void Load(StreamReader sr)
        {
            dictionary = new List<SweEngGloss>(); // Empty it!
            string line = sr.ReadLine();
            while (line != null)
            {
                SweEngGloss gloss = new SweEngGloss(line);
                dictionary.Add(gloss);
                line = sr.ReadLine();
            }
        }

        static void Help()
        {
            Console.WriteLine("Commands:\nhelp - show commands" +
                "                       \nlist - list saved words" +
                "                       \nload - load file" +
                "                       \nload /file/ - load with specified file name" +
                "                       \nnew - add new words with prompts" +
                "                       \nnew /word in swedish/ /word in english/ - add new words directly" +
                "                       \ndelete /word in swedish/ /word in english/ - delete words directly" +
                "                       \ndelete - delete words with prompts" +
                "                       \ntranslate /word in swedish/ or /word in english/ - translate word" +
                "                       \ntranslate - translate word from english or swedish with prompts" +
                "                       \nquit - quit program"
               );
        }
    }
}