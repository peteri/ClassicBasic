// <copyright file="Tokeniser.cs" company="Peter Ibbotson">
// (C) Copyright 2017 Peter Ibbotson
// </copyright>

namespace ClassicBasic.Interpreter
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Used to tokenise a line.
    /// </summary>
    public class Tokeniser : ITokeniser
    {
        private readonly ITokensProvider _tokensProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Tokeniser"/> class.
        /// </summary>
        /// <param name="tokensProvider">Token provider with a list of tokens.</param>
        public Tokeniser(ITokensProvider tokensProvider)
        {
            _tokensProvider = tokensProvider;
        }

        /// <summary>
        /// Tokenises a line of text.
        /// </summary>
        /// <param name="command">Command typed by the user.</param>
        /// <returns>List of parsed tokens.</returns>
        public ProgramLine Tokenise(string command)
        {
            int? lineNumber = null;
            List<IToken> tokens = new List<IToken>();
            string currentTokenText = string.Empty;
            bool inQuotes = false;
            bool lineNumberValid = true;
            bool remarkMode = false;
            foreach (var c in command)
            {
                // Skip white space.
                if (!inQuotes && char.IsWhiteSpace(c))
                {
                    continue;
                }

                // If it's remark mode then keep on going....
                if (remarkMode)
                {
                    currentTokenText += c;
                    inQuotes = true;
                    continue;
                }

                // Deal with line numbers.
                if (char.IsDigit(c) && lineNumberValid)
                {
                    lineNumber = (lineNumber.GetValueOrDefault() * 10) + (c - '0');
                    continue;
                }

                // Not digit or white space so no line number.
                lineNumberValid = false;

                // Got a quote character?
                if (c == '\"')
                {
                    if (inQuotes)
                    {
                        // Found the end of our string
                        inQuotes = false;
                        tokens.Add(new Token(currentTokenText, TokenType.ClassString));
                    }
                    else
                    {
                        // If we already have text then it should be a variable or number.
                        // So create a token for what we have and turn on quotes mode.
                        inQuotes = true;
                        if (currentTokenText != string.Empty)
                        {
                            tokens.Add(new Token(currentTokenText));
                        }
                    }

                    currentTokenText = string.Empty;
                    continue;
                }

                if (inQuotes)
                {
                    currentTokenText = currentTokenText + c;
                    continue;
                }

                currentTokenText = currentTokenText + char.ToUpper(c);
                var retryMatch = false;

                do
                {
                    retryMatch = false;

                    // Not a quote so maybe it's a token.
                    var matches = _tokensProvider.Tokens.Where(t => t.Text.StartsWith(currentTokenText)).ToArray();

                    // Too many matches, loop and get another character.
                    if (matches.Length > 1)
                    {
                        continue;
                    }

                    // We're matching at the start of the line.
                    if (matches.Length == 1)
                    {
                        if (matches[0].Text == currentTokenText)
                        {
                            // Exact match we're done.
                            tokens.Add(matches[0]);
                            currentTokenText = string.Empty;
                            remarkMode = matches[0].Statement == TokenType.Remark;
                        }

                        // Okay don't try the mid match
                        continue;
                    }

                    // Now for the awkward bit we want to turn AFOR in A FOR not AF OR
                    // ACRIGHT$ should be AC RIGHT$ not AC RIGHT $
                    matches = _tokensProvider.Tokens.Where(t => currentTokenText.Contains(t.Text)).OrderByDescending(t => t.Text.Length).ToArray();
                    if (matches.Length > 0)
                    {
                        var start = currentTokenText.IndexOf(matches[0].Text);
                        if (start != 0)
                        {
                            var text = currentTokenText.Substring(0, start);
                            tokens.Add(new Token(text));
                        }

                        tokens.Add(matches[0]);
                        currentTokenText = currentTokenText.Substring(start + matches[0].Text.Length);
                        retryMatch = currentTokenText != string.Empty;
                    }
                }
                while (retryMatch);
            }

            if (currentTokenText != string.Empty)
            {
                if (remarkMode)
                {
                    tokens.Add(new Token(currentTokenText, TokenType.ClassRemark));
                }
                else if (inQuotes)
                {
                    tokens.Add(new Token(currentTokenText, TokenType.ClassString));
                }
                else
                {
                    var matches = _tokensProvider.Tokens.Where(t => currentTokenText.Contains(t.Text)).OrderByDescending(t => t.Text.Length).ToArray();
                    if (matches.Length > 0)
                    {
                        var start = currentTokenText.IndexOf(matches[0].Text);
                        if (start != 0)
                        {
                            var text = currentTokenText.Substring(0, start);
                            tokens.Add(new Token(text));
                        }

                        tokens.Add(matches[0]);
                        currentTokenText = currentTokenText.Substring(start + matches[0].Text.Length);
                    }

                    if (currentTokenText != string.Empty)
                    {
                        tokens.Add(new Token(currentTokenText));
                    }
                }
            }

            return new ProgramLine(lineNumber, tokens);
        }
    }
}
