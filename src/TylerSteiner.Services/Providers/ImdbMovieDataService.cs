using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CsQuery;
using Microsoft.Extensions.Logging;
using TylerSteiner.Models;
using TylerSteiner.Models.Providers;
using TylerSteiner.Services.Internal;

namespace TylerSteiner.Services.Providers
{
    public class ImdbMovieDataService : IImdbMovieDataService
    {
        private const string FullCreditsFormat = "http://www.imdb.com/title/{0}/fullcredits";
        private const string CompanyCreditsFormat = "http://www.imdb.com/title/{0}/companycredits";

        private readonly IHttpClientProvider _httpClientProvider;
        private readonly ILogger<ImdbMovieDataService> _logger;

        public ImdbMovieDataService(IHttpClientProvider httpClientProvider, ILogger<ImdbMovieDataService> logger)
        {
            _httpClientProvider = httpClientProvider;
            _logger = logger;
        }

        public async Task<ImdbMovieData> GetMovieDataAsync(string imdbId)
        {
            try
            {
                return await GetMovieDataInternal(imdbId);
            }
            catch (Exception e)
            {
                _logger.LogCritical("Failed to get IMDB data", e);
                return null;
            }
        }

        private async Task<ImdbMovieData> GetMovieDataInternal(string imdbId)
        {
            if (string.IsNullOrWhiteSpace(imdbId))
            {
                throw new ArgumentNullException(nameof(imdbId));
            }
            
            var http = _httpClientProvider.Get();

            var getFullCredits = http.GetAsync(string.Format(FullCreditsFormat, imdbId));
            var getCompanyCredits = http.GetAsync(string.Format(CompanyCreditsFormat, imdbId));

            var fullCredits = await getFullCredits;
            var companyCredits = await getCompanyCredits;

            if (!fullCredits.IsSuccessStatusCode || !companyCredits.IsSuccessStatusCode)
            {
                _logger.LogInformation("Scraper failed to retrieve data for '{ImdbId}'", imdbId);
                return null;
            }
            
            CQ fullCreditsDom = await fullCredits.Content.ReadAsStringAsync();
            CQ companyCreditsDom = await companyCredits.Content.ReadAsStringAsync();

            var actors = ParseActors(fullCreditsDom);
            var directors = ParseFullCreditsTable(
                fullCreditsDom,
                "Directed by",
                credit => true,
                (id, name) => new Director {ImdbId = id, Name = name});
            var writers = ParseFullCreditsTable(
                fullCreditsDom,
                "Writing Credits",
                credit => true,
                (id, name) => new Writer {ImdbId = id, Name = name});
            var producers = ParseFullCreditsTable(
                fullCreditsDom,
                "Produced by",
                credit => credit == "producer",
                (id, name) => new Producer { ImdbId = id, Name = name });
            var composers = ParseFullCreditsTable(
                fullCreditsDom,
                "Music by",
                credit => true,
                (id, name) => new Composer { ImdbId = id, Name = name });
            var cinematographers = ParseFullCreditsTable(
                fullCreditsDom,
                "Cinematography by",
                credit => true,
                (id, name) => new Cinematographer { ImdbId = id, Name = name });
            var studios = ParseCompanyCreditsList(
                companyCreditsDom,
                "Production Companies",
                line => true,
                (id, name) => new Studio { ImdbId = id, Name = name });
            var distributors = ParseCompanyCreditsList(
                companyCreditsDom,
                "Distributors",
                line => line.Contains("(USA) (theatrical)"),
                (id, name) => new Distributor { ImdbId = id, Name = name });;

            var data = new ImdbMovieData
            {
                ImdbId = imdbId,
                Directors = directors.Distinct(new ImdbEqualityComparer<Director>()).ToList(),
                Writers = writers.Distinct(new ImdbEqualityComparer<Writer>()).ToList(),
                Actors = actors.Distinct(new ImdbEqualityComparer<Actor>()).ToList(),
                Producers = producers.Distinct(new ImdbEqualityComparer<Producer>()).ToList(),
                Composers = composers.Distinct(new ImdbEqualityComparer<Composer>()).ToList(),
                Cinematographers = cinematographers.Distinct(new ImdbEqualityComparer<Cinematographer>()).ToList(),
                Distributors = distributors.Distinct(new ImdbEqualityComparer<Distributor>()).ToList(),
                Studios = studios.Distinct(new ImdbEqualityComparer<Studio>()).ToList(),
            };

            return data;
        }

        private static IEnumerable<Actor> ParseActors(CQ dom)
        {
            foreach (var td in dom[".cast_list td.itemprop"])
            {
                var span = td.Cq().Find("span");
                var a = td.Cq().Find("a");

                var name = span.Text();
                var href = a.Attr("href");
                var actorId = ParseImdbId(href);

                yield return new Actor
                {
                    Name = name,
                    ImdbId = actorId,
                };
            }
        }

        private static IEnumerable<T> ParseFullCreditsTable<T>(
            CQ dom, 
            string tableTitle, 
            Func<string, bool> creditFilter,
            Func<string, string, T> factory)
        {
            var header = dom["h4.dataHeaderWithBorder"].FirstOrDefault(d => d.InnerText.Contains(tableTitle));
            if (header == null)
            {
                yield break;
            }

            var table = header.NextElementSibling.Cq();

            foreach (var tr in table.Find("tr"))
            {
                var name = tr.Cq().Find("td.name").Text();
                var cleanedName = CleanName(name);

                var href = tr.Cq().Find("a").Attr("href");
                var imdbId = ParseImdbId(href);

                var credit = tr.Cq().Find(".credit").Text();
                var cleanedCredit = CleanName(credit);

                if (!creditFilter(cleanedCredit))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(imdbId) || string.IsNullOrWhiteSpace(cleanedName))
                {
                    continue;
                }

                yield return factory(imdbId, cleanedName);
            }
        }

        private static IEnumerable<T> ParseCompanyCreditsList<T>(
            CQ dom,
            string tableTitle,
            Func<string, bool> nameFilter,
            Func<string, string, T> factory)
        {
            var header = dom["h4.dataHeaderWithBorder"].FirstOrDefault(d => d.InnerText.Contains(tableTitle));
            if (header == null)
            {
                yield break;
            }

            var ul = header.NextElementSibling.Cq();

            foreach (var li in ul.Find("li"))
            {
                var a = li.Cq().Find("a");

                if (!nameFilter(li.InnerText))
                {
                    continue;
                }

                var name = a.Text();
                var href = a.Attr("href");

                var imdbId = ParseImdbId(href);
                var cleanedName = CleanName(name);

                if (string.IsNullOrWhiteSpace(imdbId) || string.IsNullOrWhiteSpace(cleanedName))
                {
                    continue;
                }

                yield return factory(imdbId, cleanedName);
            }
        }

        private static string ParseImdbId(string link)
        {
            if (string.IsNullOrWhiteSpace(link)) return null;
            var start = link.IndexOf("/", 1, StringComparison.Ordinal);
            return link.Substring(start + 1, 9);
        }

        private static string CleanName(string name)
        {
            return name?.Replace("\n", "").Trim();
        }
    }
}