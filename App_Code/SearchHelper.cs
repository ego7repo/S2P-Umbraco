using System;
using System.Collections.Generic;
using System.Linq;
using Examine;
using Umbraco.Web;

public class SearchHelper
{
    
    public IEnumerable<SearchResult> Search(string keywords)
    {
        //grab the external searcher
        var searcher = ExamineManager.Instance.SearchProviderCollection["ExternalSearcher"];
        //not 100% sure what this does
        var searchCriteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);
        var rawQueries = new List<string>();

        if (!string.IsNullOrWhiteSpace(keywords))
        {
            //split multiple words by a 'space' character
            var words = keywords.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToLower()).ToList();

            //exclude some words
            words = words.Except(new List<string>()
            {
                "and",
                "a",
                "or",
                "the"
            }).ToList();

            //this is totally up to you how your search works, here I'm searching each word individually
            foreach (var word in words)
            {
                //this is raw Lucene syntax
                //in plain English, this search does the following:
                //+__IndexType:content <== requires the result to be content and not media
                //-template:0 <== exclude results that have no template
                //nodeName: ""{0}""~{1} <== the name of the node should have the keyword with fuzziness of 0.5           
                //mungedField: ""{0}""~{1} <== the index field 'mungedField' should have the keywords fuzziness of 0.5  

                var contentRawQuery = string.Format(@"(+__IndexType:content -template:0 && (nodeName: ""{0}""~{1} mungedField: ""{0}""~{1}))", word, 0.5);
                rawQueries.Add(contentRawQuery);
            }
        }

        var query = searchCriteria.RawQuery("(" + String.Join(")(", rawQueries) + ")");

        //send the query string to Lucene and return the results
        return searcher.Search(keywords, true).OrderByDescending(x => x.Score);

    }
}