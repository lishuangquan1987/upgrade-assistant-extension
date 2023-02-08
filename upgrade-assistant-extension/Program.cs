// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using upgrade_assistant_extension;

Console.WriteLine("Hello, World!");


var files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.csproj", SearchOption.AllDirectories);

if (files.Length == 0)
{
    Console.WriteLine("没有发现项目(.csproj)文件");
    Console.ReadLine();
    return;
}

Console.WriteLine($"共发现{files.Length}个项目：");
foreach (var file in files)
{
    Console.WriteLine($"{Path.GetFileName(Path.GetDirectoryName(file))}({file})");
}

Console.WriteLine("=======开始=========");
Parallel.ForEach(files, file =>
{
    Stopwatch stopwatch = Stopwatch.StartNew();

    var url = new Uri(file, UriKind.Absolute);
    var url_base = new Uri(AppDomain.CurrentDomain.BaseDirectory, UriKind.Absolute);
    var url_relative = url_base.MakeRelativeUri(url);

    string cmd = $" upgrade {url_relative.OriginalString} --non-interactive";
    var result = ProcessHelper.Execute(cmd, (msg, isError) =>
    {
        if (!isError)
        {
            Console.WriteLine($"[正常] {msg}");
        }
        else
        {
            Console.WriteLine($"[错误] {msg}");
        }
    });

    Console.WriteLine($"迁移{file},结果：{result},耗时：{stopwatch.ElapsedMilliseconds}ms");
});
Console.WriteLine("迁移完成");
Console.ReadLine();

