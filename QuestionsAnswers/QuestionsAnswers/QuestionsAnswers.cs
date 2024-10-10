using Azure.AI.Language.QuestionAnswering;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using System.Linq;
using Azure;

namespace QuestionsAnswers
{
    public class QuestionAnswerService
    {
        private readonly QuestionAnsweringClient _client;
        private readonly QuestionAnsweringProject _project;

        public QuestionAnswerService()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var endpoint = config["Azure:QuestionAnswering:Endpoint"] ??
                throw new InvalidOperationException("Endpoint is not configured in appsettings.json");
            var apiKey = config["Azure:QuestionAnswering:ApiKey"] ??
                throw new InvalidOperationException("ApiKey is not configured in appsettings.json");
            var projectName = config["Azure:QuestionAnswering:ProjectName"] ??
                throw new InvalidOperationException("ProjectName is not configured in appsettings.json");
            var deploymentName = config["Azure:QuestionAnswering:DeploymentName"] ??
                throw new InvalidOperationException("DeploymentName is not configured in appsettings.json");

            _client = new QuestionAnsweringClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            _project = new QuestionAnsweringProject(projectName, deploymentName);
        }

        public async Task<string> GetAnswerAsync(string question)
        {
            try
            {
                var response = await _client.GetAnswersAsync(question, _project);

                if (response.Value.Answers.Any())
                {
                    var topAnswer = response.Value.Answers.First();
                    return topAnswer.Answer;
                }
                else
                {
                    return "I'm sorry, I don't have an answer for that question. Is there something else I can help you with?";
                }
            }
            catch (Exception ex)
            {
                return $"I'm having trouble processing your question right now. Can you try asking something else? (Error: {ex.Message})";
            }
        }
    }
}