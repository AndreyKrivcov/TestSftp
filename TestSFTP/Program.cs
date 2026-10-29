
using Renci.SshNet;

using var client = new SftpClient(args[0], args[1],  args[2]);
await client.ConnectAsync(CancellationToken.None);

var list = client.ListDirectoryAsync(args[3], CancellationToken.None);

await foreach (var item in list)
{
    Console.WriteLine($"{item.Name} : {item.FullName}");
}