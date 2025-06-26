using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyFITJob.Api.MarketAnalysis.Application;

public interface ISkillExtractorService
{
    /// <summary>
    /// Extrait et compte les compétences à partir d'un texte
    /// </summary>
    /// <param name="text">Le texte contenant les compétences à extraire</param>
    /// <returns>Un dictionnaire avec les compétences et leur nombre d'occurrences</returns>
    Task<Dictionary<string, int>> ExtractSkillsAsync(string text);
} 