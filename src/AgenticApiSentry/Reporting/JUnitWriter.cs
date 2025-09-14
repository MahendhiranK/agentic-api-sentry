using System.Xml.Linq;
using AgenticApiSentry.Agent;

namespace AgenticApiSentry.Reporting
{
    public static class JUnitWriter
    {
        public static void Write(string path, List<ToolResult> results)
        {
            var suite = new XElement("testsuite", new XAttribute("name", "AgenticApiSentry"));
            foreach (var r in results)
            {
                var caseEl = new XElement("testcase",
                    new XAttribute("classname", r.ToolName),
                    new XAttribute("name", r.Summary));

                if (r.Severity == "error")
                {
                    caseEl.Add(new XElement("failure", new XAttribute("message", r.Summary)));
                }
                suite.Add(caseEl);
            }
            var doc = new XDocument(new XElement("testsuites", suite));
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            doc.Save(path);
        }
    }
}
