namespace Flow.Launcher.Plugin.GitHubRepositoryViewer.Helpers;

public static class FuzzyScore
{
    /**
    * This is a port of the FuzzyScore algorithm that is a part of the Apache Commons project
    * https://commons.apache.org/proper/commons-text/apidocs/org/apache/commons/text/similarity/FuzzyScore.html
    */
    public static int Score(string term, string query)
    {
        term = term.ToLowerInvariant().Trim();
        query = query.ToLowerInvariant().Trim();

        int score = 0;
        int termPosition = 0;
        int previousMatchPosition = int.MinValue;

        foreach (char queryCharacter in query)
        {
            bool found = false;

            for (int i = termPosition; i < term.Length; i++)
            {
                if (queryCharacter != term[i])
                {
                    continue;
                }

                score += 1;

                // bonus points for consecutive character matches
                if (previousMatchPosition + 1 == i)
                {
                    score += 2;
                }

                previousMatchPosition = i;
                termPosition = i + 1;
                found = true;

                break;
            }

            if (!found)
            {
                return 0;
            }
        }

        return score;
    }
}
