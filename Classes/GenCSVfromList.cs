using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace PlayListCreator_FW.Classes
{
    public class GenCSVfromList
    {
        public void ExportListToCsv(List<string> list, System.Web.HttpContext context)
        {
            // Set response headers
            context.Response.Clear();
            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Content-Disposition", "attachment; filename=PlaylistYT.csv");

            using (StreamWriter writer = new StreamWriter(context.Response.OutputStream))
            {
                foreach (var line in list)
                {
                    // Escape commas and quotes if needed
                    string escaped = line.Replace("\"", "\"\""); // Escape double quotes
                    if (escaped.Contains(",") || escaped.Contains("\""))
                        escaped = $"\"{escaped}\""; // Wrap in quotes if necessary

                    writer.WriteLine(escaped);
                }
            }
            context.Response.End();
        }

    }
}