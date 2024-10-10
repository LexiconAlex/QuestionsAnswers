using System;
using System.Threading.Tasks;

namespace QuestionsAnswers
{
    public class Program
    {
        private static QuestionAnswerService _qaService = null!;

        public static async Task Main(string[] args)
        {
            try
            {
                _qaService = new QuestionAnswerService();
                await RunBotAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        private static async Task RunBotAsync()
        {
            Console.WriteLine("Welcome to the QuestionsAnswers Bot!");
            Console.WriteLine("Try to ask me something. Type 'exit' or 'quit' to end the conversation.");

            while (true)
            {
                Console.Write("\nYou: ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? string.Empty;

                if (input == "exit" || input == "quit")
                {
                    Console.WriteLine("Bot: Goodbye! Have a great day!");
                    break;
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Bot: I'm sorry, I didn't catch that. Could you please repeat?");
                    continue;
                }

                if (IsGreeting(input))
                {
                    Console.WriteLine("Bot: Hello! How can I assist you today?");
                    continue;
                }

                string response = await _qaService.GetAnswerAsync(input);
                Console.WriteLine($"Bot: {response}");
            }
        }

        private static bool IsGreeting(string input)
        {
            string[] greetings = { "hello", "hi", "hey", "greetings", "good morning", "good afternoon", "good evening" };
            return Array.Exists(greetings, g => input.Contains(g));
        }
    }
}