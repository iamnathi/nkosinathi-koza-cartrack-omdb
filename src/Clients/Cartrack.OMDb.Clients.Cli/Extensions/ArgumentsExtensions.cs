using System;
using System.Collections.Generic;
using System.Text;

namespace Cartrack.OMDb.Clients.Cli.Extensions
{
    public static class ArgumentsExtensions
    {
        public static string[] ParseWithSpace(this string[] args)
        {
            if (args.Length == 0)
            {
                return Array.Empty<string>();
            }

            bool concantinate = false;
            var list = new List<string>();
            StringBuilder builder = new StringBuilder();

            for (int index = 0; index < args.Length; index++)
            {
                var exp = args[index];

                if (exp.StartsWith("\"") && !exp.EndsWith("\""))
                {
                    concantinate = true;
                    builder.Append(exp.Substring(1));
                    continue;
                }

                if (exp.StartsWith("\"") && exp.EndsWith("\""))
                {
                    list.Add(exp.Substring(1, exp.Length - 2));
                    continue;
                }


                if (concantinate)
                {
                    if (exp.EndsWith("\""))
                    {
                        concantinate = false;
                        builder.Append($" {exp.Substring(0, exp.Length - 1)}");

                        list.Add(builder.ToString());
                        builder.Clear();
                        continue;
                    }
                    else
                    {
                        builder.Append($" {exp}");
                        continue;
                    }                    
                }

                list.Add(exp);
            }


            return list.ToArray();
        }
    }
}
