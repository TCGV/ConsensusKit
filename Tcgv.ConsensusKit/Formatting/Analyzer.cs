using System.Text;
using Tcgv.ConsensusKit.Control;
using Tcgv.ConsensusKit.Exchange;

namespace Tcgv.ConsensusKit.Formatting
{
    public class Analyzer
    {
        public string PrintHistory(Instance[] instances)
        {
            var sb = new StringBuilder();

            foreach (var r in instances)
            {
                var line = "";
                var count = 0;
                foreach (var msg in r.QueryMessages())
                {
                    string newLine = PrintLine(msg);
                    count++;
                    if (newLine != line)
                    {
                        if (!string.IsNullOrEmpty(line))
                            sb.AppendLine($"{line}{(count > 1 ? $"\tx{count}" : "")}");
                        count = 0;
                        line = newLine;
                    }
                }
                sb.AppendLine($"{line}{(count > 1 ? $"\tx{count}" : "")}");
                sb.AppendLine($"[{r.Consensus}, {(r.Consensus ? r.Value : "null")}]");
                sb.AppendLine($"------------------------------");
            }

            return sb.ToString();
        }

        private string PrintLine(Message msg)
        {
            return $"{(msg.Destination == null ? "*" : msg.Destination.Id.ToString())}\t{msg.Type}\t{msg.Value ?? "null"}";
        }
    }
}
