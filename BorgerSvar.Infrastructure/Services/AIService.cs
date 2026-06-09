using BorgerSvar.Application.Common.Interfaces;
using BorgerSvar.Domain;

namespace BorgerSvar.Infrastructure.Services;

public class AIService : IAIService
{
    public async Task<string> GenerateResponseAsync(CitizenQuery citizenQuery, CancellationToken cancellationToken = default)
    {
        // Vi simulerer, at en stor AI-model (LLM) tænker sig om i et halvt sekund
        await Task.Delay(500, cancellationToken);

        var question = citizenQuery.Question.ToLower();

        // En simpel, men smart simulations-motor baseret på søgeord
        if (question.Contains("affald") || question.Contains("skrald") || question.Contains("tømmes"))
        {
            return $"Hej {citizenQuery.CitizenIdCard.FirstName}. Din skraldespand bliver tømt hver anden torsdag i lige uger. Husk at stille spanden helt frem til skellet inden kl. 06:00.";
        }
        
        if (question.Contains("pas") || question.Contains("forny"))
        {
            return $"Hej {citizenQuery.CitizenIdCard.FirstName}. Du kan forny dit pas online på kommunens hjemmeside. Du skal dog stadig bestille tid til at få taget biometrisk foto i Borgerservice.";
        }

        if (question.Contains("flytte") || question.Contains("adresse"))
        {
            return $"Hej {citizenQuery.CitizenIdCard.FirstName}. Når du flytter, skal du melde din nye adresse på virk.dk eller borger.dk senest 5 dage efter, du er flyttet.";
        }

        // Standard AI-svar, hvis spørgsmålet ikke matcher ovenstående
        return $"Hej {citizenQuery.CitizenIdCard.FirstName}. Tak for din henvendelse. Dit spørgsmål: '{citizenQuery.Question}' er modtaget, og vi vil behandle det hurtigst muligt.";
    }
}