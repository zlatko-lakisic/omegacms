$path = (Get-Item -Path ".\..\..\").FullName;
New-Service -Name "Omega Async Task Processor" -BinaryPathName "$path\MD.Tools.AsyncTask.Processor.exe";