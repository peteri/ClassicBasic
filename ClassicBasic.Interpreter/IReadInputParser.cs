using System.Collections.Generic;

namespace ClassicBasic.Interpreter
{
   /// <summary>
   /// IReadInputParser used by read and input to parse data.
   /// </summary>
   public interface IReadInputParser
   {
      /// <summary>
      /// Gets a value indicating whether there is extra data left.
      /// </summary>
      bool HasExtraData { get; }

      /// <summary>
      /// Clears the current line.
      /// </summary>
      void Clear();
      void ReadVariables(IEnumerable<VariableReference> variableReferences);
   }
}