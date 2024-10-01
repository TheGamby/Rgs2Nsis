if (args.Length != 2)
{
    Console.WriteLine("Usage: RgsToNsisConverter <input.rgs> <output.nsi>");
    return;
}

File.WriteAllText(args[1], RgsToNsisConverter.ConvertRgsToNsis(File.ReadAllText(args[0])));
Console.WriteLine($"Generated NSIS Script: {args[1]}");
