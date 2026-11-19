using Renci.SshNet;

Console.WriteLine(
    $@"Try to read directories for 
Host: {args[0]} 
Username: {args[1]} 
Password: {args[2]} 
Directory: {args[3]} ");

Console.WriteLine();
Console.WriteLine("==========================================================");

try
{
    using var client = new SftpClient(args[0], args[1], args[2]);
    await client.ConnectAsync(CancellationToken.None);

    var list = client.ListDirectoryAsync(args[3], CancellationToken.None);

    await foreach (var item in list)
    {
        Console.WriteLine($"{item.Name} : {item.FullName}");
    }
}
finally
{
    Console.WriteLine("==========================================================");
}