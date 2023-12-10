var input = File.ReadAllLines("./input.txt");
var map = new Dictionary<(int, int), string>();
var y = 0;
foreach (var line in input)
{
    var x = 0;
    foreach (var c in line)
    {
        map.Add((x, y), c.ToString());
        x++;
    }
    y++;
}

var s = map.First(x => x.Value == "S");
var pipe = new Dictionary<(int, int), (string, bool)>();
var attachedPipes = new Dictionary<(int, int), string>();
pipe.Add((s.Key.Item1, s.Key.Item2), (s.Value, false));

while (true)
{
    var nextPipe = pipe.FirstOrDefault(x => x.Value.Item2 == false);
    if (string.IsNullOrEmpty(nextPipe.Value.Item1))
    {
        break;
    }
    attachedPipes.Clear();
    attachedPipes = GetAttachedPipes(KeyValuePair.Create((nextPipe.Key.Item1, nextPipe.Key.Item2), nextPipe.Value.Item1));
    pipe.Remove((nextPipe.Key.Item1, nextPipe.Key.Item2));
    pipe.Add((nextPipe.Key.Item1, nextPipe.Key.Item2), (nextPipe.Value.Item1, true));
    foreach (var attachedPipe in attachedPipes)
    {
        if (!pipe.ContainsKey(attachedPipe.Key))
        {
            var newPipe = KeyValuePair.Create((attachedPipe.Key.Item1, attachedPipe.Key.Item2), (attachedPipe.Value, false));
            pipe.Add(newPipe.Key, newPipe.Value);
        }
    }
}

Console.WriteLine(pipe.Count/2);

Dictionary<(int, int), string> GetAttachedPipes(KeyValuePair<(int, int), string> pipe)
{
    var attachedPipes = new Dictionary<(int, int), string>();

    var leftPipe = map.FirstOrDefault(x => x.Key.Item1 == pipe.Key.Item1 - 1 && x.Key.Item2 == pipe.Key.Item2);
    var rightPipe = map.FirstOrDefault(x => x.Key.Item1 == pipe.Key.Item1 + 1 && x.Key.Item2 == pipe.Key.Item2);
    var topPipe = map.FirstOrDefault(x => x.Key.Item1 == pipe.Key.Item1 && x.Key.Item2 == pipe.Key.Item2 - 1);
    var bottomPipe = map.FirstOrDefault(x => x.Key.Item1 == pipe.Key.Item1 && x.Key.Item2 == pipe.Key.Item2 + 1);
    if (pipe.Value == "S")
    {
        if (ShouldAddPipe(leftPipe.Value, "FL-")) attachedPipes.Add(leftPipe.Key, leftPipe.Value);
        if (ShouldAddPipe(rightPipe.Value, "J7-")) attachedPipes.Add(rightPipe.Key, rightPipe.Value);
        if (ShouldAddPipe(topPipe.Value, "F7|")) attachedPipes.Add(topPipe.Key, topPipe.Value);
        if (ShouldAddPipe(bottomPipe.Value, "JL|")) attachedPipes.Add(bottomPipe.Key, bottomPipe.Value);
    }
    if (pipe.Value == "|")
    {
        if (ShouldAddPipe(topPipe.Value, "F7|")) attachedPipes.Add(topPipe.Key, topPipe.Value);
        if (ShouldAddPipe(bottomPipe.Value, "JL|")) attachedPipes.Add(bottomPipe.Key, bottomPipe.Value);
    }
    if (pipe.Value == "-")
    {
        if (ShouldAddPipe(leftPipe.Value, "FL-")) attachedPipes.Add(leftPipe.Key, leftPipe.Value);
        if (ShouldAddPipe(rightPipe.Value, "J7-")) attachedPipes.Add(rightPipe.Key, rightPipe.Value);
    }
    if (pipe.Value == "L")
    {
        if (ShouldAddPipe(topPipe.Value, "F7|")) attachedPipes.Add(topPipe.Key, topPipe.Value);
        if (ShouldAddPipe(rightPipe.Value, "J7-")) attachedPipes.Add(rightPipe.Key, rightPipe.Value);
    }
    if (pipe.Value == "J")
    {
        if (ShouldAddPipe(topPipe.Value, "F7|")) attachedPipes.Add(topPipe.Key, topPipe.Value);
        if (ShouldAddPipe(leftPipe.Value, "FL-")) attachedPipes.Add(leftPipe.Key, leftPipe.Value);
    }
    if (pipe.Value == "7")
    {
        if (ShouldAddPipe(bottomPipe.Value, "JL|")) attachedPipes.Add(bottomPipe.Key, bottomPipe.Value);
        if (ShouldAddPipe(leftPipe.Value, "FL-")) attachedPipes.Add(leftPipe.Key, leftPipe.Value);
    }
    if (pipe.Value == "F")
    {
        if (ShouldAddPipe(bottomPipe.Value, "JL|")) attachedPipes.Add(bottomPipe.Key, bottomPipe.Value);
        if (ShouldAddPipe(rightPipe.Value, "J7-")) attachedPipes.Add(rightPipe.Key, rightPipe.Value);
    }

    return attachedPipes;
}

bool ShouldAddPipe(string pipeValue, string searchString)
{
    return !string.IsNullOrEmpty(pipeValue) && pipeValue.IndexOfAny(searchString.ToCharArray()) != -1;
}