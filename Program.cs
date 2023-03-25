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
            string path = "..\\..\\..\\dict\\";
            string defaultFile = "sweeng.lis";
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
                        try
                        {
                            using (StreamReader sr = new StreamReader(path + argument[1]))
                            {
                                Load(sr);
                            }
                        }
                        catch (FileNotFoundException)
                        {
                            Console.WriteLine("File not found");
                        }
                    }
                    else if (argument.Length == 1)
                    {
                        using (StreamReader sr = new StreamReader(path + defaultFile))
                        {
                            Load(sr);
                        }
                    }
                }
                else if (command == "list")
                {
                    try
                    {
                        foreach (SweEngGloss gloss in dictionary)
                        {
                            Console.WriteLine($"{gloss.word_swe}  - {gloss.word_eng}"); //Removed "-10", unclear if it did anything
                        }
                    }
                    catch(NullReferenceException)
                    {
                        Console.WriteLine("File not loaded");
                    }
                }
                else if (command == "new")
                {
                    try
                    {
                        NewWord(argument);
                                          
                    }
                    catch (NullReferenceException)
                    {
                        Console.WriteLine("File not loaded");
                    }
                }

                else if (command == "delete")
                                             //TODO: Change so Gloss is deleted with just word in one language
                                             
                {
                    try
                    {
                        string englishWord;
                        string swedishWord;
                        if (argument.Length == 3)
                        {
                            swedishWord = argument[1];
                            englishWord = argument[2];
                            DeleteWords(englishWord, swedishWord);
                        }
                        else if (argument.Length == 1)
                        {
                            Console.WriteLine("Write word in Swedish: ");
                            swedishWord = Console.ReadLine();
                            Console.Write("Write word in English: ");
                            englishWord = Console.ReadLine();
                            DeleteWords(englishWord, swedishWord);
                        }
                    }
                    catch (NullReferenceException) 
                    {
                        Console.WriteLine("File not loaded");
                    }
                    catch (ArgumentOutOfRangeException) 
                    {
                        Console.WriteLine("Word is not in the dictionary");
                    }
                }
                else if (command == "translate")//TODO: "Else" for words not on list
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
        private static void DeleteWords(string englishWord, string swedishWord)
        {
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
            else {

                Console.WriteLine("Write >new to add new words with prompts or >new /word in swedish/ /word in english/ to add new words directly");
                    
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