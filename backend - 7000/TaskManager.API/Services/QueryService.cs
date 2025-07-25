using System.Xml;

namespace TaskManager.API.Services
{
    public class QueryService : IQueryService
    {
        private readonly Dictionary<string, string> _queries;

        public QueryService()
        {
            _queries = new Dictionary<string, string>();
            LoadQueries();
        }

        private void LoadQueries()
        {
            try
            {
                var xmlPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "queries.xml");
                
                if (!File.Exists(xmlPath))
                {
                    throw new FileNotFoundException($"Queries XML file not found at: {xmlPath}");
                }

                var xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);

                var queryNodes = xmlDoc.SelectNodes("//query");
                
                if (queryNodes != null)
                {
                    foreach (XmlNode queryNode in queryNodes)
                    {
                        var nameAttribute = queryNode.Attributes?["name"];
                        var sqlNode = queryNode.SelectSingleNode("sql");
                        
                        if (nameAttribute != null && sqlNode != null)
                        {
                            _queries[nameAttribute.Value] = sqlNode.InnerText.Trim();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to load queries from XML file", ex);
            }
        }

        public string GetQuery(string queryName)
        {
            if (_queries.TryGetValue(queryName, out var query))
            {
                return query;
            }
            
            throw new ArgumentException($"Query '{queryName}' not found in configuration");
        }
    }
}