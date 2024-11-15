using System.Text.Json.Serialization;

namespace Examples.Models.Entities
{
    public class ExDbParticipant
    {
        public int Id { get; set; }
        public ParticipantActive Active { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public ParticipantPhase Phase { get; set; }
        public bool Deleted { get; set; } = false;
        [JsonIgnore]
        public ICollection<ExDbHistory>? History { get; set; }

        public override string ToString()
        {
            string output = "\n-------------------------------\n";

            output += $"Participant ID: {Id},\n";
            output += $"Participant Name: {Name},\n";
            output += $"Most Recent Update:\n";

            if (History == null) { output += "\t(Participant has no history entries logged.)"; }
            else { output += "\t" + History.First().ToString(); }

            output += "\n-------------------------------\n";

            return output;
        }
    }
}
