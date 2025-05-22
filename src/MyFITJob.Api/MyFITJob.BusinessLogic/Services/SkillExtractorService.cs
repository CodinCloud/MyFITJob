using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFITJob.BusinessLogic.Services
{
    public class SkillExtractorService : ISkillExtractorService
    {
        private readonly Dictionary<string, string[]> _skillKeywords;

        public SkillExtractorService()
        {
            // Dictionnaire des compétences clés et leurs variations
            _skillKeywords = new Dictionary<string, string[]>
            {
                { "C#", new[] { "c#", "csharp", "dotnet", ".net" } },
                { "Java", new[] { "java", "j2ee", "spring" } },
                { "Python", new[] { "python", "django", "flask" } },
                { "JavaScript", new[] { "javascript", "js", "typescript", "ts", "react", "angular", "vue" } },
                { "SQL", new[] { "sql", "mysql", "postgresql", "sql server", "oracle" } },
                { "DevOps", new[] { "devops", "docker", "kubernetes", "ci/cd", "jenkins", "gitlab" } },
                { "Cloud", new[] { "aws", "azure", "gcp", "cloud" } },
                { "Agile", new[] { "agile", "scrum", "kanban" } },
                { "Testing", new[] { "testing", "unit test", "integration test", "qa" } },
                { "Security", new[] { "security", "cybersecurity", "owasp" } }
            };
        }

        public async Task<Dictionary<string, int>> ExtractSkillsAsync(string requirementsText)
        {
            if (string.IsNullOrWhiteSpace(requirementsText))
                return new Dictionary<string, int>();

            var result = new Dictionary<string, int>();
            var text = requirementsText.ToLower();

            foreach (var skill in _skillKeywords)
            {
                var count = skill.Value.Sum(keyword => 
                    CountOccurrences(text, keyword));
                
                if (count > 0)
                {
                    result[skill.Key] = count;
                }
            }

            return await Task.FromResult(result);
        }

        private int CountOccurrences(string text, string keyword)
        {
            int count = 0;
            int position = 0;
            
            while ((position = text.IndexOf(keyword, position, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                count++;
                position += keyword.Length;
            }

            return count;
        }
    }
} 