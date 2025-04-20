using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LabyrinthGame
{
    public class GameInstructions
    {
        private readonly Dictionary<string, string> instructions = new Dictionary<string, string>();

        public void AddInstruction(string key, string message)
        {
            if (!instructions.ContainsKey(key))
            {
                instructions[key] = message;
            }
        }
        public bool ContainsInstruction(string key)
        {
            return instructions.ContainsKey(key);
        }

        public IEnumerable<string> GetInstructions()
        {
            return instructions.Values;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var instruction in instructions.Values)
            {
                sb.AppendLine(instruction);
            }
            return sb.ToString();
        }
    }
}
