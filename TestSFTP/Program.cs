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

    Console.WriteLine("===================================================================================");
    Console.WriteLine("Click enter to continue");
    Console.ReadLine();
    Console.WriteLine("===================================================================================");
    
    try
    {
        Console.WriteLine("Create directory \"./MyTestDirectory\"");
        
        client.CreateDirectory(Path.Combine(args[3],"MyTestDirectory").Replace("\\", "/"));
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }

    var isExists = client.Exists(Path.Combine(args[3],"MyTestDirectory").Replace("\\", "/"));
    Console.WriteLine($"Directory {(isExists ? "Created" : "Not exists") }");
    
    Console.WriteLine("Click enter to Remove directory:");
    Console.ReadLine();

    try
    {
        client.DeleteDirectory(Path.Combine(args[3],"MyTestDirectory").Replace("\\", "/"));
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
    
    Console.WriteLine("Create file: \"./MyTestFile.txt\"");

    try
    {
        using MemoryStream ms = new MemoryStream();
        await using var writer = new StreamWriter(ms);
        
        writer.WriteLine("Hello World!");
        
        await client.UploadFileAsync(ms, Path.Combine(args[3],"./MyTestFile.txt").Replace("\\", "/"), CancellationToken.None);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
    
    Console.WriteLine($"File: {(client.Exists("./MyTestFile.txt") ? "Created" : "Not created")}");
    
    Console.WriteLine("===================================================================================");
    Console.WriteLine("Click enter to Remove file:");
    Console.WriteLine("===================================================================================");
    Console.ReadLine();

    try
    {
        await  client.DeleteFileAsync(Path.Combine(args[3],"./MyTestFile.txt").Replace("\\", "/"), CancellationToken.None);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
    
    Console.WriteLine("===================================================================================");
    Console.WriteLine($"File: {(!client.Exists(Path.Combine(args[3],"./MyTestFile.txt").Replace("\\", "/")) ? "Removed" : "Exists")}");
}
finally
{
    Console.WriteLine("==========================================================");
}